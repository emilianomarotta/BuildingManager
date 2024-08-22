import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../../services/task.service';
import { TaskCreateModel } from '../../models/task/taskCreateModel';
import { CategoryService } from '../../services/category.service';
import { ApartmentService } from '../../services/apartment.service';
import { CategoryReturnModel } from '../../models/category/categoryReturnModel';
import { ApartmentReturnModel } from '../../models/apartment/apartmentReturnModel';

@Component({
  selector: 'app-add-task-modal',
  templateUrl: './add-task-modal.component.html',
  styleUrls: ['./add-task-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})
export class AddTaskModalComponent {
  @Output() taskAdded = new EventEmitter<void>();
  taskForm: FormGroup;
  isVisible = false;
  categories: CategoryReturnModel[] = [];
  apartments: ApartmentReturnModel[] = [];

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private categoryService: CategoryService,
    private apartmentService: ApartmentService
  ) {
    this.taskForm = this.fb.group({
      categoryId: ['', Validators.required],
      apartmentId: ['', Validators.required],
      description: ['', Validators.required],
      creationDate: [new Date(), Validators.required],
      staffId: [null],
      startDate: [null],
      endDate: [null],
      cost: [null]
    });
  }

  open() {
    this.isVisible = true;
  }

  close() {
    this.isVisible = false;
    this.taskForm.reset();
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe(
      (data: CategoryReturnModel[]) => {
        this.categories = data;
      },
      error => {
        console.error('Error loading categories:', error);
      }
    );
  }

  loadApartments() {
    this.apartmentService.getApartments().subscribe(
      (data: ApartmentReturnModel[]) => {
        this.apartments = data;
      },
      error => {
        console.error('Error loading apartments:', error);
      }
    );
  }

  submit() {
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;
      const newTask: TaskCreateModel = {
        ...formValue,
        staffId: null,
        startDate: null,
        endDate: null,
        cost: null
      };
      this.taskService.addTask(newTask).subscribe(
        () => {
          this.close();
          this.taskAdded.emit();
          this.taskForm.reset();
        },
        error => {
          console.error('Error adding task:', error);
        }
      );
    }
  }
}

