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

INSERT INTO drinks (Name) VALUES ('Jagger Bomb');
INSERT INTO drinks (Name) VALUES ('Dark n Stormy');
INSERT INTO drinks (Name) VALUES ('Rum n Coke');

INSERT INTO ingredients (Name) VALUES ('Jagermeister');
INSERT INTO ingredients (Name) VALUES ('Rhum');
INSERT INTO ingredients (Name) VALUES ('Coke');
INSERT INTO ingredients (Name) VALUES ('Biere de gingembre');
INSERT INTO ingredients (Name) VALUES ('Redbull');

INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (1, 1, 1);
INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (1, 5, 1);
INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (2, 2, 1);
INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (2, 4, 3);
INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (1, 2, 2);
INSERT INTO recipes (DrinkId, IngredientId, Quantity) VALUES (2, 3, 4);
