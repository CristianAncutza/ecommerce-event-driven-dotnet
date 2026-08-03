# E-Commerce Event-Driven Architecture (.NET 9)

An enterprise-grade, event-driven microservices-inspired e-commerce backend built with **.NET 9**, implementing **CQRS**, **Domain-Driven Design (DDD)**, and **Event Streaming via Apache Kafka**.

---

## Architecture & Tech Stack

* **Platform:** .NET 9 (C#)
* **Architecture Pattern:** Event-Driven Architecture (EDA) & CQRS (Command Query Responsibility Segregation)
* **API Framework:** ASP.NET Core Minimal APIs & Controllers
* **Database & ORM:** PostgreSQL, Entity Framework Core 9 (Npgsql)
* **Messaging & Event Streaming:** Apache Kafka
* **Caching & Resilience:** Redis (StackExchange.Redis) & Custom Rate Limiting Middleware
* **API Documentation:** Swagger / OpenAPI

---

## Project Structure

The solution follows clean architecture principles divided into distinct layers:

* **`Order.API`**: Entry point exposing HTTP endpoints, custom middlewares, and API configurations.
* **`Order.Application`**: Core business logic, application features (Commands/Queries), and **MediatR** handlers.
* **`Order.Domain`**: Enterprise entities, business rules, aggregates, and repository interfaces.
* **`Order.Infrastructure`**: Database persistence (`OrderDbContext`), Entity Framework configurations, Kafka event producers, and Redis caching implementations.

---

## Prerequisites

Make sure you have the following installed on your machine:
* [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for running PostgreSQL, Kafka, and Redis)

---

## Getting Started

### 1. Clone the Repository
```bash
git clone [https://github.com/your-username/ecommerce-event-driven-dotnet.git](https://github.com/your-username/ecommerce-event-driven-dotnet.git)
cd ecommerce-event-driven-dotnet
