#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#Use asp.net core 5.0 as base Image and its alias is base
FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
#Set Container's working directory to /app
WORKDIR /app

#Expose 80 and 443 ports
EXPOSE 80
EXPOSE 443
#Use dotnet sdk 5.0 as base Image and its alias is build
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
#Set Container's working directory to /src
WORKDIR /src
#Copy src/Services/Identity/Identity.API/Identity.API.csproj to container's src/Services/Identity/Identity.API/
COPY ["src/Services/Identity/Identity.API/Identity.API.csproj", "src/Services/Identity/Identity.API/"]
#Execute dotnet restore to install all dependencies
RUN dotnet restore "src/Services/Identity/Identity.API/Identity.API.csproj"
#Copy all files in current directory to container's /src
COPY . .
#Set Container's working directory
WORKDIR "/src/src/Services/Identity/Identity.API"
#Execute dotnet build in Release mode and output to container's /app/build
RUN dotnet build "Identity.API.csproj" -c Release -o /app/build

#Use the above build(dotnet sdk 5.0) as base image and rename it to publish
FROM build AS publish
#Execute dotnet publish in Release mode and output to container's /app/publish
RUN dotnet publish "Identity.API.csproj" -c Release -o /app/publish

#Use the above build(asp.net core 5.0) as base image and rename it to final
FROM base AS final
#Set Container's working directory to /app
WORKDIR /app
#Copy /app/publish to the current directory
COPY --from=publish /app/publish .
#Set container's Entrypoint, run dotnet Identity.API.dll when start a container
ENTRYPOINT ["dotnet", "Identity.API.dll"]