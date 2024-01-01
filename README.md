# gspKasni
Web app that tracks how late buses in Belgrade, Serbia are.
![image](https://github.com/uuuulopta/gspKasni/assets/29780793/8fb1117a-2bd7-4086-80cf-3b4ef5f1be92)

## Development
### Backend (inside /gspApi)
This prjoect uses C# .NET and MySQL as it's database. Make sure you have those setup and then provide connection string inside `appsettings.Development.json`, afterwhich you can run `dotnet ef database update` to apply all migrations.

Run `dotent restore` to download all the dependencies and then `dotnet run` to start the development server.
### Frontend (inside Frontend/gsp)
Firstly run `npm install` to install all the dependencies.

Rename `.env.example.local` to `.env.local` and edit the port on `NEXT_PUBLIC_API_ROOT ` environment variable to match your backend port.

Run `npm run dev`, and you should see the webpage on localhost:{port}/gspKasni !

## Deployment via docker

Create an `appsettings.Production.json` and provide the connection string. If you wish to omit that file you may set the environment variable `ASPNETCORE__ConnectionStrings__Default` inside the docker container.

Inside `.env.local` configure the api root as before and change the `next.config.js` basePath if you wish.

Run `docker build -t gspapi-image -f Dockerfile .` inside the root folder to create a docker image.

Run `docker run -d -e ASPNETCORE_URLS="http://+:80;" --name gspapi gspapi-image` to create and start the docker container.

By default, the nextjs server listens on port :3000, you may change that near the end of the Dockerfile
