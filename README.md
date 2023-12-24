# WebsiteBanCaPhe
# Website Bán Cà Phê

Đây là project đồ án môn Xây dựng hệ thống thông tin trên các framework, sử dụng ASP.NET MVC 6.0. Đề tài Website bán các loại cà phê đóng gói.

## Tính năng cho người dùng

**Trang đăng kí và đăng nhập**

**Trang giới thiệu cửa hàng**

**Trang chỉnh sửa thông tin người dùng**
-Cập nhật tên, số điện thoại, giới tính.

**Trang sản phẩm**
- Hiển thị danh sách sản phẩm

**Các trang sản phẩm theo danh mục**
- Hiển thị danh sách sản phẩm theo danh mục

**Trang chi tiết giỏ hàng**
- Thêm và xoá sản phẩm trong giỏ hàng
- Tạo đơn hàng

**Trang nhập form đơn hàng**
- Nhập thông tin đơn hàng
- Tạo đơn hàng

**Trang quản lý đơn hàng**
- Xem thông tin các đơn hàng.
- Cập nhật thông tin đặt hàng.
- Cập nhật trạng thái nhận hàng.
**Tìm kiếm sản phẩm theo tên**

  
**Lọc sản phẩm theo danh mục, giá, hãng**

**Đánh giá sao, feedback sản phẩm**
- Người dùng chọn vào sản phẩm cần đánh giá.
- Tại trang chi tiết sản phẩm, người dùng có thể đánh giá sản phẩm.

## Tính năng cho ADMIN

**Trang chủ**

**Trang quản lý danh mục**
- Tạo mới danh mục và chỉnh sửa tên danh mục

**Trang quản lý sản phẩm**
- Tạo mới sản phẩm và xoá sản phẩm nếu không có trong giỏ hàng (số lượng = 0)
- Sửa thông tin sản phẩm

**Trang quản lý kho hàng**
- Hiển thị danh sách sản phẩm và số lượng sản phẩm, cập nhật số lượng sản phẩm

**Trang quản lý đơn hàng**
- Cập nhật trạng thái thanh toán cho đơn hàng

**Trang lập báo cáo**
- Báo cáo doanh thu tháng
- Báo cáo tồn kho
- Tải file excel báo cáo

**Trang dashboard**
- Tổng doanh thu tháng, năm
- Biểu đồ doanh thu năm
- Biểu đồ so sánh doanh thu theo danh mục

## Hướng dẫn cài đặt

1. Clone repository này về máy của bạn.

  `git clone https://github.com/nvcong1710/WebsiteBanCaPhe.git`

2. Mở project bằng Visual Studio/Visual Studio Code
3. Chạy file database với SSMS
4. Sửa connection string kết nối với database trong file appsetting.json
5. Chạy trên localhost

## Công nghệ sử dụng

- ASP .NET MVC 6.0
- SQL Server
- Google Chart
- APPlus

## Nhóm thực hiện
Lửa Hận Thù, thành viên:
<ol>
  <li>Nguyễn Viết Công - Nhóm trưởng</li>
  <li>Lê Trần Anh Quí</li>
  <li>Nguyễn Hữu Phụng</li>
  <li>Nguyễn Minh Quang</li>
  <li>Đặng Lưu Hà</li>
</ol>
