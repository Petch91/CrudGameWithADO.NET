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
            Console.WriteLine("4 - Afficher les détails d'un jeu");
            Console.WriteLine("5 - Ajouter  une catégorie");
            Console.WriteLine("6 - Afficher les catégories");
            Console.WriteLine("7 - Modifier  une catégorie");
            Console.WriteLine("8 - Quitter");
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
                  ViewDetailledGame();
                  Console.ReadKey();
                  break;
               case 5:
                  AddCat();
                  break;

               case 6:
                  ViewsCategories();
                  Console.ReadKey();
                  break;
               case 7:
                  ModifyCategorie();
                  break;

               default:
                  break;
            }

         }

      }

      private void ViewDetailledGame()
      {
         GameService gameService = new GameService();
         ViewGame();
         Console.Write("Entrez l'ID du jeu que vous voulez afficher : ");
         int ID = int.Parse(Console.ReadLine());
         Game game = gameService.GetGameById(ID);
         Console.Write($"\nTitre : {game.Titre}\nSynopsis : {game.Synopsis} " +
                       $"\nAnnée de sortie : {game.AnneeSortie:dd/MM/yyyy}\nCatégorie(s) :");
         foreach (Categorie c in game.Categories)
         {
            Console.Write($" {c.Id}: {c.Name}");
         }
      }

      private void ViewGamesByCategorie()
      {
         GameService gameService = new GameService();
         CategorieService categorieService = new CategorieService();
         foreach (Categorie c in categorieService.GetCategorie())
         {
            var games = gameService.GetGamesByCat(c).Select(g => new { g.Titre });
            if (games.Count() > 0)
            {
               Console.WriteLine($"{c.Name} : ");
               foreach (var g in games)
               {
                  Console.WriteLine($"\t{g.Titre}");
               }
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
            Console.Write($"ID : {g.Id} - Titre : {g.Titre} - Année de sortie :  {g.AnneeSortie:dd/MM/yyyy}");
            Console.WriteLine();
         }
      }

      private void AddGame()
      {
         Console.Clear();
         GameService service = new GameService();
         CategorieService categorieService = new CategorieService();
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
            string result = Console.ReadLine().ToLower() ?? string.Empty;
            if (result == string.Empty) break; // je sais c'est  pas une bonne pratique
            if (categorieService.GetCategorie().Where(c => c.Name == result).Count() > 0)
            {
               categorie.Name = result;
               game.Categories.Add(categorie);
               i++;
            }

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
