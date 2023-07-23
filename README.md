# C# Authentication via PHP API

This Authentication Client is a straightforward C# console application used to interact with an authentication API. It's designed to bypass platform and programming language restrictions, allowing users to register and login via the console.

## Prerequisites

- .NET 5.0 or higher
- MySQL Server

## Setup

1. Clone this repository.
2. In `Program.cs`, replace `"http://your-api-url"` with your API's URL.

### Database Setup

Run the following SQL command on your MySQL server to set up the necessary database and table:

```sql
CREATE DATABASE IF NOT EXISTS `user_db`;

USE `user_db`;

CREATE TABLE IF NOT EXISTS `users` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(255) NOT NULL, -- This should be a hashed value, not plaintext
  PRIMARY KEY (`id`),
  UNIQUE KEY `username` (`username`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
```

This will create a database named `user_db` and a table named `users` with columns for `id`, `username`, and `password`.

Ensure your API is configured to use this database and table.

## Dependencies

This project relies on the Newtonsoft.Json package version 13.0.3. To download it, follow these steps:

1. Right-click on your project in the Solution Explorer.
2. Select "Manage NuGet Packages" from the dropdown menu.
3. In the "Browse" tab, search for "Newtonsoft.Json".
4. Select the package and click "Install".
5. Once the installation is complete, you can use the package in your code.

Make sure to include the following line at the top of your C# file to import the Newtonsoft.Json namespace:

```csharp
using Newtonsoft.Json;
```

If you encounter any issues with the installation, please refer to the [Newtonsoft.Json NuGet page ↗](https://www.nuget.org/packages/Newtonsoft.Json/13.0.3?_src=template) for further instructions.

## Usage

Run the application and follow the prompts in the console to register or login.

## Contributing

Contributions are welcome! Please raise an issue or submit a pull request.

## License

This project is licensed under the MIT License.
