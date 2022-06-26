#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ProjectTracker.Ui.Api/ProjectTracker.Ui.Api.csproj", "ProjectTracker.Ui.Api/"]
RUN dotnet restore "ProjectTracker.Ui.Api/ProjectTracker.Ui.Api.csproj"
COPY . .
WORKDIR "/src/ProjectTracker.Ui.Api"
RUN dotnet build "ProjectTracker.Ui.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProjectTracker.Ui.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ProjectTracker.Ui.Api.dll"]