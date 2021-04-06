using Dapper;
using Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;

namespace ApplicationServer.DB {
    public class AvatarsDB : IAvatarsDB {

        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<Avatar> GetAllAvatars() {
            string query = "SELECT * FROM Avatar";

            try {
                using(SqlConnection con = new SqlConnection(connectionString)) {

                    return con.Query<Avatar>(query);
                    
                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
                throw;
            }
        }

        internal Avatar GetAvatarById(int id) {
            string query = "SELECT * FROM Avatar WHERE Id = @Id";

            try {
                using(SqlConnection con = new SqlConnection(connectionString)) {

                    return con.QuerySingle<Avatar>(query, new { Id = id });

                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
                throw;
            }
        }
    }
}