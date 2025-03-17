import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpStatusCode } from '@angular/common/http';
import {
  BehaviorSubject,
  Observable,
  Subject,
  lastValueFrom,
  of,
  throwError,
} from 'rxjs';
import { retry, catchError } from 'rxjs/operators'; 
import { Router } from '@angular/router';   
import { environment } from 'src/enviroment/enviroment';
import { LocalStorageService } from '../localstorage.service';
import { ToasterService } from '../toaster.service';
import { ResponseMessage } from 'src/app/library.common/common.objects/ResponseMessage.model';

@Injectable({
  providedIn: 'root',
})
export class AuthLoginService {
  apiUrl = environment.API_URL; 

  IsLoadingSubject: BehaviorSubject<boolean>;
  IsLoading$: Observable<boolean>;

  IsLoggedIn$: Observable<boolean>;
  IsLoggedInSubject: Subject<boolean>;

  UserName$: Observable<string>;
  UserNameSubject: Subject<string>;

  constructor(
    private http: HttpClient,
    private localStorage: LocalStorageService,
    private router: Router,
    private toastr: ToasterService, 
  ) {
    this.IsLoadingSubject = new BehaviorSubject<boolean>(false);
    this.IsLoading$ = this.IsLoadingSubject.asObservable();

    this.IsLoggedInSubject = new Subject<boolean>();
    this.IsLoggedIn$ = this.IsLoggedInSubject.asObservable();

    this.UserNameSubject = new Subject<string>();
    this.UserName$ = this.UserNameSubject.asObservable();
  }

  login(loginRequest: LoginRequest): Promise<ResponseMessage<LoginResponse>>{
    this.IsLoadingSubject.next(true);
    const url = `${this.apiUrl}login`;
    return lastValueFrom(
      this.http.post<ResponseMessage<LoginResponse>>(url, loginRequest)
    ).then(async (response) => {
      if (response.StatusCode === HttpStatusCode.Ok) {  
        await this.localStorage.SetToken(response.Data.Token as string);
        await this.localStorage.setUsername(response.Data.UserName as string); 
        await this.localStorage.setExpiration(response.Data.TokenLife as Date);
        this.IsLoggedInSubject.next(true);
        this.UserNameSubject.next(response.Data.UserName as string);
      }
      return response;
    }).finally(() => {
      this.IsLoadingSubject.next(false);
    }).catch((error) => { 
      if (error.status == 500) this.handleError(error);
      throw error;
    });
  }

  handleError(error: any) {
    let errorMessage = '';
    if (error.error instanceof ErrorEvent) {
      errorMessage = error.error.message;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    this.toastr.openToastError("Error", errorMessage);
    return throwError(() => {
      return errorMessage;
    });
  }

  
  logout() {
    this.localStorage.RemoveToken();
    this.localStorage.RemoveUsername(); 
    this.router.navigate(['/auth/login']);
  }

  isLoggedIn(): boolean { 
    return this.localStorage.GetToken() !== null && this.localStorage.GetToken() !== undefined && this.localStorage.GetToken() !== '' && this.localStorage.getExpiration() > new Date();
  }

  getUserName(): string {
    return this.localStorage.getUsername();
  }

  getToken(): string {
    return this.localStorage.GetToken();
  }

}
