import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { UrlService } from '../../services/url.service';
import { AuthService } from '../../services/auth.service';
import { ShortUrl } from '../../models/short-url';

@Component({
  selector: 'app-url-details',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './url-details.component.html',
  styles: []
})
export class UrlDetailsComponent implements OnInit {
  url: ShortUrl | null = null;
  isLoading:  boolean = true;
  errorMessage: string = '';

  constructor(
    private route: ActivatedRoute,
    private router:  Router,
    private urlService:  UrlService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadUrl(id);
    }
  }

  loadUrl(id: number): void {
    this.urlService.getUrlById(id).subscribe({
      next: (url) => {
        this.url = url;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Failed to load URL', error);
        this.errorMessage = 'URL not found';
        this.isLoading = false;
      }
    });
  }

  getFullUrl(shortCode: string): string {
    return `http://localhost:7277/${shortCode}`;
  }

  copyToClipboard(text: string): void {
    navigator.clipboard.writeText(text);
  }

  deleteUrl(): void {
    if (this. url && confirm('Are you sure you want to delete this URL?')) {
      this.urlService.deleteUrl(this.url.id).subscribe({
        next: () => {
          this.router.navigate(['/urls']);
        },
        error:  (error) => {
          console.error('Delete failed', error);
          this.errorMessage = 'Failed to delete URL';
        }
      });
    }
  }

  goBack(): void {
    this.router.navigate(['/urls']);
  }
}