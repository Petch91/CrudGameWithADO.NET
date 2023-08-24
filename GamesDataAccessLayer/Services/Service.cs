using GamesDataAccessLayer.Class;
using GamesDataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesDataAccessLayer.Services
{
   public abstract class Service
   {
      private string _connectionString = @"Data Source=DESKTOP-9B27V2B;Initial Catalog=GameDB;Integrated Security=True;";
      protected SqlConnection _cnx { get; set; }

      protected SqlConnection CreateConnection()
      {
         return new SqlConnection(_connectionString);
      }

      protected SqlCommand _cmd;
      protected void ExecuteNonQuery(string sqlRequest)
      {
         using (_cnx = CreateConnection())
         {
            using (_cmd = _cnx.CreateCommand())
            {
               _cmd.CommandText = sqlRequest;
               _cnx.Open();
               _cmd.ExecuteNonQuery();
               _cnx.Close();
            }
         }
      }

      protected void ExecuteNonQuery(string sqlRequest, SqlParameter[] parameters)
      {
         using(_cnx = CreateConnection()) 
         {
            using (_cmd = _cnx.CreateCommand()) 
            {
               _cmd.CommandText = sqlRequest;
               _cmd.Parameters.AddRange(parameters);
               _cnx.Open();
               _cmd.ExecuteNonQuery();
               _cnx.Close();
            }
         }
      }
      protected object ExecuteScalar(string sqlRequest)
      {
         object result;
         using (_cnx = CreateConnection())
         {
            using (_cmd = _cnx.CreateCommand())
            {
               _cmd.CommandText = sqlRequest;
               _cnx.Open();
               result = _cmd.ExecuteScalar();
               _cnx.Close();
            }
         }
         return result;
      }
      protected object ExecuteScalar(string sqlRequest, SqlParameter[] parameters)
      {
         object result; 
         using (_cnx = CreateConnection())
         {
            using (_cmd = _cnx.CreateCommand())
            {
               _cmd.CommandText = sqlRequest;
               _cmd.Parameters.AddRange(parameters);
               _cnx.Open();
               result = _cmd.ExecuteScalar();
               _cnx.Close();
            }
         }
         return result;
      }
      protected List<T> ExecuteReader<T>(string sqlRequest, Func<SqlDataReader, T> mapper)
      {
         List<T> list = new List<T>();
         using (_cnx = CreateConnection())
         {
            using (_cmd = _cnx.CreateCommand())
            {
               _cmd.CommandText = sqlRequest;
               _cnx.Open();
               using (SqlDataReader reader = _cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     list.Add(mapper(reader));
                  }
               }
               _cnx.Close();
            }
         }
         return list;
      }
      protected List<T> ExecuteReader<T>(string sqlRequest, SqlParameter[] parameters, Func<SqlDataReader,T> mapper)
      {
         List<T> list = new List<T>();
         using (_cnx = CreateConnection())
         {
            using (_cmd = _cnx.CreateCommand())
            {
               _cmd.CommandText = sqlRequest;
               _cmd.Parameters.AddRange(parameters);
               _cnx.Open();
               using (SqlDataReader reader = _cmd.ExecuteReader())
               {
                  while (reader.Read())
                  {
                     list.Add(mapper(reader));
                  }
               }
               _cnx.Close();
            }
         }
         return list;
      }

  
      
   }
}
