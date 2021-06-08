FROM ghcr.io/openfaas/classic-watchdog:0.1.5 as watchdog

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 443

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

FROM base AS final
COPY --from=publish /app/publish .
COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog

RUN chmod +x /usr/bin/fwatchdog

# Create a non-root user
RUN addgroup --system app && adduser --system --ingroup app app --home /app

RUN chown app:app -R /app

USER app
ENV fprocess="dotnet ./root.dll"

HEALTHCHECK --interval=3s CMD [ -e /tmp/.lock ] || exit 1

CMD ["fwatchdog"]