interface Message {
    type: "success" | "danger";
    message: string;
}

export default class FeedbackWidget<T extends HTMLElement = HTMLDivElement> {
    private element: T;
    private key: string = "feedback_widget";

    /**
     * Creates an instance of FeedbackWidget.
     */
    constructor(element: string) {
        if (!element) {
            throw new Error("Element is required");
        }
        this.element = document.querySelector<T>(element)!;
        this.element.style.display = "none";
    }

    /**
     * Show an alert
     */
    private show({ message, type }: Message) {
        console.log(`Showing widget with message: ${message} and type: ${type}`);
        this.element.style.display = "block";
        this.element.classList.add(`alert-${type}`);
        this.element.innerHTML = message;

        this.log({ message, type });
    }

    /**
     * Hide the widget
     */
    public hide() {
        this.element.style.display = "none";
    }

    /**
     * Show an success alert
     */
    public success(message: Message["message"]) {
        this.show({ message, type: "success" });
    }

    /**
     * Show an danger alert
     */
    public danger(message: Message["message"]) {
        this.show({ message, type: "danger" });
    }

    /**
     * Log a message to local storage
     */
    public log(alert: Message) {
        const logs = JSON.parse(localStorage.getItem(this.key) ?? "") || [];
        if (logs.length >= 10) logs.splice(0, 1);
        console.log({ length: logs.length });

        logs.push(alert);

        localStorage.setItem(this.key, JSON.stringify(logs));
    }

    /**
     * Remove all logs
     */
    private removeLog() {
        localStorage.removeItem(this.key);
    }

    /**
     * Display alert with log history
     */
    public history() {
        const arr: Message[] = JSON.parse(localStorage.getItem(this.key) ?? "") ?? [];
        const str = arr.map(v => `${v.type} - ${v.message}`).join("<br />");
        this.success(str);
    }
}
