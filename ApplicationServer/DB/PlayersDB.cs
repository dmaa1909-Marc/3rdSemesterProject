using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ApplicationServer.DB;
using Dapper;
using Model;

namespace ApplicationServer.DB {

    public class PlayersDB {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<Player> getAllPlayers() {
            string query = "SELECT * FROM Player";

            using (SqlConnection con = new SqlConnection(connectionString)) {
                return con.Query<Player>(query);
            }
        }
    }
}