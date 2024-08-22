import { Component, OnInit, ViewChild } from '@angular/core';
import { HeaderComponent } from '../home/header/header.component';
import { CommonModule } from '@angular/common';
import { SidenavComponent } from '../home/sidenav/sidenav.component';
import { TaskService } from '../services/task.service';
import { AddTaskModalComponent } from './add-task-modal/add-task-modal.component';
import { SessionStorageService } from '../services/session-storage.service';
import { TaskReturnModel } from '../models/task/taskReturnModel';
import { StartTaskModalComponent } from './start-task-modal/start-task-modal.component';
import { EndTaskModalComponent } from './end-task-modal/end-task-modal.component';

@Component({
  selector: 'app-task',
  standalone: true,
  imports: [HeaderComponent, CommonModule, SidenavComponent, AddTaskModalComponent, StartTaskModalComponent, EndTaskModalComponent],
  templateUrl: './task.component.html',
  styleUrl: './task.component.css'
})
export class TaskComponent implements OnInit {
  @ViewChild('addTaskModal') addTaskModal!: AddTaskModalComponent;
  @ViewChild('startTaskModal') startTaskModal!: StartTaskModalComponent;
  @ViewChild('endTaskModal') endTaskModal!: EndTaskModalComponent;
  tasks: TaskReturnModel[] = [];
  menuOpenIndex: number | null = null;

  constructor(
    private taskService: TaskService, 
    private sessionStorageService: SessionStorageService
  ) {}

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTasks().subscribe(
      (data: TaskReturnModel[]) => {
        this.tasks = data;
      },
      error => {
        console.error('There was an error!', error);
      }
    );
  }

  getRole(): string | null {
    return this.sessionStorageService.getRole();
  }

  openAddModal() {
    this.addTaskModal.open();
    this.addTaskModal.loadApartments();
    this.addTaskModal.loadCategories();
  }

  openStartModal(task: TaskReturnModel) {
    this.startTaskModal.open(task);
  }

  openEndModal(task: TaskReturnModel) {
    this.endTaskModal.open(task);
  }

  onTaskAdded() {
    this.loadTasks();
  }

  deleteTask(id: number) {
    this.taskService.deleteTask(id).subscribe(
      () => {
        this.loadTasks();
      },
      error => {
        console.error('There was an error deleting the task!', error);
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
