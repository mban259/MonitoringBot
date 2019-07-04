FROM microsoft/dotnet

WORKDIR /app

COPY MonitoringBot/MonitoringBot.csproj ./
RUN dotnet restore

COPY MonitoringBot ./
RUN dotnet publish -c Release -o out

WORKDIR /app/out

COPY docker-entrypoint.sh ./

CMD ["./docker-entrypoint.sh"]
