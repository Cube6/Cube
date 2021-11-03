#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/ApiGateways/Web.Bff.DiscussionBoard/Web.Bff.DiscussionBoard.csproj", "src/ApiGateways/Web.Bff.DiscussionBoard/"]
RUN dotnet restore "src/ApiGateways/Web.Bff.DiscussionBoard/Web.Bff.DiscussionBoard.csproj"
COPY . .
WORKDIR "/src/src/ApiGateways/Web.Bff.DiscussionBoard"
RUN dotnet build "Web.Bff.DiscussionBoard.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Bff.DiscussionBoard.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.Bff.DiscussionBoard.dll"]
