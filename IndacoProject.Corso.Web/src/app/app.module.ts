import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { MembershipModule } from './membership/membership.module';
import { ApiModule, Configuration, ConfigurationParameters } from 'src/app/api/indaco-api';


export function apiConfigFactory(): Configuration {
  const params: ConfigurationParameters = {
    basePath: 'http://localhost:5000',
  };
  return new Configuration(params);
}

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
  ],
  imports: [
    ApiModule.forRoot(apiConfigFactory),
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    MembershipModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
