FROM node:14

WORKDIR /usr/src/app

COPY package*.json ./

COPY . .
RUN rm -rf node_modules && npm install --cache /tmp/empty-cache

EXPOSE 3000
CMD [ "npm", "run", "prod" ]
