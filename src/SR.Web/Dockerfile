#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/SR.Web/SR.Web.csproj", "src/SR.Web/"]
COPY ["src/SR.Components/SR.Components.csproj", "src/SR.Components/"]
COPY ["src/SR.Shared/SR.Shared.csproj", "src/SR.Shared/"]
COPY ["src/SR.Data/SR.Data.csproj", "src/SR.Data/"]
RUN dotnet restore "src/SR.Web/SR.Web.csproj"
COPY . .
WORKDIR "/src/src/SR.Web"
RUN dotnet build "SR.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SR.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SR.Web.dll"]