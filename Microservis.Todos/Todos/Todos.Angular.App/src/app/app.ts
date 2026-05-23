import { HttpClient } from '@angular/common/http';
import { Component, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';


@Component({
  selector: 'app-root',
  imports: [FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
      todos: TodoModel[] = [];
      work:string="";
      name:string="";
      categories: CategoryModel[] = [];


      constructor(
        private http: HttpClient
      ) {
        this.getAllTodos();
         this.getAllCategories();
      }

      getAllTodos(){
        this.http.get<TodoModel[]>("http://localhost:5000/api/todos/getall").subscribe(res=>{
          this.todos= res;
        })
      }

     saveTodo(){
      this.http.get("http://localhost:5000/api/todos/create?work=" + this.work).subscribe(res=>{
          this.getAllTodos();
        })
     }


      getAllCategories(){
        this.http.get<CategoryModel[]>("http://localhost:5000/api/categories/getall").subscribe(res=>{
          this.categories= res;
        })
      }

     saveCategories(){
      const data ={
        name:this.name
      }
      this.http.post("http://localhost:5000/api/categories/create" ,data).subscribe(res=>{
          this.getAllCategories();
        })
     }
}


export class TodoModel {
  id:number=0;
  work:string="";
}

export class CategoryModel {
  id:number=0;
  name:string="";
}
