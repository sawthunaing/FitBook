# FitBook
FitBook: Class Booking &amp; Fitness Management System

![FitBook_Sequence drawio](https://github.com/sawthunaing/FitBook/assets/15320315/bcc1ec5f-d976-4f06-ae2c-1924d9475aaa)


Features

- User CURD
- Get Packages and Purchase Packages
- Book Class (Yoga, Zumba, etc...)
- Cancel Booking Class
- Auto Refund using HangFire and HangFire dashboard (https://localhost:7235/hangfire)
- Handle Concurrent users using Cache (Redis)
- Check Heathz API ((https://localhost:7235/healthz)
- Customer API Gatway using Ocetlot
- Support Swagger UI ((https://localhost:7235/swagger/index.html)

Before you run the project
- Find Db schema and run in mysql in doc directory
- Install Dot Net 6 sdk and Visual Studio 2022
- Install Redis server
- Need to run both Stn.FitBook.Api project and Stn.Fitbook.ApiGatway project
- Import Postman Collection
