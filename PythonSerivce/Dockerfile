FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish -o out

# Building
FROM mcr.microsoft.com/dotnet/aspnet:10.0
RUN apt-get update -y && apt-get install python3 -y && apt-get install python3-dev -y
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "PythonSerivce.dll"]
