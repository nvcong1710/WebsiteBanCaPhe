-- Tạo cơ sở dữ liệu
CREATE DATABASE WebsiteBanCaPhe;
GO

-- Sử dụng cơ sở dữ liệu vừa tạo
USE WebsiteBanCaPhe;
GO

-- Tạo bảng Account
CREATE TABLE Account (
    AccountId INT IDENTITY(1,1) PRIMARY KEY,
    PhoneNumber VARCHAR(10) NOT NULL,
    Password VARCHAR(10) NOT NULL,
    FullName NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(4) NOT NULL
);
GO

-- Thêm dữ liệu vào bảng Account
INSERT INTO Account (PhoneNumber, Password, FullName, Gender) VALUES 
(N'0999999999', N'123456', N'Lê Trần Anh Quí', N'Nam'),
(N'0987777777', N'123456', N'Nguyễn Viết Công', N'Nam');

-- Tạo bảng Category
CREATE TABLE Category (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(255) NOT NULL
);
GO
-- Thêm dữ liệu vào bảng Category
INSERT INTO Category (CategoryName) VALUES 
(N'Cà phê hoà tan'),
(N'Cà phê rang xay'),
(N'Cà phê hạt');

-- Tạo bảng Product
CREATE TABLE Product (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(255) NOT NULL,
    Price BIGINT NOT NULL,
    Quantity BIGINT NOT NULL,
    Origin NVARCHAR(255),
    PhotoURL VARCHAR(255),
    ProductDescription NVARCHAR(MAX),
    Branch NVARCHAR(255),
    CategoryId INT,
    QuantitySold BIGINT,
    Star INT,
    FOREIGN KEY (CategoryId) REFERENCES Category(CategoryId)
);
GO
-- Thêm dữ liệu vào bảng Product
INSERT INTO Product (ProductName, Price, Quantity, Origin, PhotoURL, ProductDescription, Branch, CategoryId, QuantitySold, Star) VALUES 
(N'Cà phê hòa tan cao cấp Starbucks: Caffe Mocha', 218000, 0, N'Mỹ', N'/img/Assets/StarBucksCaffeMocha.webp', N'Ðược sản xuất dựa trên công nghệ hàng đầu', N'Trung Nguyên', 2, 281, 4),
(N'Cà phê G7 3in1', 247000, 100, N'Việt Nam', N'\img\Assets\G7_3in1_Box.webp', N'Cà phê G7 3in1 được chiết xuất từ những phần tinh túy nhất có trong từng hạt cà phê để cho ra đời sản phẩm cà phê hòa tan thượng hạng.', N'Trung Nguyên', 1, 298, 5),
(N'Cà phê Chế phin 3 Trung Nguyên', 123000, 375, N'Việt Nam', N'\img\Assets\ChePhin3.webp', N'Các hạt cà phê được chọn lọc theo một tiêu chuẩn nhất định và thông qua quá trình sàng lọc đặc biệt. Điều này tạo ra cho cà phê chế phin số 3 một sự khác biệt hoàn toàn về hương vị.', N'Trung Nguyên', 1, 116, 4),
(N'Cà phê Latte Starbucks', 143000, 820, N'Mỹ', N'\img\Assets\StarBucksCaffeLatte.webp', N'Thức uống mượt mà vị sữa, kết hợp 100% hạt cà phê Aracbica tại cửa hàng và phủ lớp bọt sữa mềm mịn, thơm dịu khó cưỡng.', N'Starbucks', 1, 254, 5),
(N'Cà phê Caramel Latte Starbucks', 147000, 209, N'Mỹ', N'\img\Assets\StarBucksCaramelLatte.webp', N'Hương vị êm mượt, thơm dịu caramel, kết hợp hoàn hảo giữa 100% hạt cà phê Aracbica cùng sữa và bơ caramel, phủ bên trên một lớp bọt sữa mềm mịn.', N'Starbucks', 1, 91, 4),
(N'Cà phê Mocha Starbucks', 147000, 220, N'Mỹ', N'\img\Assets\StarBucksCaffeMocha.webp', N'Thức uống đậm vị chocolate được kết hợp từ 100% hạt cà phê Arabica nguyên chất, ca cao và phủ lên trên lớp bọt sữa mềm mịn.', N'Starbucks', 1, 180, 5);


-- Tạo bảng UserOrder
CREATE TABLE UserOrder (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    OrderDate DATETIME NOT NULL,
    ReceiverName NVARCHAR(255) NOT NULL,
    PhoneNumber VARCHAR(10) NOT NULL,
    Address NVARCHAR(255) NOT NULL,
    PaymentMethod NVARCHAR(255) NOT NULL,
    Note NVARCHAR(255),
    ShippingFee BIGINT NOT NULL,
    TotalValue BIGINT NOT NULL,
    AccountId INT,
    IsDone BIT DEFAULT 0,
    IsPaid BIT DEFAULT 0,
    EmailAddress NVARCHAR(MAX),
    FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
);
GO
-- Thêm dữ liệu vào bảng UserOrder
INSERT INTO UserOrder (OrderDate, ReceiverName, PhoneNumber, Address, PaymentMethod, Note, ShippingFee, TotalValue, AccountId, IsDone, IsPaid, EmailAddress) VALUES 
(N'2023-11-14 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Thanh toán ngân hàng', N'Giao hàng nhanh', 50000, 11975000, 2, 1, 1, N'cong.nguyen@example.com'),
(N'2023-11-14 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Thanh toán ngân hàng', N'Giao hàng tiêu chuẩn', 30000, 2617000, 1, 0, 1, N'cong.nguyen@example.com'),
(N'2023-11-15 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Ship COD', N'Giao hàng nhanh', 50000, 20690000, 1, 0, 1, N'cong.nguyen@example.com'),
(N'2023-11-15 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Thanh toán ngân hàng', N'Giao hàng tiêu chuẩn', 30000, 21582000, 1, 0, 1, N'cong.nguyen@example.com'),
(N'2023-11-15 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Ship COD', N'Giao hàng nhanh', 50000, 1430000, 2, 1, 0, N'cong.nguyen@example.com'),
(N'2023-11-15 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Ship COD', N'Giao hàng tiêu chuẩn', 30000, 45942000, 2, 0, 1, N'cong.nguyen@example.com'),
(N'2023-11-17 00:00:00', N'Nguyễn Viết Công', N'0981624798', N'ktx khu B, đại học Quốc gia Hồ Chí Minh', N'Thanh toán ngân hàng', N'Giao hàng nhanh', 50000, 4705000, 2, 0, 0, N'cong.nguyen@example.com');


-- Tạo bảng Cart
CREATE TABLE Cart (
    CartId INT IDENTITY(1,1) PRIMARY KEY,
    TotalValue BIGINT NOT NULL,
    AccountId INT,
    FOREIGN KEY (AccountId) REFERENCES Account(AccountId)
);
GO
-- Thêm dữ liệu vào bảng Cart
INSERT INTO Cart (TotalValue, AccountId) VALUES 
(0, 1),
(0, 2);

-- Tạo bảng CartDetail
CREATE TABLE CartDetail (
    CartDetailId INT IDENTITY(1,1) PRIMARY KEY,
    CartId INT NOT NULL,
    ProductId INT,
    Quantity BIGINT NOT NULL,
    TotalPrice BIGINT NOT NULL,
    FOREIGN KEY (CartId) REFERENCES Cart(CartId),
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);
GO

-- Thêm dữ liệu vào bảng CartDetail
INSERT INTO CartDetail (CartId, ProductId, Quantity, TotalPrice) VALUES 
(2, 2, 14, 3052000),
(2, 3, 1, 247000),
(1, 3, 5, 1235000),
(1, 6, 8, 1176000);
-- Tạo bảng Feedback
CREATE TABLE Feedback (
    FeedbackId INT IDENTITY(1,1) PRIMARY KEY,
    Content NVARCHAR(MAX),
    Star INT,
    FeedbackDate DATETIME,
    AccountId INT,
    ProductId INT,
    FOREIGN KEY (AccountId) REFERENCES Account(AccountId),
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);
GO

-- Thêm dữ liệu vào bảng Feedback
INSERT INTO Feedback (Content, Star, FeedbackDate, AccountId, ProductId) VALUES 
(N'Sản phẩm rất tốt, tôi rất hài lòng!', 5, N'2023-11-14 10:00:00', 1, 1),
(N'Chất lượng sản phẩm không như mong đợi.', 2, N'2023-11-15 12:30:00', 2, 2),
(N'Dịch vụ giao hàng nhanh chóng, sản phẩm đúng như mô tả.', 4, N'2023-11-16 14:45:00', 1, 3),
(N'Giá cả hợp lý, chất lượng ổn.', 3, N'2023-11-17 16:20:00', 2, 4),
(N'Sản phẩm bị lỗi, cần được đổi trả.', 1, N'2023-11-18 18:10:00', 1, 5),
(N'Tôi rất thích sản phẩm này, sẽ mua lại lần sau.', 5, N'2023-11-19 20:00:00', 2, 6);
-- Tạo bảng OrderDetail
CREATE TABLE OrderDetail (
    OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT,
    Quantity BIGINT NOT NULL,
    TotalPrice BIGINT NOT NULL,
    FOREIGN KEY (OrderId) REFERENCES UserOrder(OrderId),
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);
GO

-- Thêm dữ liệu vào bảng OrderDetail
INSERT INTO OrderDetail (OrderId, ProductId, Quantity, TotalPrice) VALUES 
(3, 4, 15, 1845000),
(3, 5, 40, 5720000),
(3, 6, 30, 4410000),
(3, 5, 14, 2002000),
(3, 4, 5, 615000);