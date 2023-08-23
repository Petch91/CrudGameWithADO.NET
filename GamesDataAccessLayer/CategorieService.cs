using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesDataAccessLayer
{
   public class CategorieService : Service
   {
      public void CreateCat(Categorie categorie)
      {
         using (SqlConnection cnx = new SqlConnection(_connectionString))
         {
            using (SqlCommand cmd = cnx.CreateCommand())
            {
               string sql = "INSERT INTO Categorie (Nom) VALUES (@nom)";

               cmd.CommandText = sql;
               cmd.Parameters.AddWithValue("nom", categorie.Name);
               cnx.Open();
               cmd.ExecuteNonQuery();
               cnx.Close();
            }
         }
      }

      public List<Categorie> GetCategorie() 
      {
         List<Categorie> list = new List<Categorie>();
         using (SqlConnection cnx = new SqlConnection(_connectionString))
         {             
            using (SqlCommand cmd = cnx.CreateCommand())
            {
               string sql = "SELECT * FROM Categorie";

               cmd.CommandText = sql;
               cnx.Open();
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while(reader.Read()) 
                  {
                     list.Add(new Categorie
                     {
                        Id = (int)reader["IdCat"],
                        Name = (string)reader["Nom"],
                     }) ;
                  }
               }
               cnx.Close();
            }
         }
         return list;
      }
      public void Update(int id,string name)
      {
         using (SqlConnection cnx = new SqlConnection(_connectionString))
         {
            using (SqlCommand cmd = cnx.CreateCommand())
            {
               string sql = "Update Categorie " +
                            "SET Nom = @nom " +
                            "WHERE IdCat = @id";

               cmd.CommandText = sql;
               cmd.Parameters.AddWithValue("nom", name);
               cmd.Parameters.AddWithValue("id", id);
               cnx.Open();
               cmd.ExecuteNonQuery();
               cnx.Close();
            }
         }
      }
   }
}
