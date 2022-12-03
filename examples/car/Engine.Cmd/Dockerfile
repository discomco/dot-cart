FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["examples/car/Engine.Cmd/Engine.Cmd.csproj", "Engine.Cmd/"]
COPY ["drivers/DotCart.Drivers.Serilog/DotCart.Drivers.Serilog.csproj", "DotCart.Drivers.Serilog/"]
COPY ["drivers/DotCart.Drivers.Ardalis/DotCart.Drivers.Ardalis.csproj", "DotCart.Drivers.Ardalis/"]
COPY ["drivers/DotCart.Drivers.Microsoft/DotCart.Drivers.Microsoft.csproj", "DotCart.Drivers.Microsoft/"]
COPY ["src/DotCart.Core/DotCart.Core.csproj", "DotCart.Core/"]
COPY ["examples/car/Engine.Context/Engine.Context.csproj", "Engine.Context/"]
COPY ["drivers/DotCart.Drivers.Redis/DotCart.Drivers.Redis.csproj", "DotCart.Drivers.Redis/"]
COPY ["src/DotCart.Context/DotCart.Context.csproj", "DotCart.Context/"]
COPY ["src/DotCart.Abstractions/DotCart.Abstractions.csproj", "DotCart.Abstractions/"]
COPY ["examples/car/Engine.Contract/Engine.Contract.csproj", "Engine.Contract/"]
COPY ["drivers/DotCart.Drivers.Polly/DotCart.Drivers.Polly.csproj", "DotCart.Drivers.Polly/"]
COPY ["drivers/DotCart.Drivers.Mediator/DotCart.Drivers.Mediator.csproj", "DotCart.Drivers.Mediator/"]
COPY ["drivers/DotCart.Drivers.K8S/DotCart.Drivers.K8S.csproj", "DotCart.Drivers.K8S/"]
COPY ["drivers/DotCart.Drivers.EventStoreDB/DotCart.Drivers.EventStoreDB.csproj", "DotCart.Drivers.EventStoreDB/"]
COPY ["drivers/DotCart.Drivers.NATS/DotCart.Drivers.NATS.csproj", "DotCart.Drivers.NATS/"]
COPY ["src/DotCart.Drivers.Default/DotCart.Drivers.Default.csproj", "DotCart.Drivers.Default/"]
COPY ["examples/car/Engine.Behavior/Engine.Behavior.csproj", "Engine.Behavior/"]
RUN dotnet restore "examples/car/Engine.Cmd/Engine.Cmd.csproj"
COPY . .
WORKDIR "/src/Engine.Cmd"
RUN dotnet build "Engine.Cmd.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Engine.Cmd.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Engine.Cmd.dll"]
