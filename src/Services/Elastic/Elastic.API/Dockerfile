#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Elastic/Elastic.API/Elastic.API.csproj", "src/Services/Elastic/Elastic.API/"]
RUN dotnet restore "src/Services/Elastic/Elastic.API/Elastic.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Elastic/Elastic.API"
RUN dotnet build "Elastic.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Elastic.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Elastic.API.dll"]