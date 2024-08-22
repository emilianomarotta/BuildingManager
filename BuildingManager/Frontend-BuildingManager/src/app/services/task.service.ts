import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { TaskEndpoints } from '../networking/endpoints';
import { TaskCreateModel } from '../models/task/taskCreateModel';
import { TaskReturnModel } from '../models/task/taskReturnModel';
import { TaskStartModel } from '../models/task/taskStartModel';
import { TaskFinishModel } from '../models/task/taskFinishModel';



interface ITaskService {
  getTasks(): Observable<TaskReturnModel[]>
  addTask(task: TaskCreateModel): Observable<TaskCreateModel>
}

@Injectable({
  providedIn: 'root'
})
export class TaskService implements ITaskService{

  constructor(private http: HttpClient) {}

  getTasks(): Observable<TaskReturnModel[]> {
    return this.http.get<TaskReturnModel[]>(TaskEndpoints.TASKS);
  }

  addTask(task: TaskCreateModel): Observable<TaskCreateModel> {
    return this.http.post<TaskCreateModel>(TaskEndpoints.TASKS, task);
  }

  startTask(id: number, task: TaskStartModel): Observable<TaskReturnModel> {
    return this.http.put<TaskReturnModel>(`${TaskEndpoints.TASK}${id}/start`, task);
  }

  endTask(id: number, task: TaskFinishModel): Observable<TaskReturnModel> {
    return this.http.put<TaskReturnModel>(`${TaskEndpoints.TASK}${id}/finish`, task);
  }

  deleteTask(id: number): Observable<void> {
    return this.http.delete<void>(`${TaskEndpoints.TASK}${id}`);
  }
}
