import { Category } from './../models/category';
import { CategoryService } from './../services/category.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormControl } from '@angular/forms';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.css']
})
export class CategoryComponent implements OnInit {

  CategoryForm: FormGroup;
  submited = false;
  model: any = {};
  category: Category;
  categorys: Category[];
  constructor(private categoryService: CategoryService) { }

  ngOnInit() {

    this.CategoryForm = new FormGroup({
      name: new FormControl('', Validators.required)
    });
    this.loadCategories();
  }

  loadCategories() {
    console.log('hello');
    this.categoryService.getCategories().subscribe((c: Category[]) => {
      this.categorys = c;
      console.log(this.categorys);
    }, error => {
      console.log('error');
    });
  }

  CategoryCreate() {
    if (this.CategoryForm.valid) {
      console.log(this.CategoryForm.value);
      this.category = Object.assign({}, this.CategoryForm.value);
      console.log(this.category);
      this.categoryService.createCategory(this.category).subscribe(() => {
        console.log('ok');
      }, error => {
        console.log('error');
      });
    }

  }

}
