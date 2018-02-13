using CamoItTest.Binders;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CamoItTest.Models {
    [ModelBinder(typeof(MappingBinder))]
    public class MappingCollection : List<Mapping> { }

}