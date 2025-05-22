FROM node:24 AS build

WORKDIR /srv
ADD . .
RUN rm -rf node_modules && npm install --cache /tmp/empty-cache

FROM node:24-slim

WORKDIR /srv
COPY --from=build /srv .

RUN apt-get update && apt-get install curl -y && npm run build

EXPOSE 3000

CMD [ "npm", "run", "prod"]
