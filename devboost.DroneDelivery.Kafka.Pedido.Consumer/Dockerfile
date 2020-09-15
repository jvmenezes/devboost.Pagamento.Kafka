FROM microsoft/dotnet:3.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["devboost.DroneDelivery.Kafka\devboost.DroneDelivery.Kafka.Consumer.csproj", "devboost.DroneDelivery.Kafka.Consumer/"]
RUN dotnet restore "Kafka.Consumer.Handler/Kafka.Consumer.Handler.csproj"
COPY . .
WORKDIR "/src/devboost.DroneDelivery.Kafka.Consumer"
RUN dotnet build "devboost.DroneDelivery.Kafka.Consumer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "devboost.DroneDelivery.Kafka.Consumer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "devboost.DroneDelivery.Kafka.Consumer.dll"]