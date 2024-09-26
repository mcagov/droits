import app from './server';
import config from "./app/config";

require("dotenv-json")();

const PORT = process.env.PORT || config.PORT;

app.listen(PORT, () => {
    console.log(`App listening on ${PORT} - url: http://localhost:${PORT}`);
    console.log('Press Ctrl+C to quit.');
});