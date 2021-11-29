#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM node:latest as build-stage
WORKDIR /app
COPY "./src/Web/cube/package*.json" ./
RUN npm install
COPY "./src/Web/cube/" .
RUN npm run build

FROM nginx as production-stage
RUN mkdir /app
COPY --from=build-stage /app/dist /app
COPY nginx.conf /etc/nginx/nginx.conf
