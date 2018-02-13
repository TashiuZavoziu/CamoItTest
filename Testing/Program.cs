using DynamicDatabase;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testing {
    class Program {
        static void Database() {
            var scheme = new TableScheme("prd");
            scheme.AllowDuplicateColumns = true;
            scheme.PrimaryKey = "sku";

            scheme.AddColumn(new TableColumn("sku", System.Data.SqlDbType.NVarChar, maxLength: 100));
            scheme.AddColumn("feature");

            var cols = scheme.GetColumns();
            var str = ConfigurationManager.ConnectionStrings["ProductsDb"].ConnectionString;
            var context = new DynamicDbContext(str);
            var table = context.GetTable(scheme);
            var entity = table.CreateDynamicEntity();
            entity["sku"] = "some key2";
            entity["feature"] = "nice";
            table.InsertOnCommit(entity);

            var entity2 = table.CreateDynamicEntity();
            entity2["sku"] = "2";
            entity2["feature"] = "hello";
            table.InsertOnCommit(entity2);
            context.CommitChanges();
        }

        static void Main(string[] args) {
            var arr = new string[] { "first", "second", "one", "first", "first", "one" };
            var groups = arr.GroupBy(s => s);
            foreach (var group in groups) {
                foreach (var item in group) {
                    var count = group.Count();
                    if (count > 1) {
                        Console.WriteLine($"{item}: {count}");
                    }
                }
            }
        }

    }
}
