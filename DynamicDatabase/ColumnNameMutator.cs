using System.Collections.Generic;

namespace DynamicDatabase {
    public delegate TableColumn ColumnNameMutator(TableColumn column, int number, IEnumerable<TableColumn> allColumns);
}
