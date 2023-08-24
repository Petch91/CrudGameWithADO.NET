using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamesDataAccessLayer.Class;
using GamesDataAccessLayer.Interfaces;

namespace GamesDataAccessLayer.Services
{
   public class CategorieService : Service , IService<Categorie>
   {
      public void Create(Categorie categorie)
      {
         string sql = "INSERT INTO Categorie (Nom) VALUES (@nom)";
         SqlParameter[] sp = { new SqlParameter("nom", categorie.Name) };
         ExecuteNonQuery(sql, sp);
      }

      public List<Categorie> GetAll()
      {

         string sql = "SELECT * FROM Categorie";
         return ExecuteReader<Categorie>(sql, reader => 
                                               new Categorie {Id = (int)reader["IdCat"], Name = reader["Nom"].ToString()});
      }
      public void Update(int id, string name)
      {
         string sql = "Update Categorie " +
                      "SET Nom = @nom " +
                      "WHERE IdCat = @id";

         _cmd.Parameters.AddWithValue("nom", name);
         _cmd.Parameters.AddWithValue("id", id);
         SqlParameter[] parms = { new SqlParameter("nom", name),
                                  new SqlParameter("id", id) };
         ExecuteNonQuery(sql, parms);

      }
      public Categorie GetByID(int id) 
      {
         string sql = "SELECT * FROM Categorie WHERE IdCat = @id";
         SqlParameter[] parms = { new SqlParameter("id", id) };
         List<Categorie> list = ExecuteReader<Categorie>(sql, parms, reader => new Categorie { Id = (int)reader["IdCat"], Name = reader["Nom"].ToString() });

         return list.Count()>0 ? list[0] : new Categorie();
      }
   }
}
