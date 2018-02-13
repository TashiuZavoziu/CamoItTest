using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace DynamicDatabase {
    public class DynamicEntity {
        private Dictionary<string, object> _values = new Dictionary<string, object>();

        public DynamicEntity() {

        }

        public DynamicEntity(IEnumerable<string> columns) {
            foreach (var column in columns) {
                AddValue(column, null);
            }
        }

        public ReadOnlyDictionary<string, object> Values => new ReadOnlyDictionary<string, object>(_values);
        public string[] Columns => _values.Keys.ToArray();

        public object this[string column] {
            get {
                if (!_values.ContainsKey(column)) {
                    throw new ColumnNotExistsException(column);
                }
                return _values[column];
            }
            set {
                if (!_values.ContainsKey(column)) {
                    throw new ColumnNotExistsException(column);
                }
                _values[column] = value;
            }
        }

        public void AddValues(IEnumerable<string> columns, IEnumerable<object> values) {
            var count = columns.Count();
            if (count != values.Count()) {
                throw new UnequalCountException(nameof(columns), nameof(values));
            }
            var colArray = columns.ToArray();
            var valArray = values.ToArray();
            for (int i = 0; i < count; i++) {
                AddValue(colArray[i], valArray[i]);
            }
        }

        public void AddValues(Dictionary<string, object> values, IEnumerable<string> columns, bool allowNonexistentColumns = false) {
            foreach (var column in columns) {
                if (values.ContainsKey(column)) {
                    AddValue(column, values[column]);
                }
                else {
                    if (!allowNonexistentColumns) {
                        throw new ColumnNotExistsException(column);
                    }
                }
            }
        }

        public void AddValue(string column, object value) {
            if (_values.ContainsKey(column)) {
                throw new DuplicateColumnException(column);
            }
            _values.Add(column, value);
        }
    }
}
