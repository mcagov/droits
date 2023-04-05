FROM node:14 AS build

WORKDIR /srv
ADD . .
RUN rm -rf node_modules && npm install --cache /tmp/empty-cache

FROM node:14-slim

WORKDIR /srv
COPY --from=build /srv .
EXPOSE 3000

RUN npm run prod && cat /root/.npm/_logs/*
# CMD [ "npm", "run", "prod"]
