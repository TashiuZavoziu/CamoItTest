namespace CamoItTest.Models {
    public class Mapping {
        public string Name { get; set; }
        public ProductParameter ProductParameter { get; set; }

        public override string ToString() {
            return $"{Name} -> {ProductParameter.Name}";
        }
    }
}