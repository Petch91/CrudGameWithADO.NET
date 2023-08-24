CREATE VIEW [dbo].[JeuxCategories]
	AS select j.IdGame, j.Titre, j.AnneeSortie, j.Synopsis,c.IdCat,c.Nom
		FROM Jeu j join JeuCat jc
		ON j.IdGame = jc.IdGame
		join Categorie c
		ON c.IdCat = jc.IdCat
