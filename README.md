# TripClientApp

A simple web API for managing clients and their trips.

## Project Structure

The project is a .NET Core Web API application. The main components are:

-   `Controllers/`: Handles incoming HTTP requests.
    -   `ClientsController.cs`: Manages client-related operations.
    -   `TripsController.cs`: Manages trip-related operations.
-   `DTOs/`: Data Transfer Objects used for request and response models.
-   `Exceptions/`: Custom exception classes.
-   `Models/`: Entity Framework Core models for the database.
-   `Services/`: Business logic and database interactions.
-   `Program.cs`: The application's entry point.
-   `appsettings.json`: Configuration file.

## API Endpoints

### Clients

-   **DELETE** `/api/clients/{idClient}`
    -   Deletes a client with the specified ID.

### Trips

-   **GET** `/api/trips`
    -   Retrieves a paginated list of all trips.
    -   Query Parameters:
        -   `page` (int, optional, default: 1): The page number.
        -   `pageSize` (int, optional, default: 10): The number of items per page.
-   **POST** `/api/trips/{idTrip}/clients`
    -   Assigns a client to a trip.
    -   Request Body: `ClientTripCreateDto`
        ```json
        {
          "firstName": "string",
          "lastName": "string",
          "email": "string",
          "telephone": "string",
          "pesel": "string",
          "idTrip": 0,
          "tripName": "string",
          "paymentDate": "2024-05-27T14:55:21.583Z"
        }
        ```

## How to Run

1.  Clone the repository.
2.  Open the solution file `TripClientApp.sln` in Visual Studio or your preferred IDE.
3.  Update the connection string in `appsettings.json` to point to your SQL Server instance.
4.  Run the database migrations to create the database schema.
    ```bash
    dotnet ef database update
    ```
5.  Run the application.

## Database Schema

The database consists of the following tables:

-   **Client**: Stores client information.
-   **Trip**: Stores trip details.
-   **Client_Trip**: A many-to-many relationship between clients and trips.
-   **Country**: Stores country information.
