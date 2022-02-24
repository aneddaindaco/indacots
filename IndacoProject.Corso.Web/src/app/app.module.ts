import { APP_INITIALIZER, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { MembershipModule } from './membership/membership.module';
import { ApiModule, Configuration, UserService } from 'src/app/api/indaco-api';
import { ConfigService } from './config.service';
import { SignalrService } from './signalr.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent
  ],
  imports: [
    ApiModule.forRoot(() => new Configuration()),
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    MembershipModule
  ],
  providers: [
    SignalrService,
    {
      provide: APP_INITIALIZER,
      useFactory: (signalrService: SignalrService) => () =>
        signalrService.initiateSignalrConnection(),
      deps: [SignalrService],
      multi: true,
    },
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
  constructor(config: ConfigService, api:UserService) {
    console.log(config)
    api.configuration.basePath = config["apiEndpoint"]
  }
}
