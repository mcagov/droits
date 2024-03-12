FROM node:14 AS build

WORKDIR /srv
ADD . .
RUN sudo apt-get install cpulimit && rm -rf node_modules && cpulimit -l 80 npm install --cache /tmp/empty-cache

FROM node:14-slim

WORKDIR /srv
COPY --from=build /srv .
EXPOSE 3000

CMD [ "npm", "run", "prod"]
