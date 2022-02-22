import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/api/indaco-api';
import { TokenStorageService } from '../token-storage.service';



@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  form: any = {
    username: null,
    email: null,
    password: null
  };
  isSuccessful = false;
  isSignUpFailed = false;
  errorMessage = '';

  constructor(private userService: UserService, private tokenStorage: TokenStorageService) { }

  ngOnInit(): void {
  }

  onSubmit(): void {
    const { username, email, password } = this.form;

    this.userService.apiUserRegisterPost({
      email,
      userName: email,
      firstName: "",
      lastName: "",
      password
    }).subscribe({
      next: (data: any) => {
        this.tokenStorage.login(data.accessToken, data.refreshToken);
        this.isSuccessful = true;
        this.isSignUpFailed = false;
      },
      error: (err: any) => {
        this.errorMessage = err.error.message;
        this.isSignUpFailed = true;
      }
    });
  }
}
