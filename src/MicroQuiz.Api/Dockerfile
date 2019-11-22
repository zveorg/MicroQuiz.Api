FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["src/MicroQuiz.Api/MicroQuiz.Api.csproj", "src/MicroQuiz.Api/"]
RUN dotnet restore "src/MicroQuiz.Api/MicroQuiz.Api.csproj"
COPY . .
WORKDIR "/src/src/MicroQuiz.Api"
RUN dotnet build "MicroQuiz.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroQuiz.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroQuiz.Api.dll"]