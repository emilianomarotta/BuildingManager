import { Component, OnInit, ViewChild } from '@angular/core';
import { HeaderComponent } from '../home/header/header.component';
import { CommonModule } from '@angular/common';
import { SidenavComponent } from '../home/sidenav/sidenav.component';
import { AddCategoryModalComponent } from './add-category-modal/add-category-modal.component';
import { CategoryService } from '../services/category.service';
import { SessionStorageService } from '../services/session-storage.service';
import { CategoryReturnModel } from '../models/category/categoryReturnModel';
import { EditCategoryModalComponent } from './edit-category-modal/edit-category-modal.component';

@Component({
  selector: 'app-category',
  standalone: true,
  imports: [HeaderComponent, CommonModule, SidenavComponent, AddCategoryModalComponent, EditCategoryModalComponent],
  templateUrl: './category.component.html',
  styleUrl: './category.component.css'
})
export class CategoryComponent implements OnInit {
  @ViewChild('addCategoryModal') addCategoryModal!: AddCategoryModalComponent;
  @ViewChild('editCategoryModal') editCategoryModal!: EditCategoryModalComponent;
  categories: CategoryReturnModel[] = [];
  menuOpenIndex: number | null = null;

  constructor(
    private categoryService: CategoryService, 
    private sessionStorageService: SessionStorageService
  ) {}

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe(
      (data: CategoryReturnModel[]) => {
        this.categories = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }
  
  getRole(): string | null {
    return this.sessionStorageService.getRole();
  }

  openModal() {
    this.addCategoryModal.open();
  }

  onCategoryAdded() {
    this.loadCategories();
  }

  openEditModal(category: CategoryReturnModel) {
    this.editCategoryModal.open(category);
  }

  deleteCategory(id: number) {
    this.categoryService.deleteCategory(id).subscribe(
      () => {
        this.loadCategories();
      },
      error => {
        console.error('There was an error deleting the category!', error);
      }
    );
  }

  toggleMenu(index: number) {
    this.menuOpenIndex = this.menuOpenIndex === index ? null : index;
  }

  closeMenu() {
    this.menuOpenIndex = null;
  }
}
