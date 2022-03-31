var __defProp = Object.defineProperty;
var __defNormalProp = (obj, key, value) => key in obj ? __defProp(obj, key, { enumerable: true, configurable: true, writable: true, value }) : obj[key] = value;
var __publicField = (obj, key, value) => {
  __defNormalProp(obj, typeof key !== "symbol" ? key + "" : key, value);
  return value;
};
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
class Game {
  constructor(url, token, playerToken) {
    __publicField(this, "url");
    __publicField(this, "token");
    __publicField(this, "playerToken");
    __publicField(this, "grid", /* @__PURE__ */ new Map());
    __publicField(this, "board", [[]]);
    __publicField(this, "connection");
    this.url = url;
    this.token = token;
    this.playerToken = playerToken;
    this.connection = new HubConnectionBuilder().withUrl("/hub").build();
    this.init();
    const gridElement = document.querySelector(".grid");
    const rows = gridElement.querySelectorAll(".grid__row");
    rows.forEach((row) => {
      const map = /* @__PURE__ */ new Map();
      const columns = row.querySelectorAll(".grid__column");
      columns.forEach((col) => {
        map.set(col.dataset.column, col);
      });
      this.grid.set(row.dataset.row, map);
    });
    for (const [rowNum, row] of this.grid.entries()) {
      for (const [colNum, col] of row.entries()) {
        col.addEventListener("click", () => {
          this.move({ column: parseInt(colNum), row: parseInt(rowNum) });
        });
      }
    }
  }
  async init() {
    try {
      await this.connection.start();
      await this.updateBoard();
      this.connection.invoke("Join", this.token);
      this.connection.on("message", (message) => console.log(`Message: ${message}`));
      this.connection.on("move", async () => {
        await this.updateBoard();
      });
    } catch (error) {
      console.error(error);
    }
  }
  async updateBoard() {
    console.log("Updating");
    const response = await fetch(`${this.url}/${this.token}`);
    if (response.status === 502) {
      await this.updateBoard();
    } else if (response.status != 200) {
      console.error(`Something went wrong: ${response.statusText}`);
      await new Promise((resolve) => setTimeout(resolve, 1e4));
      await this.updateBoard();
    } else {
      const game = await response.json();
      if (game.winner !== 0) {
      }
      this.board = game.board;
      this.board.forEach((row, rowIndex) => {
        row.forEach((value, colIndex) => {
          this.showFiche({
            color: value,
            column: colIndex,
            row: rowIndex
          });
        });
      });
    }
  }
  async move({ column, row }) {
    const move = {
      playerToken: this.playerToken,
      token: this.token,
      row,
      column
    };
    const response = await fetch(`${this.url}/move`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(move)
    });
    switch (response.status) {
      case 401:
        alert("Niet jou beurt");
        break;
      case 400:
        alert("Niet mogelijk");
      case 200:
        this.connection.invoke("Move", this.token);
      default:
        break;
    }
  }
  showFiche({ color, column, row }) {
    var _a;
    if (color === 0 || typeof color === "undefined") {
      return;
    }
    const colors = {
      "1": "white",
      "2": "black"
    };
    const fiche = document.createElement("span");
    fiche.classList.add(...["fiche", `fiche--${colors[color]}`]);
    const parent = (_a = this.grid.get(row.toString())) == null ? void 0 : _a.get(column.toString());
    if (parent) {
      parent.childNodes.forEach((node) => node.remove());
      parent.appendChild(fiche);
    }
    if (this.board) {
      this.board[row][column] = color;
    }
  }
}
function handleClick(e) {
}
async function main() {
  var _a;
  const token = decodeURIComponent(window.location.href.split("/").at(-1));
  const playerToken = (_a = document.querySelector("#playerToken")) == null ? void 0 : _a.value;
  console.log(playerToken);
  const game = new Game("https://localhost:6001/api/game", token, playerToken);
}
document.addEventListener("DOMContentLoaded", () => {
  try {
    main();
  } catch (error) {
    console.error(error);
  }
});
//# sourceMappingURL=index.js.map
