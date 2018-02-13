using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicDatabase {
    public class DuplicateColumnException : Exception {
        public DuplicateColumnException(string columnName) : base($"Duplicated column '{columnName}'") {

        }

        public DuplicateColumnException(TableColumn column) : this(column.Name) { }
    }
}
