# 🔒 Hướng dẫn Hệ thống Kiểm tra Lỗi (Validation System)

## 📋 Tổng quan

Hệ thống validation toàn diện cho trang đăng ký và đăng nhập với kiểm tra **client-side** (JavaScript) và **server-side** (C# Controller).

---

## 🎯 Các tính năng chính

### 1. ✅ Kiểm tra Real-time (Client-side)
- Validation ngay khi người dùng nhập liệu
- Hiển thị lỗi/thành công với màu sắc rõ ràng
- Border đỏ (lỗi) / xanh (hợp lệ)
- Icon thông báo trực quan

### 2. 🛡️ Kiểm tra Server-side
- Double validation để đảm bảo an toàn
- Ngăn chặn bypass validation từ client
- TempData messages cho user feedback

### 3. 🚫 Ký tự nguy hiểm bị chặn
- XSS prevention: `< > ; " ' ` \`
- SQL Injection prevention
- Path traversal prevention: `\\`

---

## 📧 Email Validation

### ❌ **Ký tự không hợp lệ:**
```
< > ; " ' ` \
```

### ✅ **Quy tắc kiểm tra:**
- **Độ dài:** 5-100 ký tự
- **Format:** phải có @ và domain (ví dụ: user@example.com)
- **Không được có:**
  - Khoảng trắng
  - Nhiều hơn 1 ký tự @
  - Dấu chấm liên tiếp (..)
  - Ký tự đặc biệt nguy hiểm

### 📝 **Ví dụ:**
```javascript
✅ VALID:
- user@example.com
- test.user@domain.co.uk
- contact+tag@company.vn

❌ INVALID:
- user@example (thiếu domain)
- user @example.com (có space)
- user@@example.com (2 ký tự @)
- user<script>@test.com (ký tự nguy hiểm)
```

---

## 🔑 Password Validation

### ❌ **Ký tự nguy hiểm:**
```
< > ; " ' `
```

### ✅ **Quy tắc kiểm tra:**
- **Độ dài:** 6-100 ký tự
- **Không được có:** ký tự nguy hiểm (XSS/SQL Injection)
- **Password Strength Meter:**
  - ⚪ Yếu: < 3 điểm
  - 🟡 Trung bình: 3 điểm
  - 🟢 Mạnh: ≥ 4 điểm

### 📊 **Điểm mạnh mật khẩu:**
1. Độ dài ≥ 6 ký tự (+1 điểm)
2. Độ dài ≥ 10 ký tự (+1 điểm)
3. Có cả chữ hoa và chữ thường (+1 điểm)
4. Có số (+1 điểm)
5. Có ký tự đặc biệt (+1 điểm)

### 📝 **Ví dụ:**
```javascript
✅ VALID:
- Password123! (Mạnh: 5 điểm)
- mypass2024 (Trung bình: 3 điểm)
- 123456 (Yếu: 1 điểm - nhưng hợp lệ)

❌ INVALID:
- 12345 (quá ngắn < 6 ký tự)
- pass<script> (ký tự nguy hiểm)
- password" (ký tự nguy hiểm)
```

---

## 👤 Username (Họ tên) Validation

### ❌ **Ký tự không hợp lệ:**
```
< > ; " ' ` \
```

### ✅ **Quy tắc kiểm tra:**
- **Độ dài:** 2-50 ký tự
- **Không được:**
  - Chỉ toàn số (123456)
  - Ký tự đặc biệt nguy hiểm
  - Để trống

### 📝 **Ví dụ:**
```javascript
✅ VALID:
- Nguyễn Văn A
- Trần Thị B
- John Doe
- Lê Minh 2024

❌ INVALID:
- A (quá ngắn < 2 ký tự)
- 123456 (chỉ toàn số)
- Nguyễn<test> (ký tự nguy hiểm)
```

---

## 📱 Phone Validation

### ✅ **Quy tắc kiểm tra:**
- **Chỉ được chứa:** Số (0-9) và dấu + (ở đầu)
- **Độ dài:** 10-15 chữ số
- **Format hợp lệ:**
  - `0912345678` (10 số)
  - `+84912345678` (có mã quốc gia)
  - `0123456789` (11 số)

### 📝 **Ví dụ:**
```javascript
✅ VALID:
- 0912345678
- +84912345678
- 0123456789012

❌ INVALID:
- 091234567 (< 10 số)
- 091-234-5678 (có dấu -)
- phone123 (có chữ)
- ++84912345678 (nhiều dấu +)
```

---

## 🏠 Address Validation

### ❌ **Ký tự không hợp lệ:**
```
< > ; " ' `
```

### ✅ **Quy tắc kiểm tra:**
- **Độ dài:** 10-200 ký tự
- **Không được có:** ký tự đặc biệt nguy hiểm
- **Phải chi tiết:** Số nhà, đường, phường/xã, quận/huyện, tỉnh/thành

### 📝 **Ví dụ:**
```javascript
✅ VALID:
- 123 Đường Lê Lợi, Phường Bến Thành, Quận 1, TP.HCM
- Số 45 Trần Hưng Đạo, Hoàn Kiếm, Hà Nội

❌ INVALID:
- TP.HCM (quá ngắn < 10 ký tự)
- 123<script>alert('xss')</script> Đường ABC (ký tự nguy hiểm)
```

---

## 🎨 UI/UX Feedback

### 🔴 **Trạng thái lỗi:**
```css
Border: #f56565 (đỏ)
Icon: ❌ hoặc fas fa-exclamation-circle
Text: Màu đỏ #f56565
```

### 🟢 **Trạng thái hợp lệ:**
```css
Border: #48bb78 (xanh lá)
Icon: ✓ hoặc fas fa-check-circle
Text: Màu xanh #48bb78
```

### 🟡 **Trạng thái cảnh báo:**
```css
Border: #ed8936 (cam)
Icon: ⚠️
Text: Màu cam #ed8936
```

---

## 🚀 Cách sử dụng

### 1️⃣ **Trang Login**
```
URL: /User/Login

Validation:
- Email: Real-time khi input/blur
- Password: Real-time khi input
- Submit: Comprehensive check before sending
```

### 2️⃣ **Trang Register**
```
URL: /User/Register

Multi-step validation:
- Step 1: Email + Password + Confirm Password
- Step 2: Username + Phone + Address
- Step 3: Confirmation (review all data)

Validation:
- Real-time cho tất cả fields
- Step validation trước khi next
- Final validation trước submit
```

---

## 🛡️ Security Features

### 1. **XSS Prevention**
- Chặn ký tự: `< > " ' `
- Encoding output trong View
- AntiXsrfToken trong Form

### 2. **SQL Injection Prevention**
- Parameterized queries (Entity Framework)
- Input validation trước khi query
- Chặn ký tự: `; ' "`

### 3. **CSRF Protection**
- `@Html.AntiForgeryToken()` trong form
- `[ValidateAntiForgeryToken]` trong controller

---

## 📊 Thống kê Validation

| Trường | Client | Server | Real-time | Ký tự chặn |
|--------|--------|--------|-----------|------------|
| Email | ✅ | ✅ | ✅ | 7 ký tự |
| Password | ✅ | ✅ | ✅ | 6 ký tự |
| Username | ✅ | ✅ | ✅ | 7 ký tự |
| Phone | ✅ | ✅ | ✅ | N/A |
| Address | ✅ | ✅ | ✅ | 6 ký tự |

---

## 🔧 Troubleshooting

### ❓ **Validation không hoạt động?**
1. Kiểm tra JavaScript console có lỗi không
2. Đảm bảo jQuery đã load
3. Xem TempData["Error"] có hiển thị không

### ❓ **Form vẫn submit khi có lỗi?**
1. Kiểm tra `e.preventDefault()` trong submit handler
2. Xem validation function return `false` chưa
3. Button có bị disable sau submit không

### ❓ **Real-time validation không chạy?**
1. Kiểm tra event listeners (`input`, `blur`)
2. Xem ID của input có đúng không
3. Console log để debug

---

## 📚 Code Reference

### **Login.cshtml**
- Real-time email validation
- Real-time password validation
- Comprehensive form validation
- Error display with icons

### **Register.cshtml**
- Multi-step form với validation mỗi step
- Real-time validation cho 5 fields
- Password strength meter
- Confirmation step

### **UserController.cs**
- Server-side validation methods
- `ContainsDangerousCharacters()` helper
- `IsValidEmail()` helper
- TempData error messages

---

## ✨ Best Practices

1. **Luôn validate cả client và server**
2. **Hiển thị lỗi rõ ràng và cụ thể**
3. **Focus vào field lỗi**
4. **Disable button khi đang submit**
5. **Sử dụng icon và màu sắc trực quan**
6. **Validate real-time để UX tốt hơn**

---

## 🎉 Kết luận

Hệ thống validation này đảm bảo:
- ✅ **Bảo mật cao** - chặn XSS, SQL Injection
- ✅ **UX tốt** - real-time feedback, error messages rõ ràng
- ✅ **Toàn diện** - validate tất cả input fields
- ✅ **Reliable** - double validation (client + server)

**Chúc bạn coding vui vẻ! 🚀**
