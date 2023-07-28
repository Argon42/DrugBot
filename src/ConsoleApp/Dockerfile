FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/ConsoleApp/ConsoleApp.csproj", "src/ConsoleApp/"]
COPY ["src/CustomProcessors/CustomProcessors.csproj", "src/CustomProcessors/"]
COPY ["src/DrugBot.Abstraction/DrugBot.Abstraction.csproj", "src/DrugBot.Abstraction/"]
COPY ["src/DrugBot/DrugBot.csproj", "src/DrugBot/"]
COPY ["src/Anecdotes/Anecdotes.csproj", "src/Anecdotes/"]
COPY ["src/Memes/Memes.csproj", "src/Memes/"]
COPY ["src/DrugBot.Vk/DrugBot.Vk.csproj", "src/DrugBot.Vk/"]
RUN dotnet restore "src/ConsoleApp/ConsoleApp.csproj"
COPY . .
WORKDIR "/src/src/ConsoleApp"
RUN dotnet build "ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DrugBotApp.dll"]
