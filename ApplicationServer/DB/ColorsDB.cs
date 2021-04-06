using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Web;

namespace ApplicationServer.DB {
    public class ColorsDB : IColorsDB {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        public IEnumerable<Color> GetColors() {
            string query = "SELECT Color FROM Color";

            try {
                using(SqlConnection con = new SqlConnection(connectionString)) {

                    IEnumerable<string> colorNames = con.Query<string>(query);

                    List<Color> colors = new List<Color>();

                    foreach(string namedColor in colorNames) {
                        colors.Add(Color.FromName(namedColor));
                        Debug.WriteLine(Color.FromName(namedColor).Name);
                    }
                    return colors;
                }
            }
            catch(Exception e) {
                Debug.WriteLine(e);
                throw;
            }
        }

        public void AddColor(Color color) {
            string query = "INSERT INTO Color VALUES (@Name)";

            if(color.IsNamedColor) {
                using(SqlConnection con = new SqlConnection(connectionString)) {
                    con.ExecuteScalar(query, color);// new { Color = color.Name });
                }
            }
            else {
                throw new ArgumentException("Color must be a named color");
            }
        }
    }
}