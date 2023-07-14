# Swagger Integration

When `"EnableSwagger": true` in the .NET application's `appSettings` file,
SwaggerUI [will be available](https://localhost:7065/swagger) for API testing.

Anyone can POST a new Account in Swagger to register for the first time. If
`Authentication.NewUsersAreAdministrators: true` in `appSettings`, then the new
user will be an Administrator. Otherwise the user will be a Mentor-Teacher.

Swagger is typically used with client credentials, whereas the EPPETA API
expects password authentication. We did not figure out how to configure
Swashbuckle to enable password authentication in Swagger _without_ a client ID,
therefore Swagger's Authorize button simply lets you paste in a token.

You must acquire the token by making an token request in another application.
For example, you can run the following command in PowerShell to get a token.

```pwsh
curl --location 'https://localhost:7065/connect/token' `
    --header 'accept: */*' `
    --header 'Content-Type: application/x-www-form-urlencoded' `
    --data-urlencode 'grant_type=password' `
    --data-urlencode 'password=string9(I' `
    --data-urlencode 'username=user9@example.com'
```

This command generates a response like this:

```
{
  "access_token": "eyJhbGciOiJSU0EtT0FFUCIsImVuYyI6IkEyNTZDQkMtSFM1MTIiLCJraWQiOiJEOTZEOTRENUQyQjczNTI4NzA2QzQ1MTQ0OTlBQUNERTU1Nzk2QjZDIiwidHlwIjoiYXQrand0IiwiY3R5IjoiSldUIn0.FPVp8wpkDlvuhUSw1VKck9Aytk6KTd_qaa8RJlto6bFEIoATD1fZXxpgHttkW_8j8tsTMzlgQ8yM8lc3DV_oSnn1eZE34Mv8VmY_K-7ZyHRE0KxtdG9cxe4Tp6L_M-n0oiaBi_Jfv6clyRHXlfaFUib9wmtUGak042RA9roaAEnBd8YnmCCGpuNSLBr83gRtrZnm6nlCu4uYzWyYhqnTNiBR3UCyxU1wiNys9S1c45fj5IJ1RPnSh-LlF0MHD-RoxhmFkd6Io6oOtle8NGS34tGQeUieVVsrnIGNvTuJa2CGveX_Pu69cHkkIRsFD1yVjCUcXrsgprHgsxmnkXHcJQ.YJ1RTGu9TPZ1FxdHQdoZqQ.m14p8pwyTWXS9Obx3Opll619R0C-fxjQreyfU0r0gkkZiurd1-UuVhaFKQUZb9P4Pnc4hp1K-_pkiZ0gA2qKQE1b6g8eCPj5ardGILMwj0S925pcNPnAToqTv8phGPdz9IVL2NKpkLw-3EM3bHbIoc9Z2VKUPy1ozegMHW5SyUiDHQsy8MEoYdmIkKsEvB3L_LyzEgWvcvydb0BQEkDvirBA52hECI2y_fjdMvFEXThRtTKP8TiRSqGmxpdGwAD5-QQT7UanyiCphr8igN7zUau-V11i9S5T2rxVlLi1FkZXOo8pYOy6ZXSBYzfSoryB1PNIhcWaXHOn3Zd4maYe4EeyFDnTqVpTRTH28yZ5Sn4qInVkrgB6LzNVKT7wA4Od9-W-lIjFWsyqqmoeDz0X1rnUAyvdcxQsTwSZxCmTSbubpsH4QvguX4nl3qLj5e6Q9zv1xBK7w6-yzyX54_aHwakNtwHWilj0-htZt6rNFIGgY5-aAzpDDzuhLLhctQVRE4L-J7fMIUu1pGPLWqSl5BcW3hFGHJJTSWKQwEI1Zqddn9Z_cqlgN-wo3-zdsOzUdIufgFldrXKPuK6x0y_C9ZIl-kaVBE_UcLhyDDlQ7mvFnM-Kc0d1c3AbipvE9v-c.UatHS4Z-rl46a01AyJAOHx6vu7SDobarifF84tkOHPw",
  "token_type": "Bearer",
  "expires_in": 3599
}
```

Copy the `access_token` value and paste it into the Authorize form in Swagger.
Once you have done this, you will be able to use any of the endpoints that
require authorization.
