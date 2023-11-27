FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /BookFindersAPIBuildDir
COPY . .
RUN dotnet restore "BookFindersAPI/BookFindersAPI.csproj"
RUN dotnet publish "BookFindersAPI/BookFindersAPI.csproj" -c release -o /BookFindersAPI/bin/published --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:6.0-focal
WORKDIR /BookFindersAPI/bin/published
COPY --from=build /BookFindersAPI/bin/published ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "BookFindersAPI.dll"]