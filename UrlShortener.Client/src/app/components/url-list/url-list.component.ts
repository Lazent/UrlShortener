import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { UrlService } from '../../services/url.service';
import { AuthService } from '../../services/auth.service';
import { ShortUrl } from '../../models/short-url';
import { CreateUrlRequest } from '../../models/create-url-request';

@Component({
  selector:  'app-url-list',
  standalone: true,
  imports:  [CommonModule, FormsModule, RouterModule],
  templateUrl:  './url-list.component.html',
  styleUrls: ['./url-list.component.scss']
})
export class UrlListComponent implements OnInit {
  urls: ShortUrl[] = [];
  newUrl: string = '';
  isLoading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';

  constructor(
    private urlService: UrlService,
    public authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    console.log('UrlListComponent initialized');
    this.loadUrls();
  }

  loadUrls(): void {
    this.isLoading = true;
    this. urlService.getAllUrls().subscribe({
      next: (data:  ShortUrl[]) => {
        console.log('URLs loaded:', data);
        this.urls = data;
        this. isLoading = false;
      },
      error: (error:  any) => {
        console. error('Error loading URLs', error);
        this.errorMessage = 'Failed to load URLs';
        this.isLoading = false;
      }
    });
  }

  createUrl(): void {
    if (!this.newUrl || !this.newUrl.trim()) {
      this.errorMessage = 'Please enter a URL';
      return;
    }

    const request: CreateUrlRequest = {
      originalUrl: this. newUrl.trim()
    };

    this.isLoading = true;
    this.errorMessage = '';
    this.successMessage = '';

    this.urlService.createUrl(request).subscribe({
      next: (createdUrl: ShortUrl) => {
        console.log('URL created:', createdUrl);
        this.urls.unshift(createdUrl);
        this.newUrl = '';
        this.successMessage = 'URL created successfully! ';
        this.isLoading = false;
        
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error: any) => {
        console.error('Error creating URL', error);
        this.errorMessage = error.error?.message || 'Failed to create URL';
        this.isLoading = false;
      }
    });
  }

  deleteUrl(id: number, createdBy: string): void {
    const currentUser = this.authService.currentUserValue;
    
    if (! currentUser) return;

    if (!currentUser.isAdmin && currentUser.login !== createdBy) {
      this.errorMessage = 'You can only delete your own URLs';
      return;
    }

    if (!confirm('Are you sure you want to delete this URL?')) {
      return;
    }

    this.urlService.deleteUrl(id).subscribe({
      next: () => {
        console.log('URL deleted:', id);
        this.urls = this.urls.filter(u => u.id !== id);
        this.successMessage = 'URL deleted successfully!';
        setTimeout(() => this.successMessage = '', 3000);
      },
      error: (error: any) => {
        console.error('Error deleting URL', error);
        this.errorMessage = 'Failed to delete URL';
      }
    });
  }

  getFullUrl(shortCode: string): string {
    return `http://localhost:7277/${shortCode}`;
  }

  copyToClipboard(text: string): void {
    navigator.clipboard.writeText(text).then(() => {
      this.successMessage = 'Copied to clipboard!';
      setTimeout(() => this.successMessage = '', 2000);
    });
  }
}