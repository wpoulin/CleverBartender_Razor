CREATE DATABASE clever_bartender;
USE clever_bartender;

CREATE TABLE drinks (
    Id int NOT NULL AUTO_INCREMENT,
    Name Varchar(255) NOT NULL,
    PRIMARY KEY (Id)
);

CREATE TABLE ingredients (
    Id int NOT NULL AUTO_INCREMENT,
    Name Varchar(255) NOT NULL,
    PumpNumber int,
    PRIMARY KEY (Id)
);

CREATE TABLE recipes (
    Id int NOT NULL AUTO_INCREMENT,
    Quantity int NOT NULL,
    DrinkId int NOT NULL,
    IngredientId int NOT NULL,
    PRIMARY KEY (Id),
    FOREIGN KEY (DrinkId) REFERENCES drinks(Id),
    FOREIGN KEY (IngredientId) REFERENCES ingredients(Id)
);

CREATE TABLE mobileNodes (
    Id int NOT NULL AUTO_INCREMENT,
    Name Varchar(255) NOT NULL,
    IpAddress Varchar(255) NOT NULL,
    PRIMARY KEY (Id)
);

INSERT INTO drinks (Name) VALUES ('Rhum n coke');
INSERT INTO drinks (Name) VALUES ('Rhum n 7up');
INSERT INTO drinks (Name) VALUES ('Jus de raisin');

INSERT INTO ingredients (Name, PumpNumber) VALUES ('Rhum', 1);
INSERT INTO ingredients (Name, PumpNumber) VALUES ('Coke', 2);
INSERT INTO ingredients (Name, PumpNumber) VALUES ('7up', 3);
INSERT INTO ingredients (Name, PumpNumber) VALUES ('Jus de raisin', 4);
INSERT INTO ingredients (Name) VALUES ('Redbull');

INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (1, 1, 2);
INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (1, 2, 4);

INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (2, 1, 1);
INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (2, 3, 5);

INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (3, 4, 6);

INSERT INTO mobileNodes (Name, IpAddress) VALUES ('Noeud Mobile 1','127.0.0.1');

alter table ingredients
add Constraint test Unique (PumpNumber)