-- NewsAPI (BTS SIO SLAM) : base de démonstration
-- Faite à partir de la vraie base locale `newsdb` (mysqldump), avec des comptes de démo.
-- Import : mysql -u root -p < database/newsapi.sql   (ou phpMyAdmin > Importer)
-- Comptes de démo : admin / demo1234 et demo / demo1234 (mots de passe hachés avec BCrypt)

SET NAMES utf8mb4;

DROP DATABASE IF EXISTS `newsapi`;
CREATE DATABASE `newsapi` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `newsapi`;

-- Comptes utilisateurs
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `Username` varchar(50) NOT NULL,
  `PasswordHash` varchar(255) NOT NULL,
  `Email` varchar(50) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`Username`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Articles mis en favoris par chaque utilisateur
CREATE TABLE `favorites` (
  `id` int NOT NULL AUTO_INCREMENT,
  `UserId` int NOT NULL,
  `Title` varchar(255) NOT NULL,
  `url` text NOT NULL,
  `urlToImage` text,
  `publishedAt` datetime DEFAULT NULL,
  `Description` text,
  `Source` varchar(50) DEFAULT NULL,
  `Category` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `user_id` (`UserId`),
  CONSTRAINT `favorites_ibfk_1` FOREIGN KEY (`UserId`) REFERENCES `users` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- Historique des ajouts / suppressions de favoris (rempli par les triggers ci-dessous)
CREATE TABLE `historique` (
  `id` int NOT NULL AUTO_INCREMENT,
  `action` enum('ajout','modif','supp','') NOT NULL,
  `dateAction` date NOT NULL,
  `idUser` int NOT NULL,
  PRIMARY KEY (`id`),
  KEY `idUser` (`idUser`),
  CONSTRAINT `historique_ibfk_1` FOREIGN KEY (`idUser`) REFERENCES `users` (`id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

DELIMITER ;;
CREATE TRIGGER `historiqueAjout` AFTER INSERT ON `favorites` FOR EACH ROW BEGIN

DECLARE dernierUser INT;

SELECT UserId INTO dernierUser
FROM favorites ORDER BY id DESC LIMIT 1;

INSERT INTO historique (action, dateAction, idUser) VALUES ('ajout', CURRENT_DATE, dernierUser);

END;;

CREATE TRIGGER `historiqueSupp` BEFORE DELETE ON `favorites` FOR EACH ROW BEGIN

DECLARE dernierUser INT;

SELECT UserId INTO dernierUser
FROM favorites ORDER BY id DESC LIMIT 1;

INSERT INTO historique (action, dateAction, idUser) VALUES ('supp', CURRENT_DATE, dernierUser);

END;;
DELIMITER ;

-- Données de démonstration (mot de passe : demo1234)
INSERT INTO `users` (`id`, `Username`, `PasswordHash`, `Email`) VALUES
(1, 'admin', '$2a$11$75.DJs4JW/AwxRLExW8b8ul5iVRHMRvnE2w6rdxlw3u9Vb9BiQzm2', 'admin@example.com'),
(2, 'demo', '$2a$11$V8EOjNQ6XMnwjqsqPlfka.Otk9K7IRN3yArZKmXm3lyjf6Y3jmbry', 'demo@example.com');

-- Articles réellement enregistrés pendant le projet (Le Monde, via NewsAPI)
INSERT INTO `favorites` (`UserId`, `Title`, `url`, `urlToImage`, `publishedAt`, `Description`, `Source`, `Category`) VALUES
(2, 'Aux Césars, le succès d’« Emila Pérez » et de « L’Histoire de Souleyman »', 'https://www.lemonde.fr/cinema/article/2025/03/01/aux-cesars-le-succes-d-emila-perez-et-de-l-histoire-de-souleyman_6570883_3476.html', 'https://img.lemde.fr/2025/03/01/0/0/3339/2226/1440/960/60/0/f04dcfb_ftp-import-images-1-bixo17cdjnmf-f378ced270634f5a8af4f31c37bc641c-1-1731c26f758e46fc86a65b547c9a2755.jpg', '2025-03-01 04:00:13', 'De bonne tenue et bien menée, la cérémonie de récompense du cinéma français a été marquée, vendredi 28 février, par des prises de parole à teneur politique.', 'Le Monde', NULL),
(2, 'Le grand déphasage de la politique française face aux bouleversements du monde', 'https://www.lemonde.fr/politique/article/2025/02/21/le-grand-dephasage-de-la-politique-francaise-face-aux-bouleversements-du-monde_6557530_823448.html', 'https://img.lemde.fr/2025/02/21/333/0/5000/3333/1440/960/60/0/734f727_sirius-fs-upload-1-bnix2aylrcfz-1740120595174-kzihnioglu-paris-pol-elysee-20250217-27.JPG', '2025-02-21 11:16:20', 'Face à la menace Trump et à l’offensive idéologique américaine, la plupart des politiques français semblent désemparés, en deçà des enjeux, au risque de rester à la remorque de l’histoire.', 'Le Monde', NULL),
(1, 'Aux Césars, le succès d’« Emila Pérez » et de « L’Histoire de Souleyman »', 'https://www.lemonde.fr/cinema/article/2025/03/01/aux-cesars-le-succes-d-emila-perez-et-de-l-histoire-de-souleyman_6570883_3476.html', 'https://img.lemde.fr/2025/03/01/0/0/3339/2226/1440/960/60/0/f04dcfb_ftp-import-images-1-bixo17cdjnmf-f378ced270634f5a8af4f31c37bc641c-1-1731c26f758e46fc86a65b547c9a2755.jpg', '2025-03-01 04:00:13', 'De bonne tenue et bien menée, la cérémonie de récompense du cinéma français a été marquée, vendredi 28 février, par des prises de parole à teneur politique.', 'Le Monde', NULL);
