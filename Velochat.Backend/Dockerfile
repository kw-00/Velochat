FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "Velochat.Backend.csproj"
RUN dotnet build "Velochat.Backend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Velochat.Backend.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "Velochat.Backend.dll"]