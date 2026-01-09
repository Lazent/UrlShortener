import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { LoginRequest } from '../../models/login-request';
import { LoginResponse } from '../../models/login-response';

@Component({
  selector: 'app-login',
  standalone:  true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  loginRequest:  LoginRequest = {
    login:  '',
    password: ''
  };
  
  errorMessage:  string = '';
  isLoading: boolean = false;
  returnUrl: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private route: ActivatedRoute
  ) {
    this.returnUrl = this.route.snapshot. queryParams['returnUrl'] || '/urls';
  }

  onSubmit(): void {
    if (!this.loginRequest.login || !this.loginRequest.password) {
      this.errorMessage = 'Please enter login and password';
      return;
    }

    this.isLoading = true;
    this. errorMessage = '';

    this. authService.login(this.loginRequest).subscribe({
      next: (response: LoginResponse) => {
        console.log('Login successful', response);
        this.router.navigate([this.returnUrl]);
      },
      error: (error: any) => {
        console.error('Login failed', error);
        this.errorMessage = error.error?.message || 'Login failed.  Please try again.';
        this.isLoading = false;
      }
    });
  }

  quickLogin(role: 'admin' | 'user'): void {
    this.loginRequest = {
      login: role,
      password: role === 'admin' ? 'admin123' : 'user123'
    };
    this.onSubmit();
  }
}