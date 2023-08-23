using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesDataAccessLayer
{
   public class Game
   {
      public int Id { get; set; }
      public string Titre { get; set; }
      public string Synopsis { get; set; }
      public DateTime AnneeSortie { get; set; }

      public List<Categorie> Categories { get; set; }

      public Game() 
      {
         Categories = new List<Categorie>();
      }
   }
}
