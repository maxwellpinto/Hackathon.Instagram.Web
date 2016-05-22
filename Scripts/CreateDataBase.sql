CREATE DATABASE Hackathon_DB;
GO
USE Hackathon_DB;

CREATE TABLE DataAccessInstagram
(
	Id uniqueidentifier not null,
	RegistrationDate DateTime not null,
	DueDate DateTime null,
	IsValid bit null,
	IdUser bigint not null,
	UserName varchar(max) null,
	FullName varchar(max) null,
	ProfilePicture varchar(max) null,
	AccessToken varchar(max) null,

	CONSTRAINT DataAcecessInstagram_PK PRIMARY KEY (Id)
);

