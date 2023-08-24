using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using GamesDataAccessLayer.Class;
using GamesDataAccessLayer.Interfaces;

namespace GamesDataAccessLayer.Services
{
   public class GameService : Service, IService<Game>
   {
      public  void Create(Game game)
      {

         string sql = "INSERT INTO Jeu (Titre,AnneeSortie,Synopsis) VALUES (@titre,@date,@desc) SELECT TOP 1 IdGame FROM Jeu ORDER BY IdGame DESC";
         SqlParameter[] parms = { new SqlParameter("titre", game.Titre),
                                     new SqlParameter("date", game.AnneeSortie),
                                     new SqlParameter("desc", game.Synopsis)
                                   };
         game.Id = (int)ExecuteScalar(sql, parms);

         foreach (Categorie c in game.Categories)
         {
            int IdCat;
            string sql2 = "SELECT IdCat FROM Categorie WHERE Nom = @nom";
            SqlParameter[] parms2 = { new SqlParameter("nom", c.Name) };
            IdCat = (int)ExecuteScalar(sql2, parms2);

            string sql3 = "INSERT INTO JeuCat (IdGame, IdCat) VALUES (@IdGame,@IdCat)";
            SqlParameter[] parms3 = { new SqlParameter("IdGame", game.Id),
                                     new SqlParameter("IdCat", IdCat) };

            ExecuteNonQuery(sql3, parms3);
         }
      }


      public List<Game> GetAll()
      {

         string sql = "SELECT IdGame, Titre, AnneeSortie FROM  Jeu";

         return ExecuteReader<Game>(sql,
                                    reader => new Game
                                    {
                                       Id = (int)reader["IdGame"],
                                       Titre = reader["Titre"].ToString(),
                                       AnneeSortie = (DateTime)reader["AnneeSortie"]
                                    });
      }

      public List<Game> GetGamesByCat(Categorie c)
      {

         string sql = "SELECT Titre FROM  JeuxCategories WHERE IdCat = @id";
         SqlParameter[] sp = {new SqlParameter("id", c.Id)};

         return ExecuteReader<Game>(sql, sp, reader => new Game { Titre = reader["Titre"].ToString() });
      }


      public Game GetGameById(int id)
      {
         Game g = new Game();
         using (_cnx = CreateConnection())
         {

            using (_cmd = _cnx.CreateCommand())
            {
               string sql = "SELECT * FROM  JeuxCategories WHERE IdGame = @id";

               _cmd.CommandText = sql;
               _cmd.Parameters.AddWithValue("id", id);
               _cnx.Open();
               using (SqlDataReader reader = _cmd.ExecuteReader())
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
                  _cnx.Close();
               }
            }
            return g;
         }
      }

   }
}

