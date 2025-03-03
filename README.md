# Airport App API

**Airport App API** is a RESTful API built with .NET, designed for managing flight reservations.

## 📌 Features

- Management of airports, flights, and reservations
- CRUD operations for main resources
- Input data validation

## 🛠️ Requirements

- .NET 5.0 or newer

## 🚀 Installation

1. **Clone the repository**

   ```bash
   git clone https://github.com/JakubTeichman/Airport_App_API.git
   cd Airport_App_API
   ```

2. **Install dependencies**

   ```bash
   dotnet restore
   ```

3. **Run the application**

   ```bash
   dotnet run
   ```

   The application will be available at `https://localhost:5001`.

## 🔗 API Endpoints

- `GET /api/airports` - Retrieves a list of airports
- `POST /api/airports` - Adds a new airport
- `GET /api/flights` - Retrieves a list of flights
- `POST /api/flights` - Adds a new flight
- `GET /api/reservations` - Retrieves a list of reservations
- `POST /api/reservations` - Creates a new reservation

## 🧪 API Testing

To test the API, it is recommended to use Swagger UI. After starting the application, navigate to:

```
https://localhost:5001/swagger
```

Swagger allows interactive testing of endpoints and provides API documentation.

