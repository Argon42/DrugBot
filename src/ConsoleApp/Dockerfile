FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ConsoleApp/ConsoleApp.csproj", "ConsoleApp/"]
COPY ["DrugBot/DrugBot.csproj", "DrugBot/"]
RUN dotnet restore "ConsoleApp/ConsoleApp.csproj"
COPY . .
WORKDIR "/src/ConsoleApp"
RUN dotnet build "ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
ARG VK_GROUP_TOKEN="default"
ENV VK_GROUP_TOKEN=${VK_GROUP_TOKEN}
ARG VK_APP_ID="default"
ENV VK_APP_ID=${VK_APP_ID}
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DrugBotApp.dll"]
