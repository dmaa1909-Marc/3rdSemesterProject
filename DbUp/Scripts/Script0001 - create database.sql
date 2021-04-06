USE [master]
GO

/****** Object:  Database [Decks]    Script Date: 19-11-2020 11:06:21 ******/
IF DB_ID('Decks') IS NOT NULL
BEGIN
 ALTER DATABASE Decks
 SET SINGLE_USER WITH
 ROLLBACK IMMEDIATE
 EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'Decks'
 DROP DATABASE [Decks]
END
GO

IF DB_ID('DecksMVCUser') IS NOT NULL
BEGIN
 ALTER DATABASE DecksMVCUser
 SET SINGLE_USER WITH
 ROLLBACK IMMEDIATE
 EXEC msdb.dbo.sp_delete_database_backuphistory @database_name = N'DecksMVCUser'
 DROP DATABASE [DecksMVCUser]
END
GO

/****** Object:  Database [Decks]    Script Date: 19-11-2020 11:06:21 ******/
CREATE DATABASE [Decks]
CREATE DATABASE [DecksMVCUser]