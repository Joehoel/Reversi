import { defineConfig } from "vite";

export default defineConfig({
    build: {
        watch: true,
        minify: true,
        rollupOptions: {
            treeshake: true,
            output: {
                dir: "../ReversiMvcApp/ReversiMvcApp/wwwroot",
                assetFileNames: assetInfo => {
                    let extType = assetInfo.name.split(".").at(1);
                    if (/png|jpe?g|svg|gif|tiff|bmp|ico/i.test(extType)) {
                        extType = "img";
                    }
                    return `${extType}/[name][extname]`;
                },
                chunkFileNames: "js/[name].js",
                entryFileNames: "js/[name].js",
                sourcemap: true,
            },
        },
        assetsDir: "",
        // outDir: "../ReversiMvcApp/ReversiMvcApp/wwwroot",
    },
});
