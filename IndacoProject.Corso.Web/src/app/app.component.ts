import { Component } from '@angular/core';
import { TokenStorageService } from './membership/token-storage.service';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'IndacoProject.Corso.Web';
  private roles: string[] = [];
  isLoggedIn = false;
  showAdminBoard = false;
  showModeratorBoard = false;
  username?: string;

  constructor(private tokenStorageService: TokenStorageService) { }

  ngOnInit(): void {
    this.isLoggedIn = this.tokenStorageService.loggedIn;

    if (this.isLoggedIn) {
      const user = this.tokenStorageService.getUser();
      this.roles = this.tokenStorageService.roles;

      this.showAdminBoard = this.roles.includes('ROLE_ADMIN');
      this.showModeratorBoard = this.roles.includes('ROLE_MODERATOR');

      this.username = this.tokenStorageService.username;
    }
  }

  logout(): void {
    this.tokenStorageService.logout();
    window.location.reload();
  }
}
