export default function (app) {
    app.get('/health', function (req, res) {
        res.send("OK");
    });
}