# TicketGo – Hệ thống đặt vé xe

## 1. Yêu cầu hệ thống

Trước khi bắt đầu, bạn cần cài đặt các công cụ sau:

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download)
- [Visual Studio Code](https://code.visualstudio.com/) + [C# Extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp)
- [Git](https://git-scm.com/downloads)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

---

## 2. Clone dự án

```bash
git clone https://github.com/Van1911/DO_AN.git
cd TicketGo
```
---

## 3. Cấu hình chuỗi kết nối
Mở file appsettings.json hoặc appsettings.Development.json trong thư mục TicketGo.Web và cập nhật chuỗi kết nối:

```bash
"ConnectionStrings": 
    {
    "DefaultConnection": "Data Source={YOURSERVERNAME};Initial Catalog=TicketGoV2;Integrated Security=True;Encrypt=False"
    }
```
Thay {YOURSERVERNAME} bằng tên server SQL Server của bạn (ví dụ: localhost, .\SQLEXPRESS, v.v.).


---

## 4. Khôi phục các gói NuGet
Chạy lệnh sau ở thư mục gốc dự án:
```bash
dotnet restore
```

---

## 5. Cấu hình cơ sở dữ liệu
➤ Trường hợp đã có migration:

```bash
dotnet ef database update
```

➤ Trường hợp chưa có migration hoặc muốn tạo mới:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

📌 Đảm bảo EF Core CLI đã được cài đặt:

```bash
dotnet tool install --global dotnet-ef
```

## 6. Chạy ứng dụng
Di chuyển vào thư mục web:

```bash
cd TicketGo.Web
dotnet run
```
Hoặc nhấn F5 trong Visual Studio Code để chạy ứng dụng với debugger.

## 📂 Cấu trúc chính của dự án
TicketGo/
│
├── TicketGo.Application      # Lớp ứng dụng - chứa logic nghiệp vụ
├── TicketGo.Domain           # Lớp domain - các thực thể và interface
├── TicketGo.Infrastructure   # Kết nối DB, repo, cấu hình DI
└── TicketGo.Web              # Giao diện người dùng (ASP.NET Core Razor Pages)

