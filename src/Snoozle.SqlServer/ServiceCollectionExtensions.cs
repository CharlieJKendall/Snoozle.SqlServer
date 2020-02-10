using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snoozle.Abstractions;
using Snoozle.SqlServer.Configuration;
using Snoozle.SqlServer.Implementation;
using Snoozle.SqlServer.Internal;
using Snoozle.SqlServer.Internal.Wrappers;
using System;
using System.Collections.Generic;

namespace Snoozle.SqlServer
{
    public static class ServiceCollectionExtensions
    {
        public static IMvcBuilder AddSnoozleSqlServer(this IMvcBuilder @this, IConfigurationSection configurationSection)
        {
            @this.Services.Configure<SnoozleSqlServerOptions>(options => configurationSection.Bind(options));

            return AddSnoozleSqlServer(@this);
        }

        public static IMvcBuilder AddSnoozleSqlServer(this IMvcBuilder @this, Action<SnoozleSqlServerOptions> optionsBuilder)
        {
            @this.Services.Configure(optionsBuilder);

            return AddSnoozleSqlServer(@this);
        }

        private static IMvcBuilder AddSnoozleSqlServer(this IMvcBuilder @this)
        {
            IServiceCollection serviceCollection = @this.Services;
            ISqlRuntimeConfigurationProvider runtimeConfigurationProvider = BuildRuntimeConfigurationProvider();

            serviceCollection.Configure<SnoozleSqlServerOptions>(options => new SnoozleSqlServerOptions());
            serviceCollection.AddScoped<ISqlExecutor, SqlExecutor>();
            serviceCollection.AddScoped<IDataProvider, SqlDataProvider>();
            serviceCollection.AddScoped<ISqlClassProvider, SqlClassProvider>();
            serviceCollection.AddSingleton(runtimeConfigurationProvider);

            @this.AddSnoozleCore(runtimeConfigurationProvider);

            return @this;
        }

        private static ISqlRuntimeConfigurationProvider BuildRuntimeConfigurationProvider()
        {
            IEnumerable<ISqlResourceConfiguration> resourceConfigurations =
                ResourceConfigurationBuilder.Build<ISqlPropertyConfiguration, ISqlResourceConfiguration, ISqlModelConfiguration>(typeof(SqlResourceConfigurationBuilder<>));

            // No need to register these with the DI container, as they are only used during startup
            ISqlParamaterProvider sqlParamaterProvider = new SqlParameterProvider();
            ISqlGenerator generator = new SqlGenerator(sqlParamaterProvider);
            ISqlExpressionBuilder sqlExpressionBuilder = new SqlExpressionBuilder(sqlParamaterProvider, new SqlClassProvider());
            Dictionary<Type, ISqlRuntimeConfiguration<IRestResource>> runtimeConfigurations = new Dictionary<Type, ISqlRuntimeConfiguration<IRestResource>>();

            foreach (ISqlResourceConfiguration configuration in resourceConfigurations)
            {
                string selectAll = generator.SelectAll(configuration);
                string selectById = generator.SelectById(configuration);
                string deleteById = generator.DeleteById(configuration);
                string updateById = generator.Update(configuration);
                string insert = generator.Insert(configuration);

                var createObjectRelationalMapFunc = typeof(ISqlExpressionBuilder)
                    .GetMethod(nameof(ISqlExpressionBuilder.CreateObjectRelationalMap))
                    .MakeGenericMethod(configuration.ResourceType)
                    .Invoke(sqlExpressionBuilder, new[] { configuration }) as Func<IDatabaseResultReader, IRestResource>;

                var getPrimaryKeySqlParameterFunc = sqlExpressionBuilder.GetPrimaryKeySqlParameter(configuration.PrimaryIdentifier);
                var getNonPrimaryKeySqlParametersFunc = sqlExpressionBuilder.GetNonPrimaryKeySqlParameters(configuration);

                var runtimeConfiguration = Activator.CreateInstance(
                    typeof(SqlRuntimeConfiguration<>).MakeGenericType(configuration.ResourceType),
                    configuration,
                    createObjectRelationalMapFunc,
                    getPrimaryKeySqlParameterFunc,
                    getNonPrimaryKeySqlParametersFunc,
                    selectAll,
                    selectById,
                    deleteById,
                    insert,
                    updateById) as ISqlRuntimeConfiguration<IRestResource>;

                runtimeConfigurations.Add(configuration.ResourceType, runtimeConfiguration);
            }

            return new SqlRuntimeConfigurationProvider(runtimeConfigurations);
        }
    }
}