# 🌸 FlowerKingdom - Hệ thống Bán Hoa Trực Tuyến

## 1. Giới thiệu
**FlowerKingdom** là hệ thống thương mại điện tử chuyên bán hoa trực tuyến, hỗ trợ khách hàng đặt mua hoa mọi lúc mọi nơi.  
Hệ thống cung cấp trải nghiệm thân thiện, dễ sử dụng, đồng thời giúp quản lý bán hàng hiệu quả cho cửa hàng.

Người dùng hệ thống gồm:
- **Khách hàng**: duyệt sản phẩm, đặt hoa, thanh toán, theo dõi đơn hàng.
- **Nhân viên / Quản trị**: quản lý sản phẩm, đơn hàng, khách hàng, báo cáo doanh thu.
- **Admin**: giám sát toàn bộ hệ thống, quản lý người dùng, phân quyền.

---

## 2. Yêu cầu chức năng (Functional Requirements)

### Xác thực
- Đăng nhập, đăng xuất, đăng ký tài khoản.
- Quên mật khẩu, đổi mật khẩu.
- Phân quyền theo vai trò (Khách hàng, Nhân viên, Admin).

### Khách hàng
- Duyệt danh mục sản phẩm.
- Tìm kiếm hoa theo tên, loại, giá.
- Đặt hàng, thanh toán (COD, ví điện tử, thẻ).
- Theo dõi trạng thái đơn hàng.
- Xem lịch sử mua hàng.

### Nhân viên
- Quản lý sản phẩm (thêm, sửa, xóa, cập nhật tồn kho).
- Quản lý đơn hàng (xác nhận, giao hàng, hủy).
- Quản lý khách hàng cơ bản (xem thông tin, hỗ trợ).
- Thống kê đơn hàng theo ngày/tuần/tháng.

### Admin
- Quản lý tài khoản nhân viên và khách hàng.
- Quản lý danh mục sản phẩm, loại hoa.
- Quản lý khuyến mãi, mã giảm giá.
- Giám sát hệ thống và xuất báo cáo doanh thu.

---

## 3. Yêu cầu phi chức năng (Non-functional Requirements)

- **Hiệu năng**: Hỗ trợ ≥ 200 người dùng đồng thời.  
- **Bảo mật**:
  - Mật khẩu mã hóa an toàn.
  - Chống tấn công SQL Injection, XSS.  
- **Khả dụng**: Uptime ≥ 99%.  
- **Khả năng mở rộng**: Tích hợp thêm thanh toán online, dịch vụ giao nhanh, app mobile.  
- **Thân thiện**: Giao diện responsive, tối ưu cho PC và điện thoại.  

---

## 4. Ràng buộc nghiệp vụ (Business Rules)

- Mỗi khách hàng chỉ có 1 tài khoản duy nhất.  
- Sản phẩm phải còn hàng mới cho phép đặt.  
- Đơn hàng chỉ được hủy trước khi nhân viên xác nhận giao.  
- Nhân viên không được xóa dữ liệu gốc (loại hoa, khách hàng).  
- Khi khách hàng bị xóa, toàn bộ đơn hàng liên quan vẫn được lưu trong báo cáo.  

---

## 5. Công nghệ đề xuất
- **Frontend**: HTML5, CSS3, JavaScript, Bootstrap 5.  
- **Backend**: ASP.NET MVC / Java Spring / Node.js (tùy chọn).  
- **Database**: SQL Server / MySQL.  
- **Triển khai**: IIS / Apache / Cloud (AWS, Azure).  

---

## 6. Hướng phát triển tương lai
- Ứng dụng mobile (Android/iOS).  
- Chatbot hỗ trợ đặt hoa nhanh.  
- Tích hợp trí tuệ nhân tạo gợi ý hoa theo dịp lễ, sở thích.  

---

📌 *Đây là tài liệu README mô tả yêu cầu ban đầu cho hệ thống bán hoa FlowerKingdom. Chức năng có thể được mở rộng tùy theo nhu cầu kinh doanh.*  
