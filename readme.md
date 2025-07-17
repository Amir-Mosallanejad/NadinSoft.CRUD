# 🛠 NadinSoft.CRUD

**NadinSoft.CRUD** is a web API built with **ASP.NET Core 8**.  
It follows clean coding rules and is organized into clear layers (like building blocks).

The app lets users **register**, **log in**, and **manage products** — like creating, updating, and deleting them.  
It also makes sure that only the user who created a product can change or remove it.

The project is easy to test and run using **Docker** and includes tools like **Swagger** to explore the API.

## ✅ Features

- 🔄 **Clean Code Structure**  
  The code is split into clear layers: Domain, Application, Infrastructure, and API.

- 🧠 **MediatR and CQRS**  
  Commands and queries are separated to make the code easier to manage and test.

- 🔐 **User Registration & Login**  
  Uses ASP.NET Identity to create accounts and log in securely.

- 🔑 **JWT Authentication**  
  After login, users receive a token to access secure endpoints.

- 🧾 **Product Management**  
  Users can create, update, delete, and list products — only the owner can modify or delete.

- ✅ **Validation with FluentValidation**  
  All requests are checked to make sure data is valid before saving.

- 🔁 **AutoMapper**  
  Converts between database models and the data sent over the API.

- 🧪 **Testing Support**  
  Includes both unit tests and acceptance tests using Gherkin (Given-When-Then style).

- 🐳 **Runs in Docker**  
  Easily run the app and database using Docker Compose.

- 📘 **Swagger UI**  
  Visual interface to test the API, including support for JWT tokens.

## 🚀 How to Run the Project

# You can run this project in two ways:

# 🐳 Option 1: Using Docker Compose

This is the easiest way. Docker will run both the API and the database for you.
✅ Requirements

Make sure you have Docker and Docker Compose installed.
📦 Docker Images Used

    mcr.microsoft.com/mssql/server:2017-latest – SQL Server

    mcr.microsoft.com/dotnet/aspnet:8.0 – .NET runtime

    mcr.microsoft.com/dotnet/sdk:8.0 – .NET build SDK

🛠 How to Run

    Open a terminal in the root of the project.

    Run:

    docker compose --env-file .env up --build

    Once running:

        API will be available at "http://localhost:8080/swagger/index.html"

        SQL Server will run on port 1433

    The .env file already contains the database connection string used inside Docker.

# 💻 Option 2: Run with Your IDE (Visual Studio, Rider, etc.)

You can also run the API directly from your IDE if you already have SQL Server installed on your machine.

🛠 Steps

    Go to the API project folder (NadinSoft.CRUD.API).
    src >> NadinSoft.CRUD.API 
    look for appsettings.json
    in appsettings.json find "LoadEnv" key and change the value to "True"
    (it makes the project to read the enviromets from .env file that located in NadinSoft.CRUD.API folder)

in **.env** file that located in **NadinSoft.CRUD.API** folder set your **MS Sql Server** configs

```
DB_SERVER=127.0.0.1
DB_NAME=NadinSoft_Db
DB_USER=
DB_PASSWORD=
DB_ENCRYPT=False
DB_TRUST_CERT=True
```

Now you can run the API project using your IDE.

## 🧪 Testing

The solution supports both unit tests and BDD (SpecFlow) tests.

**✅ Run Unit Tests**

```
dotnet test tests/NadinSoft.CRUD.UnitTest
```

**✅ Run Acceptance Test (BDD) Tests**

!! **It might take minutest for the first time because it needs to build the Docker Image and restore packages** !!

```
dotnet test tests/NadinSoft.CRUD.AcceptanceTest
```