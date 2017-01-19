CREATE TABLE pogovor
(
Id int IDENTITY(1,1) NOT NULL,
username varchar(255),
PRIMARY KEY (Id),
);

CREATE TABLE sporocila
(
ID int IDENTITY(1,1) NOT NULL,
username varchar(255),
fullname varchar(255),
time varchar(255),
message varchar(255),
PRIMARY KEY (Id)
);

CREATE TABLE uporabniki
(
uporabniskoIme varchar(255) NOT NULL,
imePriimek varchar(255),
geslo varchar(255),
admin bit,
stSporocil int,
PRIMARY KEY (uporabniskoIme)
);