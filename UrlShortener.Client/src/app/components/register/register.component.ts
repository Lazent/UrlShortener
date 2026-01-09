import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { AuthService } from '../../services/auth.service';

interface RegisterRequest {
  login:  string;
  password: string;
  confirmPassword: string;
  isAdmin: boolean;
}

@Component({
  selector:  'app-register',
  standalone: true,
  imports:  [CommonModule, FormsModule, RouterModule],
  templateUrl:  './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  registerRequest: RegisterRequest = {
    login: '',
    password: '',
    confirmPassword:  '',
    isAdmin: false
  };
  
  errorMessage: string = '';
  successMessage: string = '';
  isLoading: boolean = false;

  constructor(
    private authService:  AuthService,
    private router:  Router
  ) {
    console.log('RegisterComponent constructor called');
  }

  ngOnInit(): void {
    console.log('RegisterComponent loaded and initialized');
  }

  onSubmit(): void {
    console.log('Form submitted');
    
    if (!this.registerRequest.login || !this.registerRequest.password) {
      this.errorMessage = 'Please fill all fields';
      return;
    }

    if (this.registerRequest. password !== this.registerRequest.confirmPassword) {
      this.errorMessage = 'Passwords do not match';
      return;
    }

    if (this.registerRequest.password.length < 6) {
      this.errorMessage = 'Password must be at least 6 characters';
      return;
    }

    this. isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    const request = {
      login:  this.registerRequest.login,
      password: this.registerRequest. password,
      isAdmin: this.registerRequest.isAdmin
    };

    console.log('Sending registration:', request);

    this.authService.register(request).subscribe({
      next: (response:  any) => {
        console. log('Registration successful:', response);
        this.successMessage = 'Registration successful!  Redirecting to login...';
        this.isLoading = false;
        
        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 2000);
      },
      error: (error: any) => {
        console.error('Registration failed:', error);
        this.errorMessage = error.error?.message || 'Registration failed';
        this.isLoading = false;
      }
    });
  }
}