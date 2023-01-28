FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CarBooking/CarBooking.csproj", "CarBooking/"]
RUN dotnet restore "CarBooking/CarBooking.csproj"
COPY . .
WORKDIR "/src/CarBooking"
RUN dotnet build "CarBooking.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarBooking.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarBooking.dll"]
