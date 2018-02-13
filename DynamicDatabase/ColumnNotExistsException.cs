using System;

namespace DynamicDatabase {
    public class ColumnNotExistsException : Exception {
        public ColumnNotExistsException(string column) : base($"Column '{column}' doesn't exist") {

        }
    }
}
