# Installation Instructions for the Evaluation Tracker Application

## Database

> **Note**: At this time, the application only supports Microsoft SQL Server.

The application requires several database tables for storing
application-specific data. These tables can be stored in any SQL Server instance
on any machine that can be accessed by the web application backend. For example,
they could be installed on the same ODS database used by the Ed-Fi ODS/API, or
on another database entirely.

The scripts files are stored in the [/sql directory](../sql). Management of the
scripts can be automated with the help of [deploy-sql.ps1](../eng/deploy-sql.ps1). Executing the script will deploy all SQL scripts to an empty database named `eppeta` and assumes localhost and Windows Authentication.

## ODS/API 
The Evaluation Tracker reads evaluation metadata and candidate data from a connected ODS/API and will also send evaluation rating data to the same ODS/API. The Evaluation Tracker has been tested with ODS/API v6.x and 7.x (ODS/API v5.3 should work as well, but has not been tested).

We used the populated template that comes with a default install of the ODS/API (which includes TPDM-Core). The populated template does not contain valid evaluation metadata, so we added evaluation metadata based on the T-TESS. Sample T-TESS evaluation metadata can be found in [T-TESS Samples](./SampleEvaluationData/readme.md)

> **Note**: The candidates in the populated template do not contain person references and person references are required for PerformanceEvaluationRatings, so if you use the populated template, you will need to update the candidate data or the candidates will not be synced to the Evaluation Tracker

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

* `Serilog` Logging Settings for the Application
  * `MinimumLevel` 
    * `Default` - The lowest log level to be sent to the log if no overrides specified
    * `Override` - Overrides to the minimum level based specific namespaces (or parts of a namespace)
  * `WriteTo` - Determines locations logs are written to most common options are `File` and `Console`
  
* `ConnectionStrings`
  * `DefaultConnection` Standard SQL Server Connection String
* `SyncOdsAssetsSettings`
  * `PeriodInHours` how often the evaluation tracker will sync resources (Candidate and Evaluation Metadata) defaults to 24 hours
* `OdsApiBasePath` the URL to the ODS/API to sync data from and send data to
* `ODSAPIKey` The key of the ODS/API API Client
* `ODSAPISecret` The secret of ODS/API API Client
* `TrustAllSSLCerts` Set to true if using self signed certificates in development, otherwise should be false
* `AuthenticationTokenTimeout` Length of time in minutes that a logged in users authentication token will be valid before attempting to refresh or require a new login
* `AuthenticationRefreshTokenLifeTime` Length of time in days that the application will attempt to refresh the authentication token before requiring the user to login again
* `ResetPasswordUrl` Reset Password return url
* `RefreshTokenLifetimeMinutes` How long in minutes the password reset token will be valid
* `MailSettings` SMTP settings for password reset
  * `Host` Host that will send mail
  * `Port` Port to send mail on
  * `UserName` Username for authenticating with the email server
  * `Password` Password for authenticating with the email server
  * `From` Email address that will appear in the from of the email
  * `DeliveryMethod` Defaults to Network for using SMTP server
  * `EnableSsl` Enabled if the SMTP server user SSL

