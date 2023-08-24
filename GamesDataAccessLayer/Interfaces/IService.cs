using GamesDataAccessLayer.Class;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesDataAccessLayer.Interfaces
{
   public interface IService<T>
   {

      public void Create(T obj);
      public List<T> GetAll();

   }
}
