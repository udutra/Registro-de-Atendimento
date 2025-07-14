FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["RegistroDeAtendimento.Web/RegistroDeAtendimento.Web.csproj", "RegistroDeAtendimento.Web/"]
COPY ["RegistroDeAtendimento.Shared/RegistroDeAtendimento.Shared.csproj", "RegistroDeAtendimento.Shared/"]
COPY ["RegistroDeAtendimento.Core/RegistroDeAtendimento.Core.csproj", "RegistroDeAtendimento.Core/"]
COPY ["RegistroDeAtendimento.Domain/RegistroDeAtendimento.Domain.csproj", "RegistroDeAtendimento.Domain/"]
RUN dotnet restore "RegistroDeAtendimento.Web/RegistroDeAtendimento.Web.csproj"

COPY . .
WORKDIR "/src/RegistroDeAtendimento.Web"
RUN dotnet publish "RegistroDeAtendimento.Web.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html

COPY --from=build /app/publish/wwwroot .

RUN rm /etc/nginx/conf.d/default.conf
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 5248