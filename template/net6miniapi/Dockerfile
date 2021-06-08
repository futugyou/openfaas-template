FROM ghcr.io/openfaas/classic-watchdog:0.1.5 as watchdog

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

# Supress collection of data.
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["root/root.csproj", "root/"]
COPY ["function/Function.csproj", "function/"]
RUN dotnet restore "root/root.csproj"
COPY . .
WORKDIR "/src/root"
RUN dotnet build "root.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "root.csproj" -c Release -o /app/publish
  

COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog
RUN chmod +x /usr/bin/fwatchdog

# Create a non-root user
RUN addgroup --system app \
    && adduser --system --ingroup app app

FROM base AS final
WORKDIR /home/app/
COPY --from=publish /app/publish .
RUN chown app:app -R /home/app

USER app

ENV fprocess="dotnet ./root.dll"
EXPOSE 8080

HEALTHCHECK --interval=3s CMD [ -e /tmp/.lock ] || exit 1

CMD ["fwatchdog"]