FROM node:14 AS build

WORKDIR /srv
ADD . .
RUN rm -rf node_modules && npm install --cache /tmp/empty-cache && npm run build

FROM node:14-slim

COPY --from=build /srv .
EXPOSE 3000
CMD [ "npm", "run", "prod"]
