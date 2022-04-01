#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DataProviderApi/DataProviderApi.csproj", "DataProviderApi/"]
RUN dotnet restore "DataProviderApi/DataProviderApi.csproj"
COPY . .
WORKDIR "/src/DataProviderApi"
RUN dotnet build "DataProviderApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DataProviderApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DataProviderApi.dll"]