using GamesDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamesView
{
   public class Views
   {
      public void ViewMenu()
      {
         int choix = 0;

         while (choix < 7)
         {
            Console.Clear();
            Console.WriteLine("Que voulez vous faire ? ");
            Console.WriteLine("1 - Ajouter un jeu");
            Console.WriteLine("2 - Afficher les Jeux");
            Console.WriteLine("3 - Afficher les jeux par catégorie");
            Console.WriteLine("4 - Ajouter  une catégorie");
            Console.WriteLine("5 - Afficher les catégories");
            Console.WriteLine("6 - Modifier  une catégorie");
            Console.WriteLine("7 - Quitter");
            choix = int.Parse(Console.ReadLine());
            Console.Clear();
            switch (choix)
            {
               case 1:
                  AddGame();
                  break;
               case 2:
                  ViewGame();
                  Console.ReadKey();
                  break;
               case 3:
                  ViewGamesByCategorie();
                  Console.ReadKey();
                  break;
               case 4:
                  AddCat();
                  break;

               case 5:
                  ViewsCategories();
                  Console.ReadKey();
                  break;
               case 6:
                  ModifyCategorie();
                  break;

               default:
                  break;
            }

         }

      }

      private void ViewGamesByCategorie()
      {
         GameService gameService = new GameService();
         CategorieService categorieService = new CategorieService();
         var query = gameService.GetGames().SelectMany(g => g.Categories, (g, c) => new
         {
            IdGame = g.Id,
            g.Titre,
            g.AnneeSortie,
            g.Synopsis,
            IdCat = c.Id,
            c.Name
         })
                                 .Distinct();
         var result = categorieService.GetCategorie().GroupJoin(query, c => c.Id, g => g.IdCat, (c, g) => new { c.Name, game = g })
                                                       .Where(g => g.game.Count() > 0);
         foreach (var c in result)
         {
            Console.WriteLine(c.Name);
            foreach (var g in c.game)
            {
               Console.WriteLine("\t" + g.Titre);
            }
         }
      }


      private void ViewGame()
      {
         Console.Clear();
         GameService service = new GameService();
         Console.WriteLine("Liste des jeux:");
         foreach (Game g in service.GetGames())
         {
            Console.Write($"Titre : {g.Titre} - Année de sortie :  {g.AnneeSortie} - Synopsis : {g.Synopsis} {(g.Categories.Count > 0 ? g.Categories[0].Name : "rien")}");
            Console.WriteLine();
         }
      }

      private void AddGame()
      {
         Console.Clear();
         GameService service = new GameService();
         Game game = new Game();
         Console.Write("Entrez le titre du jeu : ");
         game.Titre = Console.ReadLine();
         game.AnneeSortie = DateTime.Now;
         Console.Write("Entrez un synopsis : ");
         game.Synopsis = Console.ReadLine();
         int i = 1;
         do
         {
            Categorie categorie = new Categorie();
            Console.Write($"Entrez la categorie({i}) (Appuyer sur ENTER sans rien rentrer pour ajouter le jeu) : ");
            i++;
            string result = Console.ReadLine() ?? string.Empty;
            if (result == string.Empty) break; // je sais c'est  pas une bonne pratique
            categorie.Name = result;
            game.Categories.Add(categorie);
         } while (true);
         service.CreateGame(game);
      }

      private void ModifyCategorie()
      {
         Console.Clear();
         CategorieService service = new CategorieService();
         ViewsCategories();
         Console.Write("Quel est l'ID de la categorie a modifié ? : ");
         int id = int.Parse(Console.ReadLine());
         Console.Write("Quel est le nouveau nom? : ");
         string name = Console.ReadLine();
         service.Update(id, name);
      }

      private void ViewsCategories()
      {
         Console.Clear();
         CategorieService service = new CategorieService();
         Console.WriteLine("Liste des catégories:");
         foreach (Categorie c in service.GetCategorie())
         {
            Console.WriteLine($"ID : {c.Id} - Nom : {c.Name}");
         }
      }

      private void AddCat()
      {
         Console.Clear();
         CategorieService service = new CategorieService();
         Categorie categorie = new Categorie();
         Console.Write("Entrez le nom de la nouvelle catégorie : ");
         categorie.Name = Console.ReadLine().ToLower();
         service.CreateCat(categorie);

      }
   }
}
