## About The Project ⚡
This project is a full stack app with ASP.NET(server side) and Blazor(client side) which uses the Onion Architecture.

## Features
- Common
  - Authentication & Authorization
  - JWT Authentication with refresh token
  - Dependency Injection
  - Pagination
  - Code sharing
- Back-end
  - Mapping with [AutoMapper](https://github.com/AutoMapper/AutoMapper)
  - Swagger tools for documenting API's ([Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)) 
  - Image processing with [ImageSharp](https://github.com/SixLabors/ImageSharp)
- Front-end
  -  UI components using [AntDesign](https://github.com/ant-design-blazor/ant-design-blazor)
  - Progressive Web Applications (PWA)
  - Panel admin
  - Intercept outgoing HTTP requests for refresh token [HttpClientInterceptor](https://github.com/jsakamoto/Toolbelt.Blazor.HttpClientInterceptor)
  - Browsers local storage APIs with [LocalStorage](https://github.com/Blazored/LocalStorage)
  
  ## Getting Started
  - Install [.NET 5 SDK](https://dotnet.microsoft.com/download/dotnet/5.0)
  - Install Visual Stadio 2019 (and above) or VS Code
  - Run `CleanBlog.Api`
  - For see api documentions run `{your-localhost}/swagger/index.html`
