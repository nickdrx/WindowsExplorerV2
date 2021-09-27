using System;
using System.Collections.Generic;
using System.Windows;
using FileExplorer.Objects;
using Npgsql;

namespace FileExplorer.SQL
{
    class Connection
    {
        // Obtain connection string information from the portal
        //
        private static string Host = "localhost";
        private static string User = "postgres";
        private static string DBname = "Explorer";
        private static string Password = "root";
        private static string Port = "5433";
        List<EFile> ScanFiles = new List<EFile>();
        List<EFile> SearchFiles = new List<EFile>();


        string connString =
                String.Format(
                    "Server={0}; User Id={1}; Database={2}; Port={3}; Password={4};SSLMode=Prefer",
                    Host,
                    User,
                    DBname,
                    Port,
                    Password);

        public Connection()
        {
            // Build connection string using parameters from portal
            //

            bool TableExists = checkTableExists();
            if (TableExists == false)
            {
                createDB();
                createTable();
            }

            dropTable();
            createTable();
        }

        public void dropTable()
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "DROP TABLE files";
                    cmd.ExecuteNonQuery();
                }
            }

        }

        public void createDB()
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = @"CREATE DATABASE Explorer";
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void createTable()
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                using (var cmd = new NpgsqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = @"CREATE TABLE Files (
                            id SERIAL PRIMARY KEY,
                            filename varchar(1000),
                            filevalue varchar(10000000),
                            path varchar(1000)
                            )";
                    cmd.ExecuteNonQuery();


                }
            }
        }

        #region checkTableExists
        public bool checkTableExists()
        {
            string sql = "SELECT * FROM information_schema.tables WHERE table_name = 'files'";
            using (var con = new NpgsqlConnection(connString))
            {
                using (var cmd = new NpgsqlCommand(sql))
                {
                    try
                    {
                        if (cmd.Connection == null)
                            cmd.Connection = con;
                        if (cmd.Connection.State != System.Data.ConnectionState.Open)
                            cmd.Connection.Open();

                        lock (cmd)
                        {
                            using (NpgsqlDataReader rdr = cmd.ExecuteReader())
                            {
                                try
                                {
                                    if (rdr != null && rdr.HasRows)
                                        return true;
                                    return false;
                                }
                                catch (Exception)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }

                }
            }
        }

        #endregion

        public bool getFiles()
        {
            using (var conn = new NpgsqlConnection(connString))
            {

                Console.Out.WriteLine("Opening connection");
                conn.Open();


                using (var command = new NpgsqlCommand("SELECT * FROM files", conn))
                {

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        EFile File = new EFile(
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3));

                        ScanFiles.Add(File);
                    }
                    reader.Close();
                }
            }

            return true;
        }

        public bool uploadFiles(List<EFile> Files, string BasePath)
        {
            using (var conn = new NpgsqlConnection(connString))
            {

                Console.Out.WriteLine("Opening connection");
                conn.Open();


                foreach (var element in Files)
                {
                    using (var command = new NpgsqlCommand("INSERT INTO files ( filename, filevalue, path) VALUES ( @q2, @q3, @q4)", conn))
                    {

                        command.Parameters.AddWithValue("q2", element.Filename);
                        command.Parameters.AddWithValue("q3", element.Filevalue);
                        command.Parameters.AddWithValue("q4", element.Path);

                        command.ExecuteNonQuery();
                    }
                }
            }
            Console.WriteLine("Files are uploaded");
            MessageBox.Show("Files are uploaded!");
            return true;
        }



        public List<EFile> searchFiles(string searchvalue)
        {
            using (var conn = new NpgsqlConnection(connString))
            {

                Console.Out.WriteLine("Opening connection");
                conn.Open();
                string sql = "SELECT filename, filevalue, path FROM files WHERE (filevalue LIKE '%" + searchvalue + "%') OR (filename LIKE '%" + searchvalue + "%')";

                using (var command = new NpgsqlCommand(sql, conn))
                {

                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        EFile File = new EFile(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetString(2));

                        SearchFiles.Add(File);
                    }
                    reader.Close();
                }
            }


            return SearchFiles;
        }
    }
}
