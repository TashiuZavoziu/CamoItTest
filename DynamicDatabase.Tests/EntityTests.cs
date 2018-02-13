using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DynamicDatabase.Tests {
    [TestClass]
    public class EntityTests {
        public EntityTests() {
        }

        [TestMethod]
        public void ValuesTest() {
            var entity = new DynamicEntity();
            entity.AddValue("number", 1);
            entity.AddValue("second", "hello");

            var dic = new Dictionary<string, object> {
                { "number", 1 },
                { "second", "hello" },
            };
            CollectionAssert.AreEquivalent(dic, entity.Values);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateColumnException))]
        public void ValuesTest2() {
            var entity = new DynamicEntity();
            entity.AddValue("number", 1);
            entity.AddValue("number", "hello");
        }

        [TestMethod]
        public void ValuesTest3() {
            var entity = new DynamicEntity();
            var columns = new string[] { "first", "second", "hello" };
            var values = new object[] { 25, "nice", 0M };
            entity.AddValues(columns, values);

            var dic = new Dictionary<string, object> {
                { "first", 25 },
                { "second", "nice" },
                { "hello", 0M },
            };
            CollectionAssert.AreEquivalent(dic, entity.Values);
        }

        [TestMethod]
        public void ValuesTest4() {
            var entity = new DynamicEntity();
            entity.AddValue("mm", "twice");
            var columns = new string[] { "first", "second", "hello" };
            var values = new object[] { 25, "nice", 0M };
            entity.AddValues(columns, values);
            entity.AddValue("sql", "mysql");

            var dic = new Dictionary<string, object> {
                { "first", 25 },
                { "second", "nice" },
                { "hello", 0M },
                { "mm", "twice" },
                { "sql", "mysql"}
            };
            CollectionAssert.AreEquivalent(dic, entity.Values);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateColumnException))]
        public void ValuesTest5() {
            var entity = new DynamicEntity();
            entity.AddValue("mm", "twice");
            var columns = new string[] { "first", "second", "hello" };
            var values = new object[] { 25, "nice", 0M };
            entity.AddValues(columns, values);
            entity.AddValue("mm", "mysql");
        }

        [TestMethod]
        [ExpectedException(typeof(UnequalCountException))]
        public void ValuesTest6() {
            var entity = new DynamicEntity();
            var columns = new string[] { "first", "hello" };
            var values = new object[] { 25, "nice", 0M };
            entity.AddValues(columns, values);
        }

        [TestMethod]
        public void ValuesTest7() {
            var entity = new DynamicEntity();
            var columns = new string[] { "first", "second", "hello" };
            var dic = new Dictionary<string, object> {
                { "first", 25 },
                { "second", "nice" },
                { "hello", 0M },
                { "mm", "twice" },
                { "sql", "mysql"}
            };
            entity.AddValues(dic, columns);
            var expected = new Dictionary<string, object> {
                { "first", 25 },
                { "second", "nice" },
                { "hello", 0M }
            };
            CollectionAssert.AreEquivalent(expected, entity.Values);
        }

        [TestMethod]
        [ExpectedException(typeof(ColumnNotExistsException))]
        public void ValuesTest8() {
            var entity = new DynamicEntity();
            var columns = new string[] { "first", "second", "hello", "test" };
            var dic = new Dictionary<string, object> {
                { "first", 25 },
                { "second", "nice" },
                { "hello", 0M },
                { "mm", "twice" },
                { "sql", "mysql"}
            };
            entity.AddValues(dic, columns);
        }

        [TestMethod]
        public void ValuesTest9() {
            var entity = new DynamicEntity();
            var columns = new string[] { "first", "second", "hello", "test" };
            var dic = new Dictionary<string, object> {
                { "first", 25 },
                { "second", "nice" },
                { "hello", 0M },
                { "mm", "twice" },
                { "sql", "mysql"}
            };
            entity.AddValues(dic, columns, true);
            var expected = new string[] { "first", "second", "hello" };
            CollectionAssert.AreEquivalent(expected, entity.Columns);
        }

        [TestMethod]
        public void ValuesTest10() {
            var entity = new DynamicEntity();
            var columns = new string[] { "first", "second", "hello", "test" };
            var dic = new Dictionary<string, object> {
                { "first", 25 },
                { "second", "nice" },
                { "hello", 0M },
                { "mm", "twice" },
                { "sql", "mysql"}
            };
            entity.AddValues(dic, columns, true);
            var expected = new Dictionary<string, object> {
                { "first", 25 },
                { "second", "nice" },
                { "hello", 0M }
            };
            CollectionAssert.AreEquivalent(expected, entity.Values);
        }

        [TestMethod]
        public void ValuesTest11() {
            var entity = new DynamicEntity();
            entity.AddValue("mm", "twice");
            var columns = new string[] { "first", "second", "hello" };
            var values = new object[] { 25, "nice", 0M };
            entity.AddValues(columns, values);
            entity.AddValue("sql", "mysql");

            entity["first"] = 1;
            entity["sql"] = "postgres";

            var dic = new Dictionary<string, object> {
                { "first", 1 },
                { "second", "nice" },
                { "hello", 0M },
                { "mm", "twice" },
                { "sql", "postgres"}
            };
            CollectionAssert.AreEquivalent(dic, entity.Values);
        }
    }
}
