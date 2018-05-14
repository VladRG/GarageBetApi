FROM microsoft/aspnetcore-build-nightly AS build
WORKDIR /src
COPY *.sln ./
COPY GarageBet.Api/*.csproj GarageBet.Api/

RUN dotnet restore
COPY . .
WORKDIR GarageBet.Api
RUN dotnet build -c Release -o /app

FROM build as publish
RUN dotnet publish -c Release -o /app

FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "/app/GarageBet.Api"]