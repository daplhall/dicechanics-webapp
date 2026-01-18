FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /App

COPY . ./
RUN dotnet restore && dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
ARG python_version="3.13"
ENV PYTHONVERSION=$python_version
ARG venv=.venv
ENV VENV=$venv

RUN apt-get update -y && apt-get install curl -y
ADD https://astral.sh/uv/install.sh /uv-installer.sh
RUN sh /uv-installer.sh && rm /uv-installer.sh
ENV PATH="/root/.local/bin/:$PATH"

WORKDIR /App
COPY --from=build /App/out /App/entrypoint.sh .
RUN chmod +x ./entrypoint.sh && uv venv $venv --python $python_version

ENTRYPOINT ["sh", "entrypoint.sh"]
CMD ["PythonSerivce.dll"]
 