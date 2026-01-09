import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ShortUrl } from '../models/short-url';
import { CreateUrlRequest } from '../models/create-url-request';

@Injectable({
  providedIn: 'root'
})
export class UrlService {
  private apiUrl = 'https://localhost:7277/api/urls';

  constructor(private http: HttpClient) { }

  getAllUrls(): Observable<ShortUrl[]> {
    return this.http.get<ShortUrl[]>(this.apiUrl);
  }

  getUrlById(id: number): Observable<ShortUrl> {
    return this.http.get<ShortUrl>(`${this.apiUrl}/${id}`);
  }

  createUrl(request: CreateUrlRequest): Observable<ShortUrl> {
    return this.http.post<ShortUrl>(this.apiUrl, request);
  }

  deleteUrl(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}