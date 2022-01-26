import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContatComponent } from './contact/contat.component';

const routes: Routes = [
  { path: 'contact', component: ContatComponent },
  { path: 'articles', loadChildren: () => import('./articles/articles.module').then(m => m.ArticlesModule) },
  { path: '', pathMatch: 'full', redirectTo: 'articles' },
  { path: '**', redirectTo: 'articles' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
