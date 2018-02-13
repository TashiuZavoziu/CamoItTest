using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDatabase {
    internal static class ColumnBuilder {
        public static string NullContraint(bool nullable) {
            return nullable ? "NULL" : "NOT NULL";
        }

        public static string Length(int? length) {
            return length.HasValue ? $"({length})" : string.Empty;
        }

        public static string Identity(bool identity) {
            return identity ? "IDENTITY" : string.Empty;
        }

        public static string PrimaryKey(string key) {
            return key == null ? string.Empty : $"PRIMARY KEY ({key})";
        }
    }
}
