DROP DATABASE IF EXISTS ANHNGUYENCLAYPOT_DB
GO
CREATE DATABASE ANHNGUYENCLAYPOT_DB
GO
USE ANHNGUYENCLAYPOT_DB
GO
CREATE TABLE [Customer] (
	[CustomerID] INT NOT NULL IDENTITY,
	[CustomerName] NVARCHAR(100) NOT NULL,
	[CustomerPhone] NVARCHAR(11) NOT NULL UNIQUE,
	[Address] NVARCHAR(255),
	[Password] NVARCHAR(30) NOT NULL,
	PRIMARY KEY([CustomerID])
);
GO

CREATE TABLE [Branch] (
	[BranchID] INT NOT NULL IDENTITY,
	[BranchName] NVARCHAR(255) NOT NULL,
	PRIMARY KEY([BranchID])
);
GO

CREATE TABLE [Staff] (
	[StaffID] INT NOT NULL IDENTITY,
	[StaffName] NVARCHAR(100) NOT NULL,
	[StaffPhone] NVARCHAR(11) NOT NULL UNIQUE,
	[BranchID] INT NOT NULL,
	[Password] NVARCHAR(30) NOT NULL,
	PRIMARY KEY([StaffID])
);
GO

CREATE TABLE [Food] (
	[FoodID] INT NOT NULL IDENTITY,
	[FoodVietnameseName] NVARCHAR(150) NOT NULL,
	[FoodEnglishName] NVARCHAR(150) NOT NULL,
	[FoodPrice] INT NOT NULL,
	PRIMARY KEY([FoodID])
);
GO
CREATE TABLE [Order] (
	[OrderID] INT NOT NULL IDENTITY,
	[CustomerID] INT,
	[BranchID] INT NOT NULL,
	[Discount] INT,
	[Status] SMALLINT NOT NULL,
	[OrderDate] DATE,
	[InTime] TIME,
	[OutTime] TIME,
	[TotalPrice] INT NOT NULL,
	PRIMARY KEY([OrderID])
);
GO

CREATE TABLE [OrderDetail] (
	[OrderID] INT NOT NULL,
	[FoodID] INT NOT NULL,
	[Quantity] INT NOT NULL,
	PRIMARY KEY([OrderID], [FoodID])
);
GO

ALTER TABLE [Staff]
ADD FOREIGN KEY([BranchID]) REFERENCES [Branch]([BranchID])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Order]
ADD FOREIGN KEY([CustomerID]) REFERENCES [Customer]([CustomerID])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [Order]
ADD FOREIGN KEY([BranchID]) REFERENCES [Branch]([BranchID])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [OrderDetail]
ADD FOREIGN KEY([OrderID]) REFERENCES [Order]([OrderID])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO
ALTER TABLE [OrderDetail]
ADD FOREIGN KEY([FoodID]) REFERENCES [Food]([FoodID])
ON UPDATE NO ACTION ON DELETE NO ACTION;
GO

INSERT INTO Customer (CustomerName,CustomerPhone,[Address],[Password])
VALUES
('John Doe', '0123456789', '123 Maple Street', 'password123'),
('Jane Smith', '0987654321', '456 Oak Avenue', 'qwerty456'),
('Alice Johnson', '0123459876', '789 Pine Road', 'alice7890'),
('Bob Brown', '0973060017', '321 Elm Street', 'securePass!'),
('Charlie Davis', '0123498765', '654 Birch Lane', 'charlieP@ss'),
('Eve Black', '0567891234', '987 Cedar Boulevard', 'evePassword'),
('Frank Green', '0456789123', '321 Spruce Terrace', 'frankieG'),
('Grace White', '0234567891', '654 Willow Way', 'grace1234'),
('Hank Miller', '0345678912', '987 Aspen Drive', 'hankMiller#'),
('Ivy Clark', '0456712345', '123 Redwood Court', 'ivyClark!'),
('Jake Wilson', '0678912345', '789 Maple Crescent', 'jakePass123'),
('Kara Lee', '0789123456', '456 Oak Place', 'karaSecure1'),
('Liam Turner', '0891234567', '123 Pine Grove', 'liamT987'),
('Mia King', '01234567855', '789 Birch Alley', 'miaKing!'),
('Noah Scott', '0438221522', '456 Cedar Circle', 'noah1234');

INSERT INTO Branch (BranchName)
VALUES
(N'B32 Cốm Vòng – Dịch Vọng Hậu – Cầu Giấy – Hà Nội'),
(N'Số 37 Phố Dương Khuê – Mai Dịch – Cầu Giấy – Hà Nội (Cạnh Đại học Thương Mại)'),
(N'Số 17 Chùa Láng – Đống Đa – Hà Nội (Cạnh Đại học Ngoại Thương)'),
(N'Ngõ 66 Dịch Vọng Hậu – Cầu Giấy – Hà Nội'),
(N'65 Nguyễn Đăng Đạo – TP Bắc Ninh'),
(N'40 42 44 Phạm Văn Đồng – Bắc Từ Liêm – Hà Nội'),
(N'142 Hoàng Quốc Việt – Nghĩa Tân – Cầu Giấy – Hà Nội'),
(N'Thôn 3, Thạch Hòa, Thạch Thất, Hà Nội'),
(N'Khu vườn đào, Đường Hoa Đào A1, Thôn Phúc Lộc, Uy Nỗ Đông Anh, Hà Nội'),
(N'861 Ngô Gia Tự, Phường Đức Giang, Long Biên, Hà Nội');

INSERT INTO Staff (StaffName,StaffPhone,BranchID,[Password])
VALUES
(N'Nguyễn Bá Đức Minh','0121212121',1,'123@'),
(N'Vũ Khánh Hoàng','0343434343',8,'123@'),
(N'Đặng Quốc Vương','0565656565',4,'123@'),
(N'Trần Đức Anh','0787878787',6,'123@'),
(N'Bùi Minh Đức','0909090909',3,'123@');

INSERT INTO Food (FoodVietnameseName,FoodEnglishName,FoodPrice)
VALUES
(N'Cơm chiên Dương Châu','Duong Chau claypot rice',35),
(N'Cơm thố Sườn','Rib claypot rice with cartilage ribs',50),
(N'Cơm thố Bò','Beef claypot rice',35),
(N'Cơm thố Gà','Chicken claypot rice',45),
(N'Cơm thố Xá Xíu','Xa Xiu claypot rice',50),
(N'Cơm thố Đặc Biệt','Special claypot rice',70),
(N'Cơm thố Bò + Gà','Beef + Chicken claypot rice',60),
(N'Coca','Coke',15),
(N'Nước cam','Orange juice',15),
(N'Nước suối','Water',10);

-- Order and OrderDetail is generated while app is working
INSERT INTO [Order] (CustomerID, BranchID, Discount, Status, OrderDate, InTime, OutTime, TotalPrice)
VALUES
(1, 1, 0, 6, '2024-06-25','11:00:00 AM','11:30:00 AM',60),
(null, 6, 0, 4, '2024-06-27',null,null, 50);

INSERT INTO OrderDetail
VALUES
(1, 4, 1), (1, 8, 1), (2, 2, 1)