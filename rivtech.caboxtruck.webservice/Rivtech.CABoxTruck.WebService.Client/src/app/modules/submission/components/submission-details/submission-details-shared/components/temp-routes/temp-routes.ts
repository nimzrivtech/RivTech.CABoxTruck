import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MyComponent } from './my-component';
import { EmptyComponent } from './empty-component';

const routes: Routes = [
  { path: 'temp', component: EmptyComponent },
  { path: 'my-component', component: MyComponent },
  // other routes
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }