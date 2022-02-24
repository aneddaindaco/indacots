import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '../token-storage.service';


@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  currentUser: any;
  username: string | undefined
  constructor(public token: TokenStorageService) { }

  ngOnInit(): void {
    //this.currentUser = this.token.getUser();
    this.username = this.token.username
  }
}
