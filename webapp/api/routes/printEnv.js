require("dotenv-json")();

export default function (app) {

    app.get('/print-env', async function (req, res, next) {
        return res.render('print-env',{data: JSON.stringify(process.env.ENV_BASE_URL)});
    });
}
