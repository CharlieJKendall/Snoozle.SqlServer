using Snoozle.SqlServer;
using System;

namespace Snoozle.TestHarness.RestResources
{
    public class Cat : IRestResource
    {
        public int Id { get; set; }

        public int? HairLength { get; set; }

        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public long? UnconfiguredProperty { get; set; }
    }

    public class CatResourceConfiguration : SqlResourceConfigurationBuilder<Cat>
    {
        public override void Configure()
        {
            ConfigurationForModel().HasTableName("Cats").HasAllowedHttpVerbs(HttpVerbs.All).HasRoute("cattyboys");

            ConfigurationForProperty(x => x.HairLength).HasColumnName("HairLengthInMeters");
            ConfigurationForProperty(x => x.Id).HasColumnName("CatId").IsPrimaryIdentifier();
            ConfigurationForProperty(x => x.DateCreated).HasComputedValue(HttpVerbs.POST).DateTimeNow();
            ConfigurationForProperty(x => x.Name).HasColumnName("WrongColumnName");
            ConfigurationForProperty(x => x.Name).HasColumnName("CatName").HasComputedValue(HttpVerbs.POST | HttpVerbs.PUT).Custom(() => "GARY");
        }
    }
}
