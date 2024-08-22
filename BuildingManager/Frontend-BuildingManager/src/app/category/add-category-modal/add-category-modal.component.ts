import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CategoryService } from '../../services/category.service';
import { CategoryCreateModel } from '../../models/category/categoryCreateModel';


@Component({
  selector: 'app-add-category-modal',
  templateUrl: './add-category-modal.component.html',
  styleUrls: ['./add-category-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class AddCategoryModalComponent {
  @Output() categoryAdded = new EventEmitter<void>();
  categoryForm: FormGroup;
  isVisible = false;

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService,

  ) {
    this.categoryForm = this.fb.group({
      name: ['', Validators.required],
    });
  }

  open() {
    this.isVisible = true;
  }

  close() {
    this.isVisible = false;
    this.categoryForm.reset();
  }

  submit() {
    if (this.categoryForm.valid) {
      const formValue = this.categoryForm.value;
      const newCategory: CategoryCreateModel = {
        ...formValue
      };
      this.categoryService.addCategory(newCategory).subscribe(
        () => {
          this.close();
          this.categoryAdded.emit();
          this.categoryForm.reset();
        },
        error => {
          console.error('Error adding Category:', error);
        }
      );
    }
  }
}

