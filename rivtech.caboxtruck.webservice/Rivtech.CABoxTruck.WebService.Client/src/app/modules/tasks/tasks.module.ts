import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskListComponent } from './components/task-list/task-list.component';
import { TaskSearchFilterComponent } from './components/task-search-filter/task-search-filter.component';



@NgModule({
  declarations: [
    TaskListComponent,
    TaskSearchFilterComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    TaskListComponent,
    TaskSearchFilterComponent,
  ]
})
export class TasksModule { }
