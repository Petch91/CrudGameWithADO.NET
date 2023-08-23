CREATE TABLE [dbo].[JeuCat]
(
	[IdGame] INT NOT NULL , 
    [IdCat] INT NOT NULL ,
    PRIMARY KEY (IdGame,IdCat),
    CONSTRAINT [FK_JeuCat_Jeu] FOREIGN KEY ([IdGame]) REFERENCES [Jeu]([IdGame]), 
    CONSTRAINT [FK_JeuCat_Categorie] FOREIGN KEY ([IdCat]) REFERENCES [Categorie]([IdCat])
)
