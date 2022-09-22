# build stage
FROM node:18 as frontend-builder

WORKDIR /app
COPY ./frontend/package.json ./package.json
RUN npm i

COPY ./frontend .

RUN npm run build

CMD ["npm", "run", "start"]


FROM hub.nexdev.net/base/dotnet/core/sdk:6.0 AS netcore-builder

WORKDIR /src

COPY ./backend .

RUN dotnet restore -v diag

WORKDIR /src/ReactAdminNet

RUN dotnet publish -r linux-x64 --self-contained false -c Release -o /app 

FROM hub.nexdev.net/base/dotnet/core/aspnet:6.0 AS base

WORKDIR /app
COPY --from=netcore-builder /app .
COPY --from=frontend-builder /app/build ./wwwroot

ENTRYPOINT ["dotnet", "ReactAdminNet.dll"]