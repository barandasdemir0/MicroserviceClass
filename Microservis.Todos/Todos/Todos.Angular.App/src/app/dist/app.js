"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
exports.__esModule = true;
exports.CategoryModel = exports.TodoModel = exports.App = void 0;
var core_1 = require("@angular/core");
var forms_1 = require("@angular/forms");
var App = /** @class */ (function () {
    function App(http) {
        this.http = http;
        this.todos = [];
        this.work = "";
        this.name = "";
        this.categories = [];
        this.getAllTodos();
        this.getAllCategories();
    }
    App.prototype.getAllTodos = function () {
        var _this = this;
        this.http.get("http://localhost:5000/api/todos/getall").subscribe(function (res) {
            _this.todos = res;
        });
    };
    App.prototype.saveTodo = function () {
        var _this = this;
        this.http.get("http://localhost:5000/api/todos/create?work=" + this.work).subscribe(function (res) {
            _this.getAllTodos();
        });
    };
    App.prototype.getAllCategories = function () {
        var _this = this;
        this.http.get("http://localhost:5000/api/categories/getall").subscribe(function (res) {
            _this.categories = res;
        });
    };
    App.prototype.saveCategories = function () {
        var _this = this;
        var data = {
            name: this.name
        };
        this.http.post("http://localhost:5000/api/categories/create", data).subscribe(function (res) {
            _this.getAllCategories();
        });
    };
    App = __decorate([
        core_1.Component({
            selector: 'app-root',
            imports: [forms_1.FormsModule],
            templateUrl: './app.html',
            styleUrl: './app.css'
        })
    ], App);
    return App;
}());
exports.App = App;
var TodoModel = /** @class */ (function () {
    function TodoModel() {
        this.id = 0;
        this.work = "";
    }
    return TodoModel;
}());
exports.TodoModel = TodoModel;
var CategoryModel = /** @class */ (function () {
    function CategoryModel() {
        this.id = 0;
        this.name = "";
    }
    return CategoryModel;
}());
exports.CategoryModel = CategoryModel;
