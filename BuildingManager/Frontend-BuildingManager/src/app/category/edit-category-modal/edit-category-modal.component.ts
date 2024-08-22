import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CategoryReturnModel } from '../../models/category/categoryReturnModel';
import { CategoryCreateModel } from '../../models/category/categoryCreateModel';
import { CategoryService } from '../../services/category.service';

@Component({
  selector: 'app-edit-category-modal',
  templateUrl: './edit-category-modal.component.html',
  styleUrls: ['./edit-category-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class EditCategoryModalComponent {
  @Input() isVisible = false;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSaved = new EventEmitter<void>();
  categoryForm: FormGroup;
  category!: CategoryReturnModel;

  constructor(
    private fb: FormBuilder,
    private categoryService: CategoryService
  ) {
    this.categoryForm = this.fb.group({
      name: ['', Validators.required]
    });
  }

  open(category: CategoryReturnModel) {
    this.isVisible = true;
    this.category = category;
    this.categoryForm.patchValue({
      name: this.category.name
    });
  }

  close() {
    this.isVisible = false;
    this.categoryForm.reset();
    this.onClose.emit();
  }

  submit() {
    if (this.categoryForm.valid) {
      const formValue = this.categoryForm.value;
      const updatedCategory: CategoryCreateModel = {
        name: formValue.name
      };
      this.categoryService.updateCategory(this.category.id, updatedCategory).subscribe(
        () => {
          this.onSaved.emit();
          this.close();
        },
        error => {
          console.error('Error updating category:', error);
        }
      );
    }
  }
}
