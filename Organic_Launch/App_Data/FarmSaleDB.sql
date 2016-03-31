
IF OBJECT_ID('Account', 'U')   
       IS NOT NULL DROP TABLE Account;
IF OBJECT_ID('Product', 'U')   
       IS NOT NULL DROP TABLE Product;
IF OBJECT_ID('Farm', 'U')   
       IS NOT NULL DROP TABLE Farm;
IF OBJECT_ID('Address', 'U')   
       IS NOT NULL DROP TABLE Address;
IF OBJECT_ID('AccountDetail', 'U')   
       IS NOT NULL DROP TABLE AccountDetail;
IF OBJECT_ID('FarmProduct', 'U')   
       IS NOT NULL DROP TABLE FarmProduct;
IF OBJECT_ID('[Order]', 'U')   
       IS NOT NULL DROP TABLE [Order];
GO
 
CREATE TABLE Account (
       accountID            INT IDENTITY PRIMARY KEY,
       username             VARCHAR(50),
       password             VARCHAR(50),
       email                VARCHAR(50),
       accountType          VARCHAR(50)
       );
CREATE TABLE Product (
       productID                  INT IDENTITY PRIMARY KEY,
       productName                VARCHAR(50),
       price                      DECIMAL(19, 2),
       productCategory            VARCHAR(50),
       productDescription  VARCHAR(255)
  );
CREATE TABLE Farm (
       farmID                     INT IDENTITY PRIMARY KEY,
       farmName                   VARCHAR(50),
       farmProfile                VARCHAR(255)
  );
CREATE TABLE Address (
       addressID            INT IDENTITY PRIMARY KEY,
       streetNum            VARCHAR(50),
       streetName           VARCHAR(50),
       province             VARCHAR(50),
       city                 VARCHAR(50),
       zip                        VARCHAR(50)
);
CREATE TABLE AccountDetail (
       accountDetailID INT IDENTITY PRIMARY KEY,
       accountID       INT,
       farmID          INT,
       addressID       INT
);
CREATE TABLE FarmProduct (
       farmProductID INT IDENTITY PRIMARY KEY,
       qty           INT,
       farmID        INT,
       productID     INT,
);
CREATE TABLE [Order] (
       orderID       INT IDENTITY PRIMARY KEY ,
       accountID     INT,
       farmProductID INT
);
 
ALTER TABLE AccountDetail ADD CONSTRAINT FKAccount1 FOREIGN KEY (accountID) REFERENCES Account (accountID);
ALTER TABLE AccountDetail ADD CONSTRAINT FKAccount2 FOREIGN KEY (farmID) REFERENCES Farm (farmID);
ALTER TABLE FarmProduct ADD CONSTRAINT FKFarmProduct1 FOREIGN KEY (farmID) REFERENCES Farm (farmID);
ALTER TABLE FarmProduct ADD CONSTRAINT FKFarmProduct2 FOREIGN KEY (productID) REFERENCES Product (productID);
ALTER TABLE [Order] ADD CONSTRAINT FKOrder1 FOREIGN KEY (accountID) REFERENCES Account (accountID);
ALTER TABLE [Order] ADD CONSTRAINT FKOrder2 FOREIGN KEY (farmProductID) REFERENCES FarmProduct (farmProductID);
ALTER TABLE AccountDetail ADD CONSTRAINT FKAccountDet1 FOREIGN KEY (addressID) REFERENCES Address (addressID);
 
INSERT INTO Account VALUES('Tin', 'bcitpassword', 'tlau@my.bcit.ca', 'farmer');
INSERT INTO Product VALUES('Lemons', 40.34, 'Fruit', 'Fresh lemons.');
INSERT INTO Farm VALUES('Partridge Farms', 'Patridge farms was established during the early American pioneer days in the days of Fort Vancouver. We have been producing poultry and lemons ever since.');
INSERT INTO Address VALUES('1071', 'Harold Road', 'BC', 'Vancouver', 'Canada');
INSERT INTO FarmProduct VALUES(5, 1, 1);
INSERT INTO [Order] VALUES(1, 1);
 
INSERT INTO Account VALUES('Slav', 'capilanopassword', 'svislas@my.sfu.ca', 'buyer');
INSERT INTO Product VALUES('Oranges', 1.45, 'Fruit', 'Well-ripened oranges.');
INSERT INTO Farm VALUES('Orangeville Farms', 'Organville farms takes citrus seriously. Deliscous, well-ripened fruit makes for excellent quality refreshments.');
INSERT INTO Address VALUES('234', 'Stadium Road', 'BC', 'Vancouver', 'Canada');
INSERT INTO FarmProduct VALUES(5, 1, 1);
INSERT INTO [Order] VALUES(1, 1);
 
INSERT INTO Account VALUES('Marrion', 'ubcpassword', 'mlulu@my.ubc.ca', 'buyer');
INSERT INTO Product VALUES('Kobe Beef', 34578.34, 'Beef', '1-month aged Kobe beef.');
INSERT INTO Farm VALUES('Japan Beef Farms', 'The Kobe beef producer of Canada is regarded as the highest quality beef producer in the Pacific Northwest area.');
INSERT INTO Address VALUES('54664', 'King George Road', 'BC', 'Prince George', 'Canada');
INSERT INTO FarmProduct VALUES(5, 1, 1);
INSERT INTO [Order] VALUES(1, 1);
 
SELECT * FROM Account;
SELECT * FROM Product;
SELECT * FROM Farm;
SELECT * FROM Address;
SELECT * FROM FarmProduct;
SELECT * FROM [Order];