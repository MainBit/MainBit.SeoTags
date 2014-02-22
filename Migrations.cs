using System;
using System.Collections.Generic;
using System.Data;
using Orchard.ContentManagement.Drivers;
using Orchard.ContentManagement.MetaData;
using Orchard.ContentManagement.MetaData.Builders;
using Orchard.Core.Contents.Extensions;
using Orchard.Data.Migration;

namespace MainBit.SeoTags {
    public class Migrations : DataMigrationImpl {

        public int Create() {

            SchemaBuilder.CreateTable("SeoTagsPartRecord", table => table
                .ContentPartRecord()
                .Column("Title", DbType.String)
                .Column("Keywords", DbType.String)
                .Column("Description", DbType.String)
                .Column("Canonical", DbType.String)
            );

            ContentDefinitionManager.AlterPartDefinition(
                "SeoTagsPart", cfg => cfg.Attachable());

            return 1;
        }
    }
}