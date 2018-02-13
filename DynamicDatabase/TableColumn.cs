using System.Data;

namespace DynamicDatabase {
    public class TableColumn {
        public TableColumn(string name = null, SqlDbType dbtype = SqlDbType.NText, bool identity = false, bool nullable = false, bool? fixedLength = default(bool?), bool? unicode = default(bool?), int? maxLength = default(int?), byte? precision = default(byte?)) {
            Name = name;
            DbType = dbtype;
            IsIdentity = identity;
            IsNullable = nullable;
            IsFixedLength = fixedLength;
            IsUnicode = unicode;
            MaxLength = maxLength;
            Precision = precision;
        }

        public string Name { get; set; }
        public SqlDbType DbType { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsNullable { get; set; }
        public bool? IsFixedLength { get; set; }
        public bool? IsUnicode { get; set; }
        public int? MaxLength { get; set; }
        public byte? Precision { get; set; }
        public string StoreType => DbType.ToString();

        public override bool Equals(object obj) {
            var other = obj as TableColumn;
            return other != null && other.Name == Name;
        }

        public override int GetHashCode() {
            return Name.GetHashCode();
        }

        public TableColumn Clone() {
            return new TableColumn {
                Name = Name,
                DbType = DbType,
                IsIdentity = IsIdentity,
                IsNullable = IsNullable,
                IsFixedLength = IsFixedLength,
                IsUnicode = IsUnicode,
                MaxLength = MaxLength,
                Precision = Precision
            };
        }

        public override string ToString() {
            return Name;
        }
    }
}
