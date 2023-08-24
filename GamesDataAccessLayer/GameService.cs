using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
      public List<Game> GetGames()
      {
         List<Game> list = new List<Game>();
         using (SqlConnection cnx = new SqlConnection(_connectionString))
         {

            using (SqlCommand cmd = cnx.CreateCommand())
            {
               string sql = "SELECT IdGame, Titre, AnneeSortie FROM  Jeu";

               cmd.CommandText = sql;
               cnx.Open();
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {

                     list.Add(new Game
                     {
                        Id = (int)reader["IdGame"],
                        Titre = (string)reader["Titre"],
                        AnneeSortie = (DateTime)reader["AnneeSortie"]
                     });
                  }
                  cnx.Close();
               }
            }
            return list;
         }
      }
      public List<Game> GetGamesByCat(Categorie c)
      {
         List<Game> list = new List<Game>();
         using (SqlConnection cnx = new SqlConnection(_connectionString))
         {

            using (SqlCommand cmd = cnx.CreateCommand())
            {
               string sql = "SELECT Titre FROM  JeuxCategories WHERE IdCat = @id";

               cmd.CommandText = sql;
               cmd.Parameters.AddWithValue("id", c.Id);
               cnx.Open();
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                        list.Add(new Game
                        {
                           Titre = (string)reader["Titre"]

                        });
                  }
                  cnx.Close();
               }
            }
            return list;
         }
      }
      public Game GetGameById(int id)
      {
         Game g = new Game();
         using (SqlConnection cnx = new SqlConnection(_connectionString))
         {

            using (SqlCommand cmd = cnx.CreateCommand())
            {
               string sql = "SELECT * FROM  JeuxCategories WHERE IdGame = @id";

               cmd.CommandText = sql;
               cmd.Parameters.AddWithValue("id",id);
               cnx.Open();
               using (SqlDataReader reader = cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     if (g.Titre != reader["Titre"].ToString())
                     {
                        
                        g.Titre = reader["Titre"].ToString();
                        g.Synopsis = reader["Synopsis"].ToString();
                        g.AnneeSortie = (DateTime)reader["AnneeSortie"];

                     }
                     g.Categories.Add(new Categorie { Name = reader["Nom"].ToString(), Id = (int)reader["IdCat"] });
                  }
                  cnx.Close();
               }
            }
            return g;
         }
      }
      //public IEnumerable<object> GetGamesByCat()
      //{
      //   CategorieService categorieService = new CategorieService();
      //   var query = GetGames().SelectMany(g => g.Categories, (g, c) => new
      //                           {
      //                              IdGame = g.Id,
      //                              g.Titre,
      //                              g.AnneeSortie,
      //                              g.Synopsis,
      //                              IdCat = c.Id,
      //                              c.Name
      //                           })
      //                           .Distinct();
      //  var result = categorieService.GetCategorie().GroupJoin(query, c => c.Id, g => g.IdCat, (c, g) => new { c.Name, game = g })
      //                                                .Where(g => g.game.Count() > 0);
      //   return result;
      //}
   }
}
