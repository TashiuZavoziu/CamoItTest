using CamoItTest.Hubs;
using CamoItTest.Hubs.HubInterfaces;
using CamoItTest.Services;
using Microsoft.AspNet.SignalR;
using System.Web;
using System.Web.Mvc;
using CamoItTest.Models;

namespace CamoItTest.Controllers {
    public class HomeController : Controller {
        private ConfigurationService _configService;
        private ProductsService _productService;

        public HomeController() {
            _configService = new ConfigurationService();
            _productService = new ProductsService();

        }

        public ActionResult Index() {
            return View();
        }

        [HttpPost]
        public JsonResult UploadProducts(HttpPostedFileBase file, MappingCollection mapping) {
            if (!file.FileName.EndsWith(".csv")) {
                return Json(new { success = false, message = "Нужно выбрать .csv файл" });
            }
            _productService.ProgressUpdated += (percent, message) => {
                var hub = GlobalHost.ConnectionManager.GetHubContext<ProductsHub, IProductsHub>();
                hub.Clients.All.UpdateProgress(percent, message);
            };
            var success = _productService.LoadProductsFile(file.InputStream, mapping);
            if (success) {
                return Json(new { success });
            }
            else {
                return Json(new { success, message = "Произошла ошибка при обработке файла" });
            }
        }

        public JsonResult ProductParameters() {
            return Json(_configService.GetParameters(), JsonRequestBehavior.AllowGet);
        }
    }
}