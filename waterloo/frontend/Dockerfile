FROM node:22-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm ci
COPY . .

RUN npm run build

FROM caddy:alpine AS production
WORKDIR /usr/share/caddy

COPY --from=build /app/dist .
COPY Caddyfile /etc/caddy/Caddyfile
RUN caddy fmt /etc/caddy/Caddyfile --overwrite

CMD ["caddy", "run", "--config", "/etc/caddy/Caddyfile"]