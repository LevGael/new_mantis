# ************************************************************
# Sequel Ace SQL dump
# Version 20033
#
# https://sequel-ace.com/
# https://github.com/Sequel-Ace/Sequel-Ace
#
# Host: 127.0.0.1 (MySQL 8.0.29)
# Database: mantis-db
# Generation Time: 2022-07-25 09:53:37 +0000
# ************************************************************


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
SET NAMES utf8mb4;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE='NO_AUTO_VALUE_ON_ZERO', SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;


# Dump of table Categorie
# ------------------------------------------------------------

DROP TABLE IF EXISTS `Categorie`;

CREATE TABLE `Categorie` (
  `idCategorie` int NOT NULL AUTO_INCREMENT,
  `libelle` varchar(50) NOT NULL,
  PRIMARY KEY (`idCategorie`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

LOCK TABLES `Categorie` WRITE;
/*!40000 ALTER TABLE `Categorie` DISABLE KEYS */;

INSERT INTO `Categorie` (`idCategorie`, `libelle`)
VALUES
	(1,'Evolution'),
	(2,'Bug'),
	(3,'Plantage'),
	(4,'TMA');

/*!40000 ALTER TABLE `Categorie` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table Utilisateur
# ------------------------------------------------------------

DROP TABLE IF EXISTS `Utilisateur`;

CREATE TABLE `Utilisateur` (
  `email` varchar(100) NOT NULL,
  `mdp` varchar(500) NOT NULL,
  `nom` varchar(50) NOT NULL,
  `prenom` varchar(50) NOT NULL,
  `firstconnexion` int NOT NULL,
  `tempsJour` varchar(3) NOT NULL,
  `tempsMineur` int NOT NULL,
  `tempsMajeur` int NOT NULL,
  `tempsCritique` int NOT NULL,
  PRIMARY KEY (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

LOCK TABLES `Utilisateur` WRITE;
/*!40000 ALTER TABLE `Utilisateur` DISABLE KEYS */;

INSERT INTO `Utilisateur` (`email`, `mdp`, `nom`, `prenom`, `firstconnexion`, `tempsJour`, `tempsMineur`, `tempsMajeur`, `tempsCritique`)
VALUES
	('gael.levanier@celios.fr','$2a$11$jbuTdZ2DvEQESQmMWbVSwO6ONryH0SPIS7Itj41tWCcSx5KTKtvK6','Levanier','Gael',0,'8',13,25,49),
	('louis.soleilhavoup@celios.fr','$2a$11$jbuTdZ2DvEQESQmMWbVSwO6ONryH0SPIS7Itj41tWCcSx5KTKtvK6','Soleilhavoup','Louis',0,'7.2',1000,16,26);

/*!40000 ALTER TABLE `Utilisateur` ENABLE KEYS */;
UNLOCK TABLES;


# Dump of table UtilisateurCategorie
# ------------------------------------------------------------

DROP TABLE IF EXISTS `UtilisateurCategorie`;

CREATE TABLE `UtilisateurCategorie` (
  `idCategorie` int NOT NULL,
  `email` varchar(100) NOT NULL,
  PRIMARY KEY (`idCategorie`,`email`),
  KEY `UtilisateurCategorie_Utilisateur0_FK` (`email`),
  CONSTRAINT `UtilisateurCategorie_Categorie_FK` FOREIGN KEY (`idCategorie`) REFERENCES `Categorie` (`idCategorie`),
  CONSTRAINT `UtilisateurCategorie_Utilisateur0_FK` FOREIGN KEY (`email`) REFERENCES `Utilisateur` (`email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

LOCK TABLES `UtilisateurCategorie` WRITE;
/*!40000 ALTER TABLE `UtilisateurCategorie` DISABLE KEYS */;

INSERT INTO `UtilisateurCategorie` (`idCategorie`, `email`)
VALUES
	(3,'gael.levanier@celios.fr'),
	(4,'gael.levanier@celios.fr'),
	(1,'louis.soleilhavoup@celios.fr'),
	(3,'louis.soleilhavoup@celios.fr');

/*!40000 ALTER TABLE `UtilisateurCategorie` ENABLE KEYS */;
UNLOCK TABLES;



/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
