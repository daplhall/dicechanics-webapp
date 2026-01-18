FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /App

COPY . ./
RUN dotnet restore && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0

RUN apt-get update -y && apt-get install curl -y
ADD https://astral.sh/uv/install.sh /uv-installer.sh
RUN sh /uv-installer.sh && rm /uv-installer.sh
ENV PATH="/root/.local/bin/:$PATH"

WORKDIR /App
COPY --from=build /App/out /App/StartWithPythonNet.sh .
RUN chmod +x ./StartWithPythonNet.sh && uv venv .venv --python 3.13

ENTRYPOINT ["sh", "StartWithPythonNet.sh", ".venv", "PythonSerivce.dll"]
 