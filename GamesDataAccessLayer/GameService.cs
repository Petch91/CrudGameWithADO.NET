using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesDataAccessLayer
{
   public class GameService : Service
   {
      public void CreateGame(Game game)
      {
         using (SqlConnection cnx = new SqlConnection(_connectionString))
         {
            
            using (SqlCommand cmd = cnx.CreateCommand())
            {
               string sql = "INSERT INTO Jeu (Titre,AnneeSortie,Synopsis) VALUES (@titre,@date,@desc) SELECT TOP 1 IdGame FROM Jeu ORDER BY IdGame DESC";

               cmd.CommandText = sql;
               cmd.Parameters.AddWithValue("titre", game.Titre);
               cmd.Parameters.AddWithValue("date", game.AnneeSortie);
               cmd.Parameters.AddWithValue("desc", game.Synopsis);
               cnx.Open();
               game.Id = (int)cmd.ExecuteScalar();
               cnx.Close();

            }
            foreach (Categorie c in game.Categories)
            {
               int IdCat;
               using (SqlCommand cmd = cnx.CreateCommand())
               {
                  string sql = "SELECT IdCat FROM Categorie WHERE Nom = @nom";

                  cmd.CommandText = sql;
                  cmd.Parameters.AddWithValue("nom", c.Name);
                  cnx.Open();
                  IdCat = (int)cmd.ExecuteScalar();
                  cnx.Close();

               }
               using (SqlCommand cmd = cnx.CreateCommand())
               {
                  string sql = "INSERT INTO JeuCat (IdGame, IdCat) VALUES (@IdGame,@IdCat)";

                  cmd.CommandText = sql;
                  cmd.Parameters.AddWithValue("IdCat", IdCat);
                  cmd.Parameters.AddWithValue("IdGame", game.Id);
                  cnx.Open();
                  cmd.ExecuteNonQuery();
                  cnx.Close();
               }
            }
         }
      }
   }
}
