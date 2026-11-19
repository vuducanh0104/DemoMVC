NET_CORE_API

## Quản lý sinh viên

Sinh viên liên kết với khoa qua `DepartmentCode` (khóa ngoại); không thể xóa khoa
đang có sinh viên. Danh mục 12 khoa của Trường Đại học Mỏ - Địa chất được thêm
bằng migration. Có thể xem, thêm, sửa và xóa khoa tại mục **Khoa**.
Các thao tác thêm, sửa, xóa sinh viên và khoa được thực hiện trong modal bằng
AJAX; dữ liệu được kiểm tra và thông báo lỗi hiển thị ngay trong modal.

Mã sinh viên được tạo tự động theo cấu trúc `YY + mã hệ đào tạo + mã ngành + số
thứ tự 4 chữ số`. Ví dụ năm nhập học 2028, hệ `21`, ngành `05` tạo mã đầu tiên
`2821050001`; số thứ tự tăng độc lập theo năm nhập học, hệ đào tạo và ngành, vì
vậy sau mã `2821050199` sẽ là `2821050200`.

Áp dụng migration trước khi chạy ứng dụng:

```powershell
dotnet ef database update
dotnet run
```

Mở mục **Sinh viên** hoặc **Khoa** trên thanh điều hướng để quản lý dữ liệu.
Migration `RemovePersonData` xóa bảng Person cũ khỏi cơ sở dữ liệu.
