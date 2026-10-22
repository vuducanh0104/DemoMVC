NET_CORE_API

## Quản lý sinh viên

Sinh viên liên kết với khoa qua `DepartmentCode` (khóa ngoại). Danh mục khoa của
Trường Đại học Mỏ - Địa chất được thêm bằng migration. Mã sinh viên được tạo tự
động theo cấu trúc `YY + mã hệ đào tạo + mã ngành + số thứ tự 4 chữ số`; ví dụ
năm nhập học 2028, hệ `21`, ngành `05` tạo mã đầu tiên `2821050001`, sau mã
`2821050199` sẽ là `2821050200`.

Áp dụng migration trước khi chạy ứng dụng:

```powershell
dotnet ef database update
dotnet run
```

Mở mục **Sinh viên** trên thanh điều hướng để thêm, xem, sửa hoặc xóa sinh viên.
