# ClaimsSystem

ClaimsSystem is a simple insurance claims management system built with **ASP.NET Core**. It allows users to submit, view, and manage insurance claims via a REST API. The system is designed with clean architecture principles, using Value Objects, repositories, and message-based integration for asynchronous processing.

Swagger UI is included for easy API testing and interaction, and the system is deployable to **Microsoft Azure**.

---

## Features

- Submit new insurance claims (with validation for phone numbers and social security numbers)
- Retrieve existing claims (sensitive data is excluded in list views)
- Asynchronous processing via Azure Service Bus
- Background worker handles claim processing and persistence
- Clean architecture using Domain, Infrastructure, API, and Worker separation
- Easily extendable for production environments

---

## Architecture

### Current Local Setup

```
Client
  ↓
ASP.NET Core API
  ↓
Azure Service Bus (Queue)
  ↓
Background Worker
  ↓
Local SQL Database
```

1. Claims are submitted via the API
2. API publishes messages to a Service Bus queue
3. The worker listens to the queue, processes claims, and stores them in a local SQL database

### Production-Ready Architecture

```
Client
  ↓
Azure API Management
  ↓
ASP.NET Core API (Azure App Service)
  ↓
Azure Service Bus (Queue)
  ↓
Azure Function (Worker)
  ↓
Azure SQL Database
```

- Uses cloud services for scalability and reliability
- Worker can be implemented as an Azure Function for serverless processing
- Data stored securely in Azure SQL Database
- API exposed and managed via Azure API Management

---

## Technologies

- .NET 8 / C#
- ASP.NET Core Web API
- Entity Framework Core (with repository pattern)
- Azure Service Bus for message-based integration
- Background Worker / Azure Function for asynchronous processing
- Swagger / OpenAPI for API documentation
- SQL Server / Azure SQL Database for storage

---

## Getting Started

**1. Clone the repository:**

```bash
git clone https://github.com/vuseo/ClaimsSystem.git
```

**2. Configure your `appsettings.json`:**

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=ClaimsDb;Trusted_Connection=True;"
  },
  "ServiceBus": {
    "ConnectionString": "<YOUR_SERVICE_BUS_CONNECTION_STRING>",
    "QueueName": "claims-queue"
  }
}
```

**3. Apply migrations:**

```bash
cd Claims.Infrastructure
dotnet ef database update
```

**4. Run the API project:**

```bash
cd Claims.Api
dotnet run
```

**5. Access Swagger UI at:**

```
https://localhost:7177/swagger
```

**6. Run the worker** to start processing messages from the Service Bus queue.

---

## Security & Compliance

- Sensitive data (phone numbers, social security numbers) is stored and transmitted securely
- Value Objects enforce format and integrity rules for critical fields
- In production, ensure encryption at rest and in transit, and use **Azure Key Vault** for secrets

---

## Future Improvements

- Implement transactional outbox pattern for guaranteed message delivery
- Add retry policies and dead-letter queues for failed message processing
- Extend API with authentication/authorization
- Deploy the worker as a serverless Azure Function for cost-efficient scaling
