export default function (app) {
    app.get('/health', function (req, res) {
        res.json({ status: 'ok', uptime: process.uptime(), timestamp: new Date().toISOString() });
    });
}