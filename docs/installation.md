# Installation Instructions for the Evaluation Tracker Application

## Database

> **Note**: At this time, the application only supports Microsoft SQL Server.

The application requires several database tables for storing
application-specific data. These tables can be stored in any SQL Server instance
on any machine that can be accessed by the web application backend. For example,
they could be installed on the same ODS database used by the Ed-Fi ODS/API, or
on another database entirely.

The scripts files are stored in the [/sql directory](../sql). Management of the
scripts can be automated with the help of
[deploy-sql.ps1](../eng/deploy-sql.ps1).

## AppSettings

* `Authentication`
  * `Audience`: default "epp-eta".
  * `IssuerUrl` and `Authority`: this web service's base URL, same value for both.
  * `NewUsersAreAdministrators`: when `true`, then any new user signup will be
    created as an Administrator. When `false`, will be created as a
    Mentor-Teacher.
  * `SigningKey`: random 32 characters, base-64 encoded.
  * `RequireHttps`: when `true`, tokens will only be created for HTTPS requests.
* `CorsAllowedOrigins`: comma-separated list of allowed origins (base URL for
  the web application, as seen in the browser).
