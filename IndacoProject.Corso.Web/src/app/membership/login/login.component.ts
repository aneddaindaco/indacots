import { Component, OnInit } from '@angular/core';
import { ApiModule, UserService } from 'src/app/api/indaco-api';
import { TokenStorageService } from '../token-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: any = {
    username: null,
    password: null
  };
  isLoggedIn = false;
  isLoginFailed = false;
  errorMessage = '';

  constructor(private userService: UserService, private tokenStorage: TokenStorageService) { }

  ngOnInit(): void {
    if (this.tokenStorage.accessToken) {
      this.isLoggedIn = true;
    }
  }

  onSubmit(): void {
    const { username, password } = this.form;

    this.userService.apiUserLoginPost({ userName: username, password }).subscribe({
      next: (data:any) => {
        this.tokenStorage.login(data.accessToken, data.refreshToken);
        this.isLoginFailed = false;
        this.isLoggedIn = this.tokenStorage.loggedIn;
        this.reloadPage();
      },
      error: (err: any) => {
        this.errorMessage = err.error.message;
        this.isLoginFailed = true;
      }
    });
  }

  reloadPage(): void {
    window.location.reload();
  }
}
