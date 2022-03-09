class FeedbackWidget {
    /**
     * Creates an instance of FeedbackWidget.
     * @param {string} element
     * @memberof FeedbackWidget
     */
    constructor(element) {
        this.element = $(element);
        this.element.css("display", "none");
        this.key = "feedback_widget";
    }

    /**
     * Show an alert
     * @param {string} message
     * @param {("success"|"danger")} type
     * @memberof FeedbackWidget
     */
    show(message, type) {
        console.log(`Showing widget with message: ${message} and type: ${type}`);
        this.element.css("display", "block");
        this.element.addClass(`alert-${type}`);
        this.element.html(message);

        this.log({ message, type });
    }

    /**
     * Hide the widget
     * @memberof FeedbackWidget
     */
    hide() {
        this.element.hide();
    }

    /**
     * Show an success alert
     * @param {string} message
     * @memberof FeedbackWidget
     */
    success(message) {
        this.show(message, "success");
    }

    /**
     * Show an danger alert
     * @param {string} message
     * @memberof FeedbackWidget
     */
    danger(message) {
        this.show(message, "danger");
    }

    /**
     * Log a message to local storage
     *
     * @param {Object} alert
     * @param {string} alert.message
     * @param {string} alert.type
     * @memberof FeedbackWidget
     */
    log(alert) {
        const logs = JSON.parse(localStorage.getItem(this.key)) ?? [];
        if (logs.length >= 10) logs.splice(0, 1);
        console.log({ length: logs.length });

        logs.push(alert);

        localStorage.setItem(this.key, JSON.stringify(logs));
    }

    /**
     * Remove all logs
     * @memberof FeedbackWidget
     */
    removeLog() {
        localStorage.removeItem(this.key);
    }

    /**
     * Display alert with log history
     *
     * @memberof FeedbackWidget
     */
    history() {
        const arr = JSON.parse(localStorage.getItem(this.key)) ?? [];

        const str = arr.map(v => `${v.type} - ${v.message}`).join("<br />");
        this.success(str);
    }
}
