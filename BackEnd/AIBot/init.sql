-- MySQL dump 10.13  Distrib 8.0.21, for Win64 (x86_64)
--
-- Host: localhost    Database: aiape
-- ------------------------------------------------------
-- Server version	8.0.21

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__efmigrationshistory`
--

LOCK TABLES `__efmigrationshistory` WRITE;
/*!40000 ALTER TABLE `__efmigrationshistory` DISABLE KEYS */;
INSERT INTO `__efmigrationshistory` VALUES ('20210615091331_init','5.0.3');
/*!40000 ALTER TABLE `__efmigrationshistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `answers`
--

DROP TABLE IF EXISTS `answers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `answers` (
  `AnswerId` int NOT NULL AUTO_INCREMENT,
  `UserId` int DEFAULT NULL,
  `QuestionId` int NOT NULL,
  `Content` longtext NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifyTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`AnswerId`),
  KEY `IX_Answers_QuestionId` (`QuestionId`),
  KEY `IX_Answers_UserId` (`UserId`),
  CONSTRAINT `FK_Answers_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`QuestionId`) ON DELETE CASCADE,
  CONSTRAINT `FK_Answers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `answers`
--

LOCK TABLES `answers` WRITE;
/*!40000 ALTER TABLE `answers` DISABLE KEYS */;
/*!40000 ALTER TABLE `answers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `likeanswers`
--

DROP TABLE IF EXISTS `likeanswers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `likeanswers` (
  `UserId` int NOT NULL,
  `AnswerId` int NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`AnswerId`,`UserId`),
  KEY `IX_LikeAnswers_UserId` (`UserId`),
  CONSTRAINT `FK_LikeAnswers_Answers_AnswerId` FOREIGN KEY (`AnswerId`) REFERENCES `answers` (`AnswerId`) ON DELETE CASCADE,
  CONSTRAINT `FK_LikeAnswers_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `likeanswers`
--

LOCK TABLES `likeanswers` WRITE;
/*!40000 ALTER TABLE `likeanswers` DISABLE KEYS */;
/*!40000 ALTER TABLE `likeanswers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `likequestions`
--

DROP TABLE IF EXISTS `likequestions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `likequestions` (
  `UserId` int NOT NULL,
  `QuestionId` int NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionId`,`UserId`),
  KEY `IX_LikeQuestions_UserId` (`UserId`),
  CONSTRAINT `FK_LikeQuestions_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`QuestionId`) ON DELETE CASCADE,
  CONSTRAINT `FK_LikeQuestions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `likequestions`
--

LOCK TABLES `likequestions` WRITE;
/*!40000 ALTER TABLE `likequestions` DISABLE KEYS */;
/*!40000 ALTER TABLE `likequestions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `natrualanswers`
--

DROP TABLE IF EXISTS `natrualanswers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `natrualanswers` (
  `NatrualAnswerId` int NOT NULL AUTO_INCREMENT,
  `Content` longtext NOT NULL,
  PRIMARY KEY (`NatrualAnswerId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `natrualanswers`
--

LOCK TABLES `natrualanswers` WRITE;
/*!40000 ALTER TABLE `natrualanswers` DISABLE KEYS */;
/*!40000 ALTER TABLE `natrualanswers` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `natrualquestionanswerrelations`
--

DROP TABLE IF EXISTS `natrualquestionanswerrelations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `natrualquestionanswerrelations` (
  `NaturalQuestionId` int NOT NULL,
  `NatrualAnswerId` int NOT NULL,
  PRIMARY KEY (`NaturalQuestionId`,`NatrualAnswerId`),
  KEY `IX_NatrualQuestionAnswerRelations_NatrualAnswerId` (`NatrualAnswerId`),
  CONSTRAINT `FK_NatrualQuestionAnswerRelations_NatrualAnswers_NatrualAnswerId` FOREIGN KEY (`NatrualAnswerId`) REFERENCES `natrualanswers` (`NatrualAnswerId`) ON DELETE CASCADE,
  CONSTRAINT `FK_NatrualQuestionAnswerRelations_NaturalQuestions_NaturalQuest~` FOREIGN KEY (`NaturalQuestionId`) REFERENCES `naturalquestions` (`NaturalQuestionId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `natrualquestionanswerrelations`
--

LOCK TABLES `natrualquestionanswerrelations` WRITE;
/*!40000 ALTER TABLE `natrualquestionanswerrelations` DISABLE KEYS */;
/*!40000 ALTER TABLE `natrualquestionanswerrelations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `naturalquestions`
--

DROP TABLE IF EXISTS `naturalquestions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `naturalquestions` (
  `NaturalQuestionId` int NOT NULL AUTO_INCREMENT,
  `Content` longtext NOT NULL,
  PRIMARY KEY (`NaturalQuestionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `naturalquestions`
--

LOCK TABLES `naturalquestions` WRITE;
/*!40000 ALTER TABLE `naturalquestions` DISABLE KEYS */;
/*!40000 ALTER TABLE `naturalquestions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questionhotdatas`
--

DROP TABLE IF EXISTS `questionhotdatas`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questionhotdatas` (
  `QuestionId` int NOT NULL,
  `HotValue` int NOT NULL,
  `ModifyTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionId`),
  CONSTRAINT `FK_QuestionHotDatas_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`QuestionId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questionhotdatas`
--

LOCK TABLES `questionhotdatas` WRITE;
/*!40000 ALTER TABLE `questionhotdatas` DISABLE KEYS */;
/*!40000 ALTER TABLE `questionhotdatas` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questions`
--

DROP TABLE IF EXISTS `questions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questions` (
  `QuestionId` int NOT NULL AUTO_INCREMENT,
  `BestAnswerId` int DEFAULT NULL,
  `UserId` int DEFAULT NULL,
  `Title` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Remarks` longtext NOT NULL,
  `CreateTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `ModifyTime` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`QuestionId`),
  KEY `IX_Questions_UserId` (`UserId`),
  CONSTRAINT `FK_Questions_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`UserId`) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questions`
--

LOCK TABLES `questions` WRITE;
/*!40000 ALTER TABLE `questions` DISABLE KEYS */;
/*!40000 ALTER TABLE `questions` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `questiontagrelations`
--

DROP TABLE IF EXISTS `questiontagrelations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `questiontagrelations` (
  `TagId` int NOT NULL,
  `QuestionId` int NOT NULL,
  PRIMARY KEY (`QuestionId`,`TagId`),
  KEY `IX_QuestionTagRelations_TagId` (`TagId`),
  CONSTRAINT `FK_QuestionTagRelations_Questions_QuestionId` FOREIGN KEY (`QuestionId`) REFERENCES `questions` (`QuestionId`) ON DELETE CASCADE,
  CONSTRAINT `FK_QuestionTagRelations_Tags_TagId` FOREIGN KEY (`TagId`) REFERENCES `tags` (`TagId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `questiontagrelations`
--

LOCK TABLES `questiontagrelations` WRITE;
/*!40000 ALTER TABLE `questiontagrelations` DISABLE KEYS */;
/*!40000 ALTER TABLE `questiontagrelations` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tags`
--

DROP TABLE IF EXISTS `tags`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tags` (
  `TagId` int NOT NULL AUTO_INCREMENT,
  `Category` int NOT NULL,
  `Name` varchar(16) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Desc` text NOT NULL,
  PRIMARY KEY (`TagId`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tags`
--

LOCK TABLES `tags` WRITE;
/*!40000 ALTER TABLE `tags` DISABLE KEYS */;
/*!40000 ALTER TABLE `tags` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `UserId` int NOT NULL AUTO_INCREMENT,
  `Email` varchar(254) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Bcrypt` char(60) NOT NULL,
  `Name` varchar(18) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Auth` int NOT NULL,
  `ProfilePhoto` int NOT NULL,
  PRIMARY KEY (`UserId`),
  UNIQUE KEY `Name` (`Name`),
  UNIQUE KEY `AK_Users_Email` (`Email`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2021-06-15 17:14:25
