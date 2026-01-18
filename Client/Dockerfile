ARG NODE_VERSION=24.7.0-alpine
FROM node:${NODE_VERSION} AS build

WORKDIR /app
COPY package.json package-lock.json ./
RUN --mount=type=cache,target=/root/.npm npm install
COPY . .
# EXPOSE 4200
# CMD ["npm", "start", "--", "--host=0.0.0.0"]
RUN npm run build

FROM nginx:alpine
COPY --from=build /app/dist/Client/browser /usr/share/nginx/html
COPY nginx/default.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
