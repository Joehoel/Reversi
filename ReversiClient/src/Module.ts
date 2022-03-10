export default abstract class Module {
    private config: Record<string, any> = {};

    public static init(callback: () => void) {
        callback();
    }
}
