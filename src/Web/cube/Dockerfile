#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#FROM node:latest as build-stage
#WORKDIR /app
#COPY "./src/Web/cube/package*.json" ./
#RUN npm install
#COPY "./src/Web/cube/" .
#ENV NODE_OPTIONS=--openssl-legacy-provider
#RUN npm run build
#
#FROM nginx as production-stage
#RUN mkdir /app
#COPY --from=build-stage /app/dist /app
#COPY nginx.conf /etc/nginx/nginx.conf


# develop stage
FROM node:alpine as develop-stage
WORKDIR /app
#COPY package*.json ./
COPY "./src/Web/cube/package*.json" ./
RUN npm install
COPY "./src/Web/cube/" .

# build stage

FROM develop-stage as build-stage
ENV NODE_OPTIONS=--openssl-legacy-provider
RUN npm run build

# production stage
FROM nginx:alpine as production-stage
COPY --from=build-stage /app/dist /usr/share/nginx/html
EXPOSE 8080
CMD ["nginx", "-g", "daemon off;"]
#