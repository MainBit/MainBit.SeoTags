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

        public int UpdateFrom1() {

            SchemaBuilder.CreateTable("SeoTagsPartRecord",
                table => table
                    .ContentPartRecord()
                        .Column<string>("Title", c => c.WithLength(2048))
                        .Column<bool>("Keywords", c => c.WithDefault(2048))
                        .Column<string>("Description", c => c.WithLength(2048))
                        .Column<string>("Canonical", c => c.WithLength(2048)));

            return 2;
        }
    }
}