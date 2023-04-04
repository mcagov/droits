FROM node:14 AS build

WORKDIR /srv

ADD package.json .
RUN rm -rf node_modules && npm install --cache /tmp/empty-cache


FROM node:14-slim

COPY --from=build /srv .
ADD . .

EXPOSE 3000

CMD [ "node", "-r", "esm", "server.js"]
