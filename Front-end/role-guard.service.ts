import { Injectable } from '@angular/core';
import {
    Router,
    CanActivate,
    ActivatedRouteSnapshot
} from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { XxService } from 'app/xx.service';

@Injectable()
export class RoleGuardService implements CanActivate {
    userRole: string;

    constructor(public router: Router, private oAuthService: OAuthService, private xxService: XxService) {
        this.userRole = this.getUserRole();
    }

    canActivate(route: ActivatedRouteSnapshot): boolean {
        if (this.userRole !== 'XXXX') {
           this.router.navigate(['unauthorized']);
           return false;
        }

        return true;
    }

    getUserRole() {
        const base64Url = this.oAuthService.getAccessToken().split('.')[1];
        const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
        const jsonPayload = decodeURIComponent(atob(base64).split('').map(function (c) {
            return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
        }).join(''));

        return JSON.parse(jsonPayload).role;
    }
}
