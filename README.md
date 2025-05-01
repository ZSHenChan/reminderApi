# Reminder API

The Reminder API is a .NET Core 8-based application designed to manage reminders. It uses PostgreSQL as the primary database, Redis for caching, and JWT for secure token issuance. The application also incorporates filters and middleware to handle malformed or invalid data effectively.

## Features

- **Reminder Management**: Create, retrieve, update, and delete reminders.
- **PostgreSQL Integration**: Uses Entity Framework Core for database operations.
- **Redis Caching**: Improves performance by caching frequently accessed data.
- **JWT Authentication**: Issues and validates JSON Web Tokens for secure API access.
- **Data Validation**: Middleware and filters ensure robust handling of malformed or invalid data.

## Technologies Used

- **.NET Core 8**
- **PostgreSQL** with Entity Framework Core
- **Redis** for caching
- **JWT** for authentication
- **Middleware and Filters** for data validation

## Prerequisites

- [.NET Core SDK 8](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Redis](https://redis.io/download)

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/ZSHenChan/reminderApi
cd reminderApi-sln/reminderApi
```

### 2. Configure the Environment

Create a `.env` file in the root directory and add the following environment variables:

```env
POSTGRES_CONNECTION_STRING=Host=<host>;Database=<database>;Username=<username>;Password=<password>
REDIS_CONNECTION_STRING=<redis-connection-string>
JWT_SECRET=<your-jwt-secret>
```

### 3. Apply Database Migrations

Run the following commands to apply migrations to the PostgreSQL database:

```bash
dotnet ef database update
```

### 4. Run the Application

Start the application using the .NET CLI:

```bash
dotnet run
```

The API will be available at `http://localhost:5000`.

## API Endpoints

- **POST /api/reminders**: Create a new reminder.
- **GET /api/reminders**: Retrieve all reminders.
- **GET /api/reminders/{id}**: Retrieve a specific reminder by ID.
- **PUT /api/reminders/{id}**: Update a specific reminder.
- **DELETE /api/reminders/{id}**: Delete a specific reminder.

## Error Handling

The application uses middleware and filters to handle:

- **Malformed Data**: Ensures the request body matches the expected structure.
- **Invalid Data**: Validates the data against business rules.

## Contributing

Contributions are welcome! Please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
