import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskService } from '../../services/task.service';
import { UserService } from '../../services/user.service';
import { CommonModule } from '@angular/common';
import { TaskReturnModel } from '../../models/task/taskReturnModel';
import { UserReturnModel } from '../../models/user/userReturnModel';
import { TaskStartModel } from '../../models/task/taskStartModel';

@Component({
  selector: 'app-start-task-modal',
  templateUrl: './start-task-modal.component.html',
  styleUrls: ['./start-task-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})

export class StartTaskModalComponent {
  @Input() isVisible = false;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSaved = new EventEmitter<void>();
  taskForm: FormGroup;
  staffs: UserReturnModel[] = [];
  task!: TaskReturnModel;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService,
    private userService: UserService
  ) {
    this.taskForm = this.fb.group({
      staffId: ['', Validators.required]
    });
  }

  open(task: TaskReturnModel) {
    this.isVisible = true;
    this.task = task;
    this.taskForm.patchValue({
      staffId: task.staffId
    });
    this.loadStaff();
  }

  close() {
    this.isVisible = false;
    this.taskForm.reset();
    this.onClose.emit();
  }

  submit() {
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;
      const startedTask: TaskStartModel = {
        staffId: formValue.staffId,
      };
      this.taskService.startTask(this.task.id, startedTask).subscribe(
        () => {
          this.onSaved.emit();
          this.close();
        },
        error => {
          console.error('Error starting task:', error);
        }
      );
    }
  }

  loadStaff() {
    this.userService.getStaffs().subscribe(
      (data: UserReturnModel[]) => {
        this.staffs = data;
      },
      error => {
        console.error('Error loading staff:', error);
      }
    );
  }
}
