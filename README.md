# AI Report Rewriter

## 📌 Overview

AI Report Rewriter is a backend system built using Clean Architecture that rewrites and summarizes financial reports using AI.

The system is designed to be scalable, maintainable, and production-ready.

---

## 🚀 Features

* Rewrite financial reports using AI
* Generate summaries automatically
* Store reports in the database
* Fetch reports with pagination and filtering
* Secure APIs with API Key
* Rate limiting implemented
* Caching for performance optimization
* Global exception handling
* Retry logic for external API calls

---

## 🏗️ Architecture

This project follows Clean Architecture:

* API Layer → Controllers
* Application Layer → Business logic, DTOs, Interfaces
* Domain Layer → Core entities
* Infrastructure Layer → Database, AI integration

---

## 🧠 Tech Stack

* .NET 8 / .NET 9
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server
* Hugging Face API

---

## 🔑 API Endpoints

### POST /api/reports/rewrite

Rewrite and summarize a report

### GET /api/reports

Get all reports with pagination

### GET /api/reports/{id}

Get report by ID

---

## 🔒 Security

* API Key authentication
* Rate limiting applied

---

## ⚡ How to Run

```bash
dotnet build
dotnet run
```

---

## 📊 Key Highlights

* Clean Architecture implementation
* Dependency Injection
* Separation of concerns
* Scalable and maintainable design
* Real-world backend practices

---

## 🎯 Future Improvements

* Add authentication (JWT)
* Add unit & integration tests
* Deploy to cloud (Azure/AWS)
* Add advanced logging (ELK / Monitoring)
