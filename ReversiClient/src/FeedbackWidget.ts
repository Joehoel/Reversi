interface Message {
    type: "success" | "danger";
    message: string;
}

export default class FeedbackWidget<T extends HTMLElement = HTMLDivElement> {
    private element: T;
    private selector: string;
    private key: string = "feedback_widget";

    /**
     * Creates an instance of FeedbackWidget.
     */
    constructor(selector: string) {
        if (!selector) {
            throw new Error("Element is required");
        }
        this.selector = `.${selector.slice(1)}`;
        this.element = document.querySelector<T>(selector)!;

        const close = document.querySelector<HTMLButtonElement>(`${this.selector}__close`);

        close?.addEventListener("click", () => this.hide());
    }

    /**
     * Show an alert
     */
    private show({ message, type }: Message) {
        console.log(`Showing widget with message: ${message} and type: ${type}`);

        this.element.style.opacity = "1";
        setTimeout(() => {
            this.element.style.visibility = "visible";
        }, 300);

        this.element.classList.add(`alert-${type}`);
        this.element.querySelector(`${this.selector}__text`)!.innerHTML = message;

        const icon = document.createElement("i");

        icon.classList.add(...["fa-solid", `fa-${type === "success" ? "check" : "x"}`]);

        // <i class="fa-solid fa-check"></i>;

        this.element.querySelector(`${this.selector}__content`)!.prepend(icon);

        this.log({ message, type });
    }

    /**
     * Hide the widget
     */
    public hide() {
        this.element.style.opacity = "0";
        setTimeout(() => {
            this.element.style.visibility = "hidden";
        }, 300);
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
        const logs = JSON.parse(localStorage.getItem(this.key)!) || [];
        if (logs.length >= 10) logs.splice(0, 1);

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
