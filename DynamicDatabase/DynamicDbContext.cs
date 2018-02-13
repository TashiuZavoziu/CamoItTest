using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using static DynamicDatabase.ColumnBuilder;

namespace DynamicDatabase {
    public class DynamicDbContext {
        private SqlConnection _connection;
        public SqlTransaction Transaction { get; set; }

        public SqlConnection Connection => _connection;

        public DynamicDbContext(SqlConnection connection) {
            _connection = connection;
            _connection.Open();
            Transaction = _connection.BeginTransaction();
        }

        public DynamicDbContext(string dbconnection) {
            _connection = new SqlConnection(ConfigurationManager.ConnectionStrings[dbconnection].ConnectionString);
            _connection.Open();
            Transaction = _connection.BeginTransaction();
        }

        private SqlConnection GetConnection() {
            return new SqlConnection(_connection.ConnectionString);
        }

        public void CreateTable(TableScheme scheme) {
            var connection = GetConnection();
            connection.Open();
            var create = new StringBuilder($"create table [{scheme.TableName}] (");
            foreach (var column in scheme.GetColumns()) {
                create.Append($"[{column.Name}] {column.StoreType}{Length(column.MaxLength)} {NullContraint(column.IsNullable)} {Identity(column.IsIdentity)},");
            }
            if (scheme.PrimaryKey != null) {
                create.Append(PrimaryKey(scheme.PrimaryKey));
            }
            else {
                create.Length--;
            }
            create.Append(")");
            var command = new SqlCommand(create.ToString(), connection);
            command.ExecuteNonQuery();
            connection.Close();
        }

        public bool TableExists(string tableName) {
            var connnection = GetConnection();
            connnection.Open();
            const string cmd = @"SELECT count(*) 
                 FROM INFORMATION_SCHEMA.TABLES
                 WHERE TABLE_SCHEMA = 'dbo'
                 AND TABLE_NAME = @tableName";
            var command = new SqlCommand(cmd, connnection);
            command.Parameters.AddWithValue("@tableName", tableName);
            var count = (int)command.ExecuteScalar();
            connnection.Close();
            return count > 0;
        }

        public DynamicTable GetTable(TableScheme scheme) {
            return new DynamicTable(this, scheme);
        }

        public void CommitChanges() {
            Transaction.Commit();
        }

    }
}
