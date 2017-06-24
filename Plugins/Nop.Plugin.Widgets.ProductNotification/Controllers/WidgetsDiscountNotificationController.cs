using System.Linq;
using System.Web.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Widgets.ProductNotification.Models;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Stores;
using Nop.Web.Framework.Controllers;
using Nop.Plugin.Widgets.ProductNotification;

namespace Nop.Plugin.Widgets.ProductNotification.Controllers
{
    public class WidgetsProductNotificationController : BasePluginController
    {
        private readonly IWorkContext _workContext;
        private readonly IStoreContext _storeContext;
        private readonly IStoreService _storeService;
        private readonly ISettingService _settingService;
        private readonly ICacheManager _cacheManager;
        private readonly ILocalizationService _localizationService;

        public WidgetsProductNotificationController(IWorkContext workContext,
            IStoreContext storeContext,
            IStoreService storeService, 
            IPictureService pictureService,
            ISettingService settingService,
            ICacheManager cacheManager,
            ILocalizationService localizationService)
        {
            this._workContext = workContext;
            this._storeContext = storeContext;
            this._storeService = storeService;
            this._settingService = settingService;
            this._cacheManager = cacheManager;
            this._localizationService = localizationService;
        }

        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure()
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var productNotificationSettings = _settingService.LoadSetting<ProductNotificationSettings>(storeScope);
            var model = new ConfigurationModel();
            model.Text = productNotificationSettings.Text;
            model.ActiveStoreScopeConfiguration = storeScope;
            if (storeScope > 0)
            {
                model.Text_OverrideForStore = _settingService.SettingExists(productNotificationSettings, x => x.Text, storeScope);
               
            }

            return View("~/Plugins/Widgets.ProductNotification/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            //load settings for a chosen store scope
            var storeScope = this.GetActiveStoreScopeConfiguration(_storeService, _workContext);
            var productNotificationSettings = _settingService.LoadSetting<ProductNotificationSettings>(storeScope);


            productNotificationSettings.Text = model.Text;
     

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSettingOverridablePerStore(productNotificationSettings, x => x.Text, model.Text_OverrideForStore, storeScope, false);
            
            //now clear settings cache
            _settingService.ClearCache();
            

            SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));
            return Configure();
        }

        [ChildActionOnly]
        public ActionResult PublicInfo(string widgetZone, object additionalData = null)
        {
            var productNotificationSettings = _settingService.LoadSetting<ProductNotificationSettings>(_storeContext.CurrentStore.Id);

            var model = new PublicInfoModel();
            model.Text = productNotificationSettings.Text;
           

            return View("~/Plugins/Widgets.ProductNotification/Views/PublicInfo.cshtml", model);
        }
    }
}