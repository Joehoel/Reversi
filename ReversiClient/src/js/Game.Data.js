Game.Data = (function () {
    console.log("'Game.Data' loaded!");

    const config = {
        api: "3bbb1ccb121511bbda62ec2b35e9a606",
        url: "http://api.openweathermap.org/data/2.5",
        mock: [
            {
                url: "api/spel/beurt",
                data: 0,
            },
        ],
    };

    /**
     * @typedef {Object} state
     * @property {("production"|"development")} environment
     */

    /**
     * @type {state}
     */
    const state = {
        environment: "development",
    };

    const getMockData = url => {
        const mockData = config.mock.find(route => url.includes(route.url))?.data;

        return new Promise((resolve, reject) => {
            if (typeof mockData === "undefined") reject(`Geen mock data beschikbaar voor ${url}`);
            resolve(mockData);
        });
    };

    const get = async url => {
        try {
            if (state.environment == "development") {
                return getMockData(url);
            }

            const res = await fetch(`${config.url}${url}&apiKey=${config.api}`);
            const data = await res.json();

            return data;
        } catch (error) {
            console.error(error);
        }
    };

    /**
     * @param {state} { environment }
     */
    const privateInit = function ({ environment }) {
        if (environment !== "development" && environment !== "production") {
            throw new Error("Environment moet gelijk zijn aan 'production' of 'development'");
        }
        state.environment = environment;

        // get("api/spel/beurt").then(console.log);
    };

    return {
        init: privateInit,
        get,
    };
})();
