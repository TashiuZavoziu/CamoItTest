using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicDatabase;
using System.Collections.Generic;
using System.Linq;

namespace DynamicDatabase.Tests {
    [TestClass]
    public class TableTests {
        private TableColumn Mutator(TableColumn col, int num, IEnumerable<TableColumn> cols) {
            return col.Clone();
        }

        [TestMethod]
        public void TestTableName() {
            string name = "somename";
            var scheme = new TableScheme(name);

            Assert.AreEqual(name, scheme.TableName);
        }

        [TestMethod]
        public void TestTableName2() {
            string name = "somename";
            var scheme = new TableScheme("another");
            scheme.TableName = name;

            Assert.AreEqual(name, scheme.TableName);
        }

        [TestMethod]
        public void TestAddColumn() {
            var scheme = new TableScheme();
            scheme.AddColumn("first");
            scheme.AddColumn("second");
            scheme.AddColumn("fouth");
            scheme.AddColumn("4");

            var expected = new TableColumn[] {
                new TableColumn("first"),
                new TableColumn("second"),
                new TableColumn("fouth"),
                new TableColumn("4")
            };
            CollectionAssert.AreEquivalent(expected, scheme.GetColumns());
        }

        [TestMethod]
        public void TestAddColumn2() {
            var scheme = new TableScheme();
            scheme.AllowDuplicateColumns = true;
            scheme.AddColumn("ivan");
            scheme.AddColumn("none");
            scheme.AddColumn("ivan");
            scheme.AddColumn("heh");

            var expected = new TableColumn[] {
                new TableColumn("ivan1"),
                new TableColumn("none"),
                new TableColumn("heh"),
                new TableColumn("ivan2")
            };
            CollectionAssert.AreEquivalent(expected, scheme.GetColumns());
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateColumnException))]
        public void TestAddColumn3() {
            var scheme = new TableScheme();

            scheme.AddColumn("ivan");
            scheme.AddColumn("none");
            scheme.AddColumn("ivan");
            scheme.AddColumn("heh");
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateColumnException))]
        public void TestGetColumns() {
            var scheme = new TableScheme();
            scheme.AllowDuplicateColumns = true;
            scheme.ColumnNameMutator = Mutator;
            scheme.AddColumn("ivan");
            scheme.AddColumn("none");
            scheme.AddColumn("ivan");
            scheme.AddColumn("heh");

            var cols = scheme.GetColumns();

        }
    }
}
