using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc;

namespace Nop.Plugin.Widgets.ProductNotification.Models
{
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }


        [NopResourceDisplayName("Nop.Plugin.Widgets.ProductNotification.Text")]
        [UIHint("Text")]
        public string Text { get; set; }
        public bool Text_OverrideForStore { get; set; }
    }
}