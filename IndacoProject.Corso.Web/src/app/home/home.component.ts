import { Component, OnInit } from '@angular/core';
import { UserService } from '../api/indaco-api';
import { SignalrService } from '../signalr.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent  implements OnInit {
  hubMailMessage: string | undefined;

  constructor(public signalrService: SignalrService) { }

  ngOnInit(): void {

    this.signalrService.hubHelloMessage.subscribe((message: string) => {
      this.hubMailMessage = message;
    });
  }
}
