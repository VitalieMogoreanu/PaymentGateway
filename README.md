# PaymentGateway

The solution was created using Visual Studio Community 2019 for Mac targetting .Net 5.0. It is expected that the solution file and project files are compatible with an equivalent Visual Studio for Windows version

## Projects
- [PaymentGatewayAPI](PaymentGatewayAPI) - ASP.NET Core Web API project that includes PaymentGateway RESTful HTTP service implementation, mock bank and memory-based payment details store
- [PaymentGatewayTests](PaymentGatewayTests) - xUnit based tests of the service

## Running
To run PaymentGatewayAPI you can use Visual Studio 'Run Project' functionality that would open Swagger UI in a new browser tab/window. Swagger UI would provide both service documentation and a client to make service calls
