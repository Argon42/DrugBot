FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BlazorServerApp/BlazorServerApp.csproj", "src/BlazorServerApp/"]
COPY ["src/DrugBot.Infrastructure/DrugBot.Infrastructure.csproj", "src/DrugBot.Infrastructure/"]
COPY ["src/CustomProcessors/CustomProcessors.csproj", "src/CustomProcessors/"]
COPY ["src/DrugBot.Abstraction/DrugBot.Abstraction.csproj", "src/DrugBot.Abstraction/"]
COPY ["src/DrugBot/DrugBot.csproj", "src/DrugBot/"]
COPY ["src/Anecdotes/Anecdotes.csproj", "src/Anecdotes/"]
COPY ["src/Memes/Memes.csproj", "src/Memes/"]
COPY ["src/DrugBot.Vk/DrugBot.Vk.csproj", "src/DrugBot.Vk/"]
RUN dotnet restore "src/BlazorServerApp/BlazorServerApp.csproj"
COPY . .
WORKDIR "/src/src/BlazorServerApp"
RUN dotnet build "BlazorServerApp.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BlazorServerApp.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlazorServerApp.dll"]