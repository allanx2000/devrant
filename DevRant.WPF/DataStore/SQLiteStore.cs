using Innouvous.Utils.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DevRant.WPF.DataStore
{
    public class SQLiteStore : SQLiteClient, IPersistentDataStore
    {
        private const string Filename = "history.db";
        
        public static SQLiteStore CreateInstance(string folder = null)
        {
            string path = Filename;

            if (folder != null)
                path = Path.Combine(folder, Filename);

            bool exists = File.Exists(path);

            SQLiteStore db = new SQLiteStore(path, exists);
            return db;
        }

        public string DBPath
        {
            get
            {
                return dbPath;
            }
        }

        private readonly string dbPath;

        public SQLiteStore(string path, bool exists) : base(path, !exists)
        {
            this.dbPath = path;

            CreateQueries();

            if (!exists)
                CreateAllTables();
            else
                Upgrade();
        }

        private void Upgrade()
        {
            if (!SQLUtils.CheckTableExists(TableDrafts, this))
            {
                ExecuteNonQuery(CreateDraftsTableQuery);
            }
        }

        private const string TableRead = "tbl_read";
        private const string TableDrafts = "tbl_drafts";

        private string CreateReadTableQuery;
        private string CreateDraftsTableQuery;
        
        #region Create Queries
        private void CreateQueries()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("CREATE TABLE {0} (");
            sb.AppendLine("PostID       INTEGER NOT NULL PRIMARY KEY,");
            sb.AppendLine("Added        datetime NOT NULL");
            sb.AppendLine(")");

            CreateReadTableQuery = string.Format(sb.ToString(), TableRead);
            sb.Clear();

            sb.AppendLine("CREATE TABLE {0} (");
            sb.AppendLine("ID           INTEGER NOT NULL PRIMARY KEY,");
            sb.AppendLine("TextString   TEXT NOT NULL,");
            sb.AppendLine("ImagePath    VARCHAR(400),");
            sb.AppendLine("TagString    VARCHAR(100)");
            sb.AppendLine(")");

            CreateDraftsTableQuery = string.Format(sb.ToString(), TableDrafts);
            sb.Clear();


            InsertReadQuery = "INSERT INTO {0} VALUES({1},'{2}')";
            FindReadQuery = "SELECT COUNT(*) FROM {0} WHERE PostID = {1}";

            InsertDraftQuery = "INSERT INTO {0} VALUES(NULL,'{1}',{2},{3})";
            UpdateDraftQuery = "UPDATE {0} SET TextString = '{2}', ImagePath={3}, TagString={4} WHERE ID = {1}";
        }

        private void CreateAllTables()
        {
            ExecuteNonQuery(CreateReadTableQuery);
            ExecuteNonQuery(CreateDraftsTableQuery);
        }

        #endregion

        #region Read
        private string InsertReadQuery;
        private string FindReadQuery;

        public bool IsRead(int postId)
        {
            string query = string.Format(FindReadQuery, TableRead, postId);
            int count = Convert.ToInt32(ExecuteScalar(query));
            return count > 0;
        }

        public void MarkRead(int postId)
        {
            if (!IsRead(postId))
            {
                string query = string.Format(InsertReadQuery, TableRead, postId, SQLUtils.ToSQLDateTime(DateTime.Now));
                ExecuteNonQuery(query);
            }
        }

        #endregion

        #region Drafts
        private string InsertDraftQuery;
        private string UpdateDraftQuery;

        public void RemoveDraft(long id)
        {
            string cmd = "DELETE FROM {0} WHERE ID = {1}";
            cmd = string.Format(cmd, TableDrafts, id);

            ExecuteNonQuery(cmd);
        }
        
        public void AddDraft(SavedPostContent pc)
        {
            string tags = SQLUtils.SQLEncode(pc.Tags, true, true);
            string image = SQLUtils.SQLEncode(pc.ImagePath, true, true);

            string cmd = string.Format(InsertDraftQuery,
                TableDrafts,
                SQLUtils.SQLEncode(pc.Text),
                image,
                tags
            );

            ExecuteNonQuery(cmd);

            long id = SQLUtils.GetLastInsertRow(this);
            pc.SetId(id);
        }

        public int GetNumberOfDrafts()
        {
            string cmd = "select count(*) FROM {0}";
            cmd = string.Format(cmd, TableDrafts);

            var r = ExecuteScalar(cmd);
            
            int count = Convert.ToInt32(r);
            return count;
        }

        public List<SavedPostContent> GetDrafts()
        {
            string cmd = "select * FROM {0}";
            cmd = string.Format(cmd, TableDrafts);

            var dt = ExecuteSelect(cmd);

            List<SavedPostContent> results = new List<SavedPostContent>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dt.Rows[i];

                var post = ParsePostContent(row);
                results.Add(post);
            }
            
            return results;
        }

        public SavedPostContent GetDraft(long id)
        {
            string cmd = "select * FROM {0} WHERE ID = {1}";
            cmd = string.Format(cmd, TableDrafts, id);

            var dt = ExecuteSelect(cmd);

            if (dt.Rows.Count > 0)
            {
                SavedPostContent post = ParsePostContent(dt.Rows[0]);
                return post;
            }
            else
                return null;
        }

        private SavedPostContent ParsePostContent(DataRow row)
        {   
            long id = Convert.ToInt64(row["ID"]);
            string text = row["TextString"].ToString();

            object tmp = row["ImagePath"];
            string img = SQLUtils.IsNull(tmp) ? null : tmp.ToString();

            tmp = row["TagString"];
            string tag = SQLUtils.IsNull(tmp) ? null : tmp.ToString();

            SavedPostContent pc = new SavedPostContent(text, id, tag, img);
            return pc;
        }

        public void UpdateDraft(SavedPostContent data)
        {
            string tags = SQLUtils.SQLEncode(data.Tags, true, true);
            string image = SQLUtils.SQLEncode(data.ImagePath, true, true);
            
            string cmd = string.Format(UpdateDraftQuery,
                TableDrafts,
                data.ID,
                SQLUtils.SQLEncode(data.Text),
                image,
                tags
            );

            ExecuteNonQuery(cmd);
        }

        #endregion
    }
}
