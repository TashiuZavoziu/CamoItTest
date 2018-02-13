using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DynamicDatabase {
    public class DynamicTable {
        public DynamicTable(DynamicDbContext context, TableScheme scheme) {
            Context = context;
            Scheme = scheme;
            Entities = new List<DynamicEntity>();
        }

        public TableScheme Scheme { get; }
        public IEnumerable<DynamicEntity> Entities { get; set; }
        public DynamicDbContext Context { get; }

        private IEnumerable<string> _cols;
        private IEnumerable<string> _Columns => _cols ?? (_cols = Scheme.GetColumns().Select(c => c.Name));

        public DynamicEntity CreateDynamicEntity() {
            return new DynamicEntity(_Columns);
        }

        public int InsertOnCommit(DynamicEntity entity) {
            var columns = new StringBuilder();
            var values = new StringBuilder();
            var command = Context.Connection.CreateCommand();
            foreach (var pair in entity.Values) {
                columns.Append($"[{pair.Key}],");
                values.Append($"@{pair.Key},");
                command.Parameters.AddWithValue(pair.Key, pair.Value);
            }
            columns.Length--;
            values.Length--;
            command.CommandText = $"insert into [{ Scheme.TableName}] ({columns}) values ({values})";
            command.Transaction = Context.Transaction;
            return command.ExecuteNonQuery();
        }
    }
}
