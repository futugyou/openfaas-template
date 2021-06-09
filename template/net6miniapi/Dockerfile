FROM ghcr.io/openfaas/of-watchdog:0.8.4 as watchdog

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["openfaas/Openfaas.csproj", "openfaas/"]
COPY ["function/Function.csproj", "function/"]
COPY ["foundation/Foundation.csproj", "foundation/"]
RUN dotnet restore "openfaas/Openfaas.csproj"
COPY . .
WORKDIR "/src/openfaas"
RUN dotnet build "Openfaas.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Openfaas.csproj" -c Release -o /app/publish


FROM base AS final
WORKDIR /home/app
COPY --from=publish /app/publish .
COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog

RUN chmod +x /usr/bin/fwatchdog

# Create a non-root user
RUN addgroup --system app && adduser --system --ingroup app app

RUN chown -R app /home/app

USER app
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
ENV upstream_url="http://127.0.0.1:80"
ENV fprocess="dotnet ./Openfaas.dll"
ENV write_debug=true
ENV cgi_headers="true"
ENV mode="http"
EXPOSE 8080
EXPOSE 443

HEALTHCHECK --interval=3s CMD [ -e /tmp/.lock ] || exit 1

CMD ["fwatchdog"]