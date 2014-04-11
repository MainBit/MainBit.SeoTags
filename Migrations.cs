using Orchard.ContentManagement.MetaData;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace MainBit.SeoTags {
    public class Migrations : DataMigrationImpl {

        public int Create() {
            ContentDefinitionManager.AlterPartDefinition("SeoTagsPart", builder => builder
                .Attachable()
                .WithDescription("Title, Keywords, Description, Canonical, Pagging.")
            );

            return 1;
        }
    }
}