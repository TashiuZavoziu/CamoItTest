using CamoItTest.Models;
using CamoItTest.Models.Enums;
using CamoItTest.Utils;
using DynamicDatabase;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace CamoItTest.Services {
    public class ProductsService : IProgress {
        private DynamicDbContext _context;
        private ConfigurationService _configService;

        public ProductsService() {
            _configService = new ConfigurationService();
            _context = new DynamicDbContext("ProductsDb");
        }

        public event Action<int, string> ProgressUpdated;

        public bool LoadProductsFile(Stream csvFile, MappingCollection mapping) {
            if (!CheckRules(mapping.Select(m => m.ProductParameter.Name))) {
                return false;
            }
            var scheme = new TableScheme();
            scheme.AllowDuplicateColumns = true;
            scheme.PrimaryKey = "SKU";
            var dbMapping = mapping
                .Where(m => m.ProductParameter.Rule != Parameters.Ignore &&
                    m.ProductParameter.Rule != Parameters.NotMapped)
                .Select(m => m);
            foreach (var column in dbMapping.Select(dm => dm.ProductParameter.Name)) {
                AddColumn(scheme, column);
            }
            scheme.GenerateTableName();
            if (!_context.TableExists(scheme.TableName)) {
                _context.CreateTable(scheme);
            }
            ProgressUpdated?.Invoke(0, "Загрузка файла...");
            try {
                using (var sr = new StreamReader(csvFile)) {
                    var headers = sr.ReadLine().Split(',');
                    var mapIndices = new Dictionary<string, int>();
                    var groups = dbMapping.GroupBy(m => m.ProductParameter.Name);
                    foreach (var group in groups) {
                        if (group.Count() == 1) {
                            var map = group.First();
                            mapIndices.Add(map.ProductParameter.Name, Array.IndexOf(headers, map.Name));
                        }
                        else {
                            var groupArray = group.ToArray();
                            for (int i = 0; i < groupArray.Length; i++) {
                                var map = groupArray[i];
                                var name = scheme.ColumnNameMutator(new TableColumn(map.ProductParameter.Name), i, null);
                                mapIndices.Add(name.Name, Array.IndexOf(headers, map.Name));
                            }
                        }
                    }
                    var table = _context.GetTable(scheme);
                    while (!sr.EndOfStream) {
                        var line = sr.ReadLine();
                        if (line != null) {
                            var values = line.Split(',');
                            var entity = table.CreateDynamicEntity();
                            foreach (var index in mapIndices) {
                                entity[index.Key] = values[index.Value];
                            }
                            table.InsertOnCommit(entity);
                            ProgressUpdated?.Invoke((int)((float)sr.BaseStream.Position * 100 / sr.BaseStream.Length), "Загрузка файла...");
                        }
                    }
                }
                ProgressUpdated?.Invoke(100, "Сохранение в базу данных...");
                _context.CommitChanges();
            }
            catch (SqlException ex) when (ex.Number == 2627) {
                ProgressUpdated?.Invoke(0, "Нарушение ограничения первичного ключа");
                return false;
            }
            catch (SqlException ex) {
                ProgressUpdated?.Invoke(0, "Ошибка при добавлении в базу данных");
                return false;
            }
            catch (Exception ex) {
                ProgressUpdated?.Invoke(0, "Произошла ошибка");
                return false;
            }
            finally {
                _context.Connection.Close();
            }
            ProgressUpdated?.Invoke(100, "Завершено");
            return true;
        }

        private void AddColumn(TableScheme scheme, string column) {
            TableColumn col;
            switch (column) {
                case "SKU":
                    col = new TableColumn(column, System.Data.SqlDbType.NVarChar, maxLength: 100);
                    break;
                case "Brand":
                    col = new TableColumn(column, System.Data.SqlDbType.NVarChar, maxLength: 100);
                    break;
                case "Price":
                    col = new TableColumn(column, System.Data.SqlDbType.Decimal);
                    break;
                case "Weight":
                    col = new TableColumn(column, System.Data.SqlDbType.NVarChar, maxLength: 50);
                    break;
                case "Feature":
                    col = new TableColumn(column, System.Data.SqlDbType.NVarChar, maxLength: 200);
                    break;
                case "Product parameter":
                    col = new TableColumn(column, System.Data.SqlDbType.NVarChar, maxLength: 200);
                    break;
                default:
                    col = new TableColumn(column);
                    break;
            }
            scheme.AddColumn(col);
        }

        private bool CheckRules(IEnumerable<string> columns) {
            var rules = _configService.GetParameters();
            foreach (var rule in rules) {
                var count = columns.Where(c => c == rule.Name).Count();
                switch (rule.Rule) {
                    case Parameters.RequiredOne:
                        if (count != 1) {
                            return false;
                        }
                        break;
                    case Parameters.RequiredMany:
                        if (count < 1) {
                            return false;
                        }
                        break;
                    case Parameters.NotRequiredOne:
                        if (count > 1) {
                            return false;
                        }
                        break;
                    case Parameters.NotRequiredMany:
                    case Parameters.NotMapped:
                    case Parameters.Ignore:
                    default:
                        break;
                }
            }
            return true;
        }
    }
}