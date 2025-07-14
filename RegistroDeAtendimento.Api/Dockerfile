FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5253

RUN apt-get update && apt-get install -y curl

USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src


COPY ["RegistroDeAtendimento.Api/RegistroDeAtendimento.Api.csproj", "RegistroDeAtendimento.Api/"]
COPY ["RegistroDeAtendimento.Infrastructure/RegistroDeAtendimento.Infrastructure.csproj", "RegistroDeAtendimento.Infrastructure/"]
COPY ["RegistroDeAtendimento.Domain/RegistroDeAtendimento.Domain.csproj", "RegistroDeAtendimento.Domain/"]
COPY ["RegistroDeAtendimento.Core/RegistroDeAtendimento.Core.csproj", "RegistroDeAtendimento.Core/"]
COPY ["RegistroDeAtendimento.Application/RegistroDeAtendimento.Application.csproj", "RegistroDeAtendimento.Application/"]
COPY ["RegistroDeAtendimento.Shared/RegistroDeAtendimento.Shared.csproj", "RegistroDeAtendimento.Shared/"]
RUN dotnet restore "RegistroDeAtendimento.Api/RegistroDeAtendimento.Api.csproj"
COPY . .
WORKDIR "/src/RegistroDeAtendimento.Api"
RUN dotnet build "./RegistroDeAtendimento.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

RUN apt-get update && apt-get install -y curl

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RegistroDeAtendimento.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RegistroDeAtendimento.Api.dll"]
