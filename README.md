# SummaProject - Part 1: User Management

## Overview

This part of the project comprises an ASP.NET Core backend for managing user data and a frontend implemented using both Vue.js and jQuery. The backend handles user CRUD operations and email functionalities, while the frontend provides a user interface for interacting with the backend.

### Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Running the Project](#running-the-project)
  - [Backend](#backend)
  - [Vue.js Frontend](#vuejs-frontend)
  - [jQuery Frontend](#jquery-frontend)
- [API Endpoints](#api-endpoints)
- [Project Structure](#project-structure)
- [Environment Variables](#environment-variables)

## Prerequisites

- .NET Core SDK
- Visual Studio or any preferred IDE
- Node.js and npm

## Installation

1. **Clone the repository**

   ```sh
   git clone https://github.com/nivdoron1/SummaProject.git
   cd invoice-processing
   ```

2. **Install required packages**

   For the .NET backend:

   ```sh
   dotnet restore
   ```

   For the Vue.js frontend:

   ```sh
   cd vue-frontend
   npm install
   ```

4. **Build the projects**

   For the .NET backend:

   ```sh
   dotnet build
   ```

   For the Vue.js frontend:

   ```sh
   cd vue-frontend
   npm run build
   ```

## Running the Project

### Backend

1. **Run the .NET backend**

   ```sh
   dotnet run --project SummaProject1Vue
   ```

### Vue.js Frontend

1. **Navigate to the Vue.js project directory**

   ```sh
   cd vue-frontend
   ```

2. **Start the Vue.js development server**

   ```sh
   npm run serve
   ```

### jQuery Frontend

1. **Open the `index.html` file in a web browser**

   Simply open the `index.html` file located in the root directory of the jQuery frontend project.

## API Endpoints

### UsersController

- **Create or Update User**

  ```http
  POST /users/createOrUpdateUser
  ```

  **Parameters**:
  - `Username`: The username of the user.
  - `Email`: The email of the user.
  - `BirthDate`: The birth date of the user.
  - `photo`: The photo of the user (optional).

- **Get User by ID**

  ```http
  GET /users/{id}
  ```

  **Parameters**:
  - `id`: The ID of the user.

- **Get User by Email**

  ```http
  GET /users/email/{email}
  ```

  **Parameters**:
  - `email`: The email of the user.

- **Delete User by ID**

  ```http
  DELETE /users/{id}
  ```

  **Parameters**:
  - `id`: The ID of the user to delete.

- **Delete User by Email**

  ```http
  DELETE /users/email/{email}
  ```

  **Parameters**:
  - `email`: The email of the user to delete.

- **Get All Users**

  ```http
  GET /users/allUsers
  ```

## Project Structure

### Backend

- **Controllers**
  - `UsersController.cs`: Manages user CRUD operations.

- **Models**
  - `User.cs`: Represents the user entity.

### Frontend

#### Vue.js

- **Components**
  - `ProfileForm.vue`: Form for creating and updating users.
  - `UserList.vue`: Displays a list of users and their details.

- **Services**
  - `userService.js`: Handles API calls for user operations.

#### jQuery

- **HTML**
  - `index.html`: Main entry point for the jQuery frontend.

- **JavaScript**
  - `index.js`: Contains the jQuery code for managing user forms and interactions.

- **CSS**
  - `style.css`: Styles for the jQuery frontend.

# SummaProject - Part 2: Invoice Processing and Email Sending

## Overview

This part of the project processes invoices from PDF files and sends the data via email using the SendGrid service. It includes:

- Converting PDF content to JSON format
- Sending the JSON data via email
- A command-line application for testing the functionality
- An API controller to trigger the invoice processing and email sending

### Table of Contents

- [Overview](#overview)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Running the Project](#running-the-project)
  - [Command-Line Application](#command-line-application)
  - [ASP.NET Core Application](#aspnet-core-application)
- [API Endpoints](#api-endpoints)
- [Code Structure](#code-structure)
- [Environment Variables](#environment-variables)
- [Contributing](#contributing)
- [License](#license)

## Prerequisites

- .NET Core SDK
- Visual Studio or any preferred IDE
- SendGrid API Key

## Installation
Exactly like the previous project

3. **Set Environment Variables**

   To set the required environment variables for SendGrid, a batch file (`setenv.bat`) is provided. Run this file to automatically set the environment variables:

   ```sh
   .\setenv.bat
   ```

   The `setenv.bat` file contains the following commands to set the SendGrid environment variables:

   ```bat
   @echo off
   setx SENDGRID_API_KEY "your_sendgrid_api_key"
   echo SENDGRID_API_KEY has been set to:
   echo %SENDGRID_API_KEY%
   setx SENDGRID_EMAIL "your_email@example.com"
   echo SENDGRID_EMAIL has been set to:
   echo %SENDGRID_EMAIL%
   setx SENDGRID_USERNAME "Your Name"
   echo %SENDGRID_USERNAME%
   pause
   ```

4. **Build the project**

   ```sh
   dotnet build
   ```

## Running the Project

### Command-Line Application

1. **Navigate to the project directory**

   ```sh
   cd path\to\your\project
   ```

2. **Run the appli
cation**

   ```sh
   dotnet run
   ```

   When prompted, enter your email address. The application will read the PDF file, convert its content to JSON, and send it to the specified email address.

### ASP.NET Core Application

1. **Run the web application**

   ```sh
   dotnet run --project path\to\your\web\project
   ```

2. **API Endpoints**

   - **Send Email**

     Trigger the invoice processing and email sending:

     ```sh
     GET /Invoice/sendemail/{ToEmail}
     ```

     Replace `{ToEmail}` with the recipient's email address.

## Code Structure

- **Invoices/Invoice.cs**: Defines the `Invoice` class, which represents the invoice data.
- **Invoices/Product.cs**: Defines the `Product` class, which represents individual products in the invoice.
- **SummaProject1Vue/Controllers/SendEmail.cs**: Contains the `SendEmail` class for sending emails using SendGrid.
- **SummaProject1Vue/Controllers/InvoiceController.cs**: Provides API endpoints to trigger invoice processing and email sending.
- **Program.cs**: Entry point for the command-line application that processes a PDF file and sends the data via email.
- **InvoiceFetching.cs**: Contains the `InvoiceFetching` class that processes the PDF file and extracts the invoice data.

## Environment Variables

Make sure to set the following environment variables:

- `SENDGRID_API_KEY`: Your SendGrid API key.
- `SENDGRID_EMAIL`: The email address from which the emails will be sent.
- `SENDGRID_USERNAME`: The username to be displayed in the email.

These can be set manually or by running the provided batch file (`setenv.bat`).

## Additional Instructions
To call the API functions, refer to the SummaProject.http file and send the request as needed.


