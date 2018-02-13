using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace DynamicDatabase {
    using ColumnEntry = KeyValuePair<TableColumn, int>;

    public class TableScheme {
        private Dictionary<TableColumn, int> _dupColumns = new Dictionary<TableColumn, int>();

        public string PrimaryKey { get; set; }
        public string TableName { get; set; }
        public bool AllowDuplicateColumns { get; set; }
        public ColumnNameMutator ColumnNameMutator { get; set; }

        public TableScheme() {
            ColumnNameMutator = DefaultMutator;
        }

        public TableScheme(string name) : this() {
            TableName = name;
        }

        public TableScheme(ColumnNameMutator mutator) {
            ColumnNameMutator = mutator;
        }

        public TableScheme(string name, ColumnNameMutator mutator) {
            TableName = name;
            ColumnNameMutator = mutator;
        }

        public void AddColumn(string columnName) {
            AddColumn(new TableColumn(columnName));
        }

        public void AddColumn(TableColumn column) {
            if (_dupColumns.ContainsKey(column)) {
                if (!AllowDuplicateColumns) {
                    throw new DuplicateColumnException(column);
                }
                else {
                    _dupColumns[column]++;
                }
            }
            else {
                _dupColumns.Add(column, 1);
            }
        }

        public TableColumn[] GetColumns() {
            var columns = new HashSet<TableColumn>();
            bool error = false;
            try {
                foreach (var colEntry in _dupColumns) {
                    if (IsDuplicated(colEntry)) {
                        for (int i = 0; i < colEntry.Value; i++) {
                            var col = ColumnNameMutator(colEntry.Key, i, _dupColumns.Keys);
                            if (!columns.Add(col)) {
                                error = true;
                                throw new DuplicateColumnException(col);
                            }
                        }
                    }
                    else {
                        if (!columns.Add(colEntry.Key)) {
                            error = true;
                            throw new DuplicateColumnException(colEntry.Key);
                        }
                    }
                }
            }
            finally {
                if (error) {
                    columns.Clear();
                }
            }
            return columns.ToArray();
        }

        private bool IsDuplicated(ColumnEntry entry) {
            return entry.Value != 1;
        }

        public void GenerateTableName() {
            var md5 = MD5.Create();
            var bytes = Encoding.UTF8.GetBytes(string.Join("", GetColumns().Select(c => c.Name).ToArray()));
            var hash = md5.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                sb.Append(hash[i].ToString("X2"));
            }
            TableName = sb.ToString();
        }

        public static TableColumn DefaultMutator(TableColumn column, int number, IEnumerable<TableColumn> allColumns) {
            var col = column.Clone();
            col.Name = $"{column.Name}{number + 1}";
            return col;
        }
    }
}
