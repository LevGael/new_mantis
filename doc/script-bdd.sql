#------------------------------------------------------------
#        Script MySQL.
#------------------------------------------------------------


#------------------------------------------------------------
# Table: Utilisateur
#------------------------------------------------------------

CREATE TABLE Utilisateur(
        email          Varchar (100) NOT NULL ,
        mdp            Varchar (500) NOT NULL ,
        nom            Varchar (50) NOT NULL ,
        prenom         Varchar (50) NOT NULL ,
        firstconnexion Int NOT NULL ,
        tempsJour      Varchar (3) NOT NULL ,
        tempsMineur    Int NOT NULL ,
        tempsMajeur    Int NOT NULL ,
        tempsCritique  Int NOT NULL
	,CONSTRAINT Utilisateur_PK PRIMARY KEY (email)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: Categorie
#------------------------------------------------------------

CREATE TABLE Categorie(
        idCategorie Int  Auto_increment  NOT NULL ,
        libelle     Varchar (50) NOT NULL
	,CONSTRAINT Categorie_PK PRIMARY KEY (idCategorie)
)ENGINE=InnoDB;


#------------------------------------------------------------
# Table: UtilisateurCategorie
#------------------------------------------------------------

CREATE TABLE UtilisateurCategorie(
        idCategorie Int NOT NULL ,
        email       Varchar (100) NOT NULL
	,CONSTRAINT UtilisateurCategorie_PK PRIMARY KEY (idCategorie,email)

	,CONSTRAINT UtilisateurCategorie_Categorie_FK FOREIGN KEY (idCategorie) REFERENCES Categorie(idCategorie)
	,CONSTRAINT UtilisateurCategorie_Utilisateur0_FK FOREIGN KEY (email) REFERENCES Utilisateur(email)
)ENGINE=InnoDB;

