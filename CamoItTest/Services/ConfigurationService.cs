using CamoItTest.Models;
using CamoItTest.Models.Enums;
using System.Collections.Generic;

namespace CamoItTest.Services {
    public class ConfigurationService {
        public IEnumerable<ProductParameter> GetParameters() {
            var list = new List<ProductParameter> {
                new ProductParameter { Name = "NoMapped", Rule = Parameters.NotMapped },
                new ProductParameter { Name = "SKU", Rule = Parameters.RequiredOne },
                new ProductParameter { Name = "Brand", Rule = Parameters.RequiredOne },
                new ProductParameter { Name = "Price", Rule = Parameters.RequiredOne },
                new ProductParameter { Name = "Weight", Rule = Parameters.NotRequiredOne },
                new ProductParameter { Name = "Feature", Rule = Parameters.NotRequiredMany },
                new ProductParameter { Name = "ProductParameter", Rule = Parameters.NotRequiredMany },
                new ProductParameter { Name = "Ignore", Rule = Parameters.Ignore },
            };
            return list;
        }
    }
}