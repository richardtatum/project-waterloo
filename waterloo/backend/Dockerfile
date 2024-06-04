FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["src/InfoTrack.ProjectWaterloo/InfoTrack.ProjectWaterloo.csproj", "waterloo/InfoTrack.ProjectWaterloo/"]
RUN dotnet restore "waterloo/InfoTrack.ProjectWaterloo/InfoTrack.ProjectWaterloo.csproj"
COPY ./src ./waterloo
WORKDIR "/src/waterloo/InfoTrack.ProjectWaterloo"
RUN dotnet build "InfoTrack.ProjectWaterloo.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InfoTrack.ProjectWaterloo.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InfoTrack.ProjectWaterloo.dll"]