import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { TaskService } from '../../services/task.service';
import { CommonModule } from '@angular/common';
import { TaskReturnModel } from '../../models/task/taskReturnModel';
import { TaskFinishModel } from '../../models/task/taskFinishModel';

@Component({
  selector: 'app-end-task-modal',
  templateUrl: './end-task-modal.component.html',
  styleUrls: ['./end-task-modal.component.css'],
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule]
})

export class EndTaskModalComponent {
  @Input() isVisible = false;
  @Output() onClose = new EventEmitter<void>();
  @Output() onSaved = new EventEmitter<void>();
  taskForm: FormGroup;
  task!: TaskReturnModel;

  constructor(
    private fb: FormBuilder,
    private taskService: TaskService
  ) {
    this.taskForm = this.fb.group({
      cost: ['', Validators.required]
    });
  }

  open(task: TaskReturnModel) {
    this.isVisible = true;
    this.task = task;
    this.taskForm.patchValue({
      cost: task.cost
    });
  }

  close() {
    this.isVisible = false;
    this.taskForm.reset();
    this.onClose.emit();
  }

  submit() {
    if (this.taskForm.valid) {
      const formValue = this.taskForm.value;
      const endedTask: TaskFinishModel = {
        cost: formValue.cost,
      };
      this.taskService.endTask(this.task.id, endedTask).subscribe(
        () => {
          this.onSaved.emit();
          this.close();
        },
        error => {
          console.error('Error ending task:', error);
        }
      );
    }
  }
}
