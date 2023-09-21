FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /Case.Frontend
COPY . .
RUN dotnet restore
RUN dotnet build Case.Frontend.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Case.Frontend.csproj -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf