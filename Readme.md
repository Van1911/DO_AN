## 1. Cài đặt các công cụ cần thiết
.NET SDK 8
IDE: Visual Studio Code + C# extension
Git
SQL Server

## 2. Clone project về
https://github.com/Van1911/DO_AN.git

## 3. Kiểm tra file cấu hình
Mở file appsettings.json và/hoặc appsettings.Development.json để kiểm tra:

Chuỗi kết nối database (ConnectionStrings)
    Data Source={YOURSERVERNAME};Initial Catalog=TicketGoV2;Integrated Security=True;Encrypt=False
->Sau có dán vào ConnectionStrings ở trong file appsettings.json

## 4. Khôi phục gói NuGet
dotnet restore

## 5. (Tuỳ chọn) Cấu hình DB – Migration
Kiểm tra file DbContext và thư mục Migrations
Nếu có sẵn migration:
    dotnet ef database update

Nếu chưa có migration hoặc muốn tạo mới:
    dotnet ef migrations add InitialCreate
    dotnet ef database update

## 6. Chạy project
    cd .\TicketGo.Web\
    dotnet run
 or
    F5