using Innouvous.Utils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevRant.WPF.DataStore
{
    public class HistoryStore : SQLiteClient, IHistoryStore
    {
        private const string Filename = "history.db";

        private static HistoryStore instance;

        public static HistoryStore Instance
        {
            get
            {
                if (instance == null)
                {
                    bool exists = File.Exists(Filename);
                    instance = new HistoryStore(exists);
                }

                return instance;
            }
        }

        public HistoryStore(bool exists) : base(Filename, !exists)
        {
            CreateQueries();

            if (!exists)
                CreateTables();
        }

        private const string TableRead = "tbl_read";
        private string CreateTableQuery;
        private string InsertQuery;
        private string FindQuery;

        private void CreateQueries()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CREATE TABLE {0} (");
            sb.AppendLine("PostID       INTEGER NOT NULL PRIMARY KEY,");
            sb.AppendLine("Added        datetime NOT NULL");
            sb.AppendLine(")");

            CreateTableQuery = string.Format(sb.ToString(), TableRead);
            sb.Clear();

            InsertQuery = "INSERT INTO {0} VALUES({1},'{2}')";
            FindQuery = "SELECT COUNT(*) FROM {0} WHERE PostID = {1}";
        }

        private void CreateTables()
        {
            ExecuteNonQuery(CreateTableQuery);
        }

        public bool IsRead(int postId)
        {
            string query = string.Format(FindQuery, TableRead, postId);
            int count = Convert.ToInt32(ExecuteScalar(query));
            return count > 0;
        }

        public void MarkRead(int postId)
        {
            string query = string.Format(InsertQuery, TableRead, postId, SQLUtils.ToSQLDateTime(DateTime.Now));
            ExecuteNonQuery(query);
        }
    }
}
