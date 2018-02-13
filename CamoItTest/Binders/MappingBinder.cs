using CamoItTest.Models;
using Newtonsoft.Json;
using System.Web.Mvc;

namespace CamoItTest.Binders {
    public class MappingBinder : IModelBinder {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext) {
            var mapping = controllerContext.HttpContext.Request.Form.Get(bindingContext.ModelName);
            switch (controllerContext.HttpContext.Request.Headers["Content-Type"]) {
                case "application/json":
                default:
                    return JsonConvert.DeserializeObject<MappingCollection>(mapping);
            }
        }
    }
}