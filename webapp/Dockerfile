FROM node:14

WORKDIR /usr/src/app

COPY package*.json ./

COPY . .
RUN rm package-lock.json && npm cache clean --force && rm -rf node_modules && npm install

EXPOSE 3000
CMD [ "npm", "run", "start" ]
