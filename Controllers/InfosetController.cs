using System.Web.Mvc;
using Upgrade.Services;
using Orchard.ContentManagement;
using Orchard;
using Orchard.Core.Settings.Models;
using Orchard.ContentManagement.FieldStorage.InfosetStorage;
using System;
using MainBit.SeoTags.Models;

namespace MainBit.SeoTags.Controllers
{
    public class InfosetController : Controller
    {
        private readonly IUpgradeService _upgradeService;
        private readonly IOrchardServices _orchardServices;

        public InfosetController(IUpgradeService upgradeService,
            IOrchardServices orchardServices)
        {
            _upgradeService = upgradeService;
            _orchardServices = orchardServices;
        }

        public ActionResult Index()
        {
            var parts = _orchardServices.ContentManager
                .Query<SeoTagsPart, SeoTagsPartRecord>().List();

            foreach (var part in parts)
            {
                part.Title = part.Title;
                part.Keywords = part.Keywords;
                part.Description = part.Description;
                part.Canonical = part.Canonical;
            }

            return null;
        }

        public ActionResult Index2()
        {
            var site = _orchardServices.WorkContext.CurrentSite.As<SiteSettingsPart>();

            var seoTagsTable = _upgradeService.GetPrefixedTableName("MainBit_SeoTags_SeoTagsPartRecord");
            if (_upgradeService.TableExists(seoTagsTable))
            {
                //_upgradeService.ExecuteReader("SELECT * FROM " + seoTagsTable, (reader, connection) =>
                //{
                //    lastContentItemId = (int)reader["Id"];
                //    var menuItemPart = _orchardServices.ContentManager.Get(lastContentItemId);
                //    if (menuItemPart != null)
                //    {
                //        menuItemPart.As<InfosetPart>().Store("MenuItemPart", "Url", ConvertToString(reader["Url"]));
                //    }

                //    site.As<InfosetPart>().Store("SeoTagsPart", "Title", ConvertToString(reader["Title"]));
                //    site.As<InfosetPart>().Store("SeoTagsPart", "Keywords", ConvertToString(reader["Keywords"]));
                //    site.As<InfosetPart>().Store("SeoTagsPart", "Description", ConvertToString(reader["Description"]));
                //    site.As<InfosetPart>().Store("SeoTagsPart", "Canonical", ConvertToString(reader["Canonical"]));
                //});

                _upgradeService.ExecuteReader("DROP TABLE " + seoTagsTable, null);
            }

            return null;
        }

        private static string ConvertToString(object readerValue)
        {
            return readerValue == DBNull.Value ? null : (string)readerValue;
        }
    }
}