const Game = (function (url) {
    const config = {
        api: url,
    };

    const privateInit = function () {
        console.log(url);
    };

    return {
        init: privateInit,
    };
})("/api/cute-cats");
