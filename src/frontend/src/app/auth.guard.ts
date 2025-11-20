import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { LocalStorageUtils } from './utils/localstorage';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private localStorageUtils: LocalStorageUtils) { }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    return this.canActivateVerify(childRoute, state);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    return this.canActivateVerify(route, state);
  }

  canActivateVerify(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.isAuthenticated() && this.isValidToken()) {
      return true;
    }

    if (state?.url) this.router.navigate(['/login'], { queryParams: { returnUrl: state.url } });
    else this.router.navigate(['/login']);

    return false;
  }

  private isAuthenticated(): boolean {
    const token = this.localStorageUtils.getUserToken();
    return !!token;
  }
  private isValidToken(): boolean {
    const now = new Date();

    var dateLogged = new Date(this.localStorageUtils.getExpiresAt());

    if (now > dateLogged) {
      return false;
    }

    return true;
  }

}

@Injectable({
  providedIn: 'root'
})
export class AuthConnectedGuard implements CanActivate {

  constructor(private router: Router, private localStorageUtils: LocalStorageUtils) { }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    return this.canActivateVerify(childRoute, state);
  }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    return this.canActivateVerify(route, state);
  }

  canActivateVerify(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.isAuthenticated() && this.isValidToken()) {
      this.router.navigate(['/pages/dashboard']);
      return false;
    }

    return true;
  }

  private isAuthenticated(): boolean {
    const token = this.localStorageUtils.getUserToken();
    return !!token;
  }
  private isValidToken(): boolean {
    const now = new Date();

    var dateLogged = new Date(this.localStorageUtils.getExpiresAt());

    if (now > dateLogged) {
      return false;
    }

    return true;
  }

}



