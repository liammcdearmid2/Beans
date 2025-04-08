Beans API - Technical Test

Overview
This API is a simple implementation for managing beans, including CRUD operations and functionality to select a "Bean of the Day." The API is built using ASP.NET Core with a MySQL database hosted on AWS RDS.

Prerequisites
Before testing the API, you will need:

Clone the repository from GitHub:
git clone https://github.com/liammcdearmid2/Beans

AWS RDS MySQL Database:
The API connects to an AWS RDS MySQL database. 

The connection string is included in the appsettings.json file but needs amending.

As this is held in a public repository, I have provided the credentials to connect to the database in the separate cover note (see Section 3).

Swagger UI:
Swagger UI is enabled for testing and interacting with the API. Once the API is running, you can access Swagger at http://localhost:5000/swagger.

Setup and Running the API Locally
After cloning the repository, run the application using:
dotnet run

The API will start running locally on http://localhost:5000 by default.

Verify the API is Running:
Open your browser and navigate to http://localhost:5000/swagger to access the Swagger UI.

From the Swagger UI, you can test the available API endpoints.

Testing the API
Once the application is running locally, you can test the following endpoints using Swagger UI or a tool like Postman:

GET /api/beans - Retrieves all beans.

GET /api/beans/{id} - Retrieves a specific bean by ID.

POST /api/beans - Adds a new bean.

PATCH /api/beans/{id} - Updates a bean by ID.

DELETE /api/beans/{id} - Deletes a bean by ID.

POST /api/beans/pick-botd - Selects the "Bean of the Day."

Sample Requests:
Get all beans:
Method: GET
URL: /api/allBeans

Get bean by ID:
Method: GET
URL: /api/beans/{id}

Add a new bean:
Method: POST
URL: /api/beans
Body: { "_id": "123", "Name": "Espresso", "Cost": 3.5 }

Troubleshooting:
If you encounter any errors when building or running the project after cloning the repository, try the following steps:

-Restore NuGet Packages
If you see errors about missing references or packages:
Right-click on the solution in the Solution Explorer and select "Restore NuGet Packages". Alternatively, open a terminal and run: dotnet restore.

-Ensure .NET SDK is Installed
Make sure you have the correct version of the .NET SDK installed. This project was built using .NET 8.0.
You can check your installed SDK version by running: dotnet --version

-Confirm that the connection string in appsettings.json is correct.
This is located in the cover note document.

-Try Cleaning and Rebuilding the Solution
Go to Build > Clean Solution, then Build > Rebuild Solution.

-Run as Administrator
Some permissions issues can be solved by running Visual Studio as administrator.


