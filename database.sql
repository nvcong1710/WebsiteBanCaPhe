CREATE TABLE [dbo].[Account] (
    [AccountId]   INT            IDENTITY (1, 1) NOT NULL,
    [PhoneNumber] VARCHAR (10)   NOT NULL,
    [Password]    VARCHAR (10)   NOT NULL,
    [FullName]    NVARCHAR (100) NOT NULL,
    [Gender]      NVARCHAR (4)   NOT NULL,
    PRIMARY KEY CLUSTERED ([AccountId] ASC)
);

CREATE TABLE [dbo].[Category] (
    [CategoryId]   INT            IDENTITY (1, 1) NOT NULL,
    [CategoryName] NVARCHAR (255) NOT NULL,
    PRIMARY KEY CLUSTERED ([CategoryId] ASC)
);

CREATE TABLE [dbo].[Product] (
    [ProductId]          INT            IDENTITY (1, 1) NOT NULL,
    [ProductName]        NVARCHAR (255) NOT NULL,
    [Price]              BIGINT         NOT NULL,
    [Quantity]           BIGINT         NOT NULL,
    [Origin]             NVARCHAR (255) NULL,
    [PhotoURL]           VARCHAR (255)  NULL,
    [ProductDescription] NVARCHAR (MAX) NULL,
    [Branch]             NVARCHAR (255) NULL,
    [CategoryId]         INT            NULL,
    [QuantitySold]       BIGINT         NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC),
    FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([CategoryId])
);

CREATE TABLE [dbo].[Cart] (
    [CartId]     INT    IDENTITY (1, 1) NOT NULL,
    [TotalValue] BIGINT NOT NULL,
    [AccountId]  INT    NULL,
    PRIMARY KEY CLUSTERED ([CartId] ASC),
    FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([AccountId])
);

CREATE TABLE [dbo].[CartDetail] (
    [CartDetailId] INT    IDENTITY (1, 1) NOT NULL,
    [CartId]       INT    NOT NULL,
    [ProductId]    INT    NULL,
    [Quantity]     BIGINT NOT NULL,
    [TotalPrice]   BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([CartDetailId] ASC),
    FOREIGN KEY ([CartId]) REFERENCES [dbo].[Cart] ([CartId]),
    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId])
);

CREATE TABLE [dbo].[UserOrder] (
    [OrderId]       INT            IDENTITY (1, 1) NOT NULL,
    [OrderDate]     DATETIME       NOT NULL,
    [ReceiverName]  NVARCHAR (255) NOT NULL,
    [PhoneNumber]   VARCHAR (10)   NOT NULL,
    [Address]       NVARCHAR (255) NOT NULL,
    [PaymentMethod] NVARCHAR (255) NOT NULL,
    [Note]          NVARCHAR (255) NULL,
    [ShippingFee]   BIGINT         NOT NULL,
    [TotalValue]    BIGINT         NOT NULL,
    [AccountId]     INT            NULL,
    [IsDone]        BIT            DEFAULT ((0)) NULL,
    [IsPaid]        BIT            DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([OrderId] ASC),
    FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([AccountId])
);

CREATE TABLE [dbo].[OrderDetail] (
    [OrderDetailId] INT    IDENTITY (1, 1) NOT NULL,
    [OrderId]       INT    NOT NULL,
    [ProductId]     INT    NULL,
    [Quantity]      BIGINT NOT NULL,
    [TotalPrice]    BIGINT NOT NULL,
    PRIMARY KEY CLUSTERED ([OrderDetailId] ASC),
    FOREIGN KEY ([OrderId]) REFERENCES [dbo].[UserOrder] ([OrderId]),
    FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Product] ([ProductId])
);


INSERT INTO [dbo].[Account] ([AccountId], [PhoneNumber], [Password], [FullName], [Gender]) VALUES (3, N'0999999999', N'123456', N'Lê Trần Anh Quí', N'Nam')
INSERT INTO [dbo].[Account] ([AccountId], [PhoneNumber], [Password], [FullName], [Gender]) VALUES (1003, N'0987777777', N'123456', N'Nguyễn Viết Công', N'Nam')


INSERT INTO [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (1, N'Cà phê hoà tan')
INSERT INTO [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (2, N'Cà phê rang xay')
INSERT INTO [dbo].[Category] ([CategoryId], [CategoryName]) VALUES (3, N'Cà phê hạt')


INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [Price], [Quantity], [Origin], [PhotoURL], [ProductDescription], [Branch], [CategoryId], [QuantitySold]) VALUES (2, N'Cà phê hòa tan cao cấp Starbucks: Caffe Mocha', 218000, 0, N'Mỹ', N'/img/Assets/StarBucksCaffeMocha.webp', N'Ðược sản xuất dựa trên công nghệ hàng đầu', N'Trung Nguyên', 2, 281)
INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [Price], [Quantity], [Origin], [PhotoURL], [ProductDescription], [Branch], [CategoryId], [QuantitySold]) VALUES (3, N'Cà phê G7 3in1', 247000, 100, N'Việt Nam', N'\img\Assets\G7_3in1_Box.webp', N'Cà phê G7 3in1 được chiết xuất từ những phần tinh túy nhất có trong từng hạt cà phê để cho ra đời sản phẩm cà phê hòa tan thượng hạng.', N'Trung Nguyên', 1, 298)
INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [Price], [Quantity], [Origin], [PhotoURL], [ProductDescription], [Branch], [CategoryId], [QuantitySold]) VALUES (4, N'Cà phê Chế phin 3 Trung Nguyên', 123000, 375, N'Việt Nam', N'\img\Assets\ChePhin3.webp', N'Các hạt cà phê được chọn lọc theo một tiêu chuẩn nhất định và thông qua quá trình sàng lọc đặc biệt. Điều này tạo ra cho cà phê chế phin số 3 một sự khác biệt hoàn toàn về hương vị.', N'Trung Nguyên', 1, 116)
INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [Price], [Quantity], [Origin], [PhotoURL], [ProductDescription], [Branch], [CategoryId], [QuantitySold]) VALUES (5, N'Cà phê Latte Starbucks', 143000, 820, N'Mỹ', N'\img\Assets\StarBucksCaffeLatte.webp', N'Thức uống mượt mà vị sữa, kết hợp 100% hạt cà phê Aracbica tại cửa hàng và phủ lớp bọt sữa mềm mịn, thơm dịu khó cưỡng.', N'Starbucks', 1, 254)
INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [Price], [Quantity], [Origin], [PhotoURL], [ProductDescription], [Branch], [CategoryId], [QuantitySold]) VALUES (6, N'Cà phê Caramel Latte Starbucks', 147000, 209, N'Mỹ', N'\img\Assets\StarBucksCaramelLatte.webp', N'Hương vị êm mượt, thơm dịu caramel, kết hợp hoàn hảo giữa 100% hạt cà phê Aracbica cùng sữa và bơ caramel, phủ bên trên một lớp bọt sữa mềm mịn.', N'Starbucks', 1, 91)
INSERT INTO [dbo].[Product] ([ProductId], [ProductName], [Price], [Quantity], [Origin], [PhotoURL], [ProductDescription], [Branch], [CategoryId], [QuantitySold]) VALUES (7, N'Cà phê Mocha Starbucks', 147000, 220, N'Mỹ', N'\img\Assets\StarBucksCaffeMocha.webp', N'Thức uống đậm vị chocolate được kết hợp từ 100% hạt cà phê Arabica nguyên chất, ca cao và phủ lên trên lớp bọt sữa mềm mịn.', N'Starbucks', 1, 180)


INSERT INTO [dbo].[Cart] ([CartId], [TotalValue], [AccountId]) VALUES (1, 0, 3)
INSERT INTO [dbo].[Cart] ([CartId], [TotalValue], [AccountId]) VALUES (2, 0, 1003)


INSERT INTO [dbo].[CartDetail] ([CartDetailId], [CartId], [ProductId], [Quantity], [TotalPrice]) VALUES (9010, 2, 2, 14, 3052000)
INSERT INTO [dbo].[CartDetail] ([CartDetailId], [CartId], [ProductId], [Quantity], [TotalPrice]) VALUES (10010, 2, 3, 1, 247000)
INSERT INTO [dbo].[CartDetail] ([CartDetailId], [CartId], [ProductId], [Quantity], [TotalPrice]) VALUES (11012, 1, 3, 5, 1235000)
INSERT INTO [dbo].[CartDetail] ([CartDetailId], [CartId], [ProductId], [Quantity], [TotalPrice]) VALUES (11013, 1, 7, 8, 1176000)


INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (4003, N'2023-11-14 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Thanh toán ngân hàng', NULL, 0, 11975000, 3, 1, 1)
INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (4004, N'2023-11-14 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Thanh toán ngân hàng', NULL, 0, 2617000, 3, 0, 1)
INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (5002, N'2023-11-15 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Ship COD', NULL, 0, 20690000, 1003, 0, 1)
INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (5003, N'2023-11-15 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Thanh toán ngân hàng', NULL, 0, 21582000, 1003, 0, 1)
INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (6002, N'2023-11-15 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Ship COD', NULL, 0, 1430000, 3, 1, 0)
INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (7002, N'2023-11-15 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Ship COD', NULL, 0, 45942000, 3, 0, 1)
INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (8004, N'2023-11-16 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Momo', NULL, 0, 109494000, 3, 1, 0)
INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (8005, N'2023-11-16 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Momo', NULL, 0, 247000, 3, 0, 0)
INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (9004, N'2023-11-16 00:00:00', N'Lê Trần Anh Quí', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Thanh toán ngân hàng', NULL, 0, 858000, 1003, 0, 0)
INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (9005, N'2023-11-16 00:00:00', N'Lê Trần Anh Quí', N'0999888999', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Momo', NULL, 0, 5751000, 1003, 0, 0)
INSERT INTO [dbo].[UserOrder] ([OrderId], [OrderDate], [ReceiverName], [PhoneNumber], [Address], [PaymentMethod], [Note], [ShippingFee], [TotalValue], [AccountId], [IsDone], [IsPaid]) VALUES (10004, N'2023-11-17 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Thanh toán ngân hàng', NULL, 0, 4705000, 3, 0, 0)



INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (4003, 4003, 4, 15, 1845000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (4004, 4003, 5, 40, 5720000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (4005, 4003, 7, 30, 4410000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (4006, 4004, 5, 14, 2002000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (4007, 4004, 4, 5, 615000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (5002, 5002, 3, 14, 3458000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (5003, 5002, 6, 6, 882000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (5004, 5002, 2, 75, 16350000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (5005, 5003, 2, 99, 21582000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (6002, 6002, 5, 10, 1430000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (7002, 7002, 3, 186, 45942000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (8002, 8004, 6, 85, 12495000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (8003, 8004, 3, 88, 21736000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (8004, 8004, 4, 81, 9963000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (8005, 8004, 5, 150, 21450000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (8006, 8004, 2, 100, 21800000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (8007, 8004, 7, 150, 22050000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (8008, 8005, 3, 1, 247000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (9008, 9004, 5, 6, 858000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (9009, 9005, 3, 9, 2223000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (9010, 9005, 2, 7, 1526000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (9011, 9005, 5, 14, 2002000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (10008, 10004, 4, 15, 1845000)
INSERT INTO [dbo].[OrderDetail] ([OrderDetailId], [OrderId], [ProductId], [Quantity], [TotalPrice]) VALUES (10009, 10004, 5, 20, 2860000)
