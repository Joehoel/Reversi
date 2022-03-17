import { H as HubConnectionBuilder } from "./vendor.js";
const p = function polyfill() {
  const relList = document.createElement("link").relList;
  if (relList && relList.supports && relList.supports("modulepreload")) {
    return;
  }
  for (const link of document.querySelectorAll('link[rel="modulepreload"]')) {
    processPreload(link);
  }
  new MutationObserver((mutations) => {
    for (const mutation of mutations) {
      if (mutation.type !== "childList") {
        continue;
      }
      for (const node of mutation.addedNodes) {
        if (node.tagName === "LINK" && node.rel === "modulepreload")
          processPreload(node);
      }
    }
  }).observe(document, { childList: true, subtree: true });
  function getFetchOpts(script) {
    const fetchOpts = {};
    if (script.integrity)
      fetchOpts.integrity = script.integrity;
    if (script.referrerpolicy)
      fetchOpts.referrerPolicy = script.referrerpolicy;
    if (script.crossorigin === "use-credentials")
      fetchOpts.credentials = "include";
    else if (script.crossorigin === "anonymous")
      fetchOpts.credentials = "omit";
    else
      fetchOpts.credentials = "same-origin";
    return fetchOpts;
  }
  function processPreload(link) {
    if (link.ep)
      return;
    link.ep = true;
    const fetchOpts = getFetchOpts(link);
    fetch(link.href, fetchOpts);
  }
};
p();
var main$1 = "";
const gridElement = document.querySelector(".grid");
const rows = gridElement.querySelectorAll(".grid__row");
const grid = /* @__PURE__ */ new Map();
rows.forEach((row2) => {
  const map = /* @__PURE__ */ new Map();
  const columns = row2.querySelectorAll(".grid__column");
  columns.forEach((col) => {
    map.set(col.dataset.column, col);
  });
  grid.set(row2.dataset.row, map);
});
for (const [rowNum, row2] of grid.entries()) {
  for (const [colNum, col] of row2.entries()) {
    col.addEventListener("click", () => connection.invoke("Move", +rowNum, +colNum, "black"));
  }
}
const row = Array.from({ length: 8 }).map(() => 0);
const cols = Array.from({ length: 8 }).map(() => row.slice(0));
const board = cols;
const connection = new HubConnectionBuilder().withUrl("/hub").build();
function showFiche(row2, column, color) {
  var _a, _b;
  const fiche = document.createElement("span");
  fiche.classList.add(...["fiche", `fiche--${color}`]);
  (_b = (_a = grid.get(row2.toString())) == null ? void 0 : _a.get(column.toString())) == null ? void 0 : _b.appendChild(fiche);
  const num = color === "white" ? 1 : 2;
  board[row2 - 1][column - 1] = num;
}
async function main() {
  await connection.start();
  const token = window.location.href.split("/").at(-1);
  connection.on("move", (row2, col, color) => {
    showFiche(row2, col, color);
    console.log(color);
  });
  const response = await fetch(`https://localhost:6001/api/game/${token}`);
  await response.json();
}
document.addEventListener("DOMContentLoaded", () => {
  try {
    main();
  } catch (error) {
    console.error(error);
  }
});
//# sourceMappingURL=index.js.map
