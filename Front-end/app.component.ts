import { Component } from '@angular/core';
import { OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';
import { AppSettings } from './app.settings';
import { interval } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  clientId: string;
  adfsUrl: string;

  public constructor(private oauthService: OAuthService, private appSettings: AppSettings) {
    this.clientId = appSettings.clientId();
    this.adfsUrl = appSettings.adfsEndpoint();

    this.configure();
    this.oauthService.setStorage(sessionStorage);
    this.oauthService.setStorage(localStorage);
    this.oauthService.tryLogin();

    this.oauthService.timeoutFactor = 0.9;
    this.oauthService.setupAutomaticSilentRefresh();
  }

  private configure() {
    this.oauthService.configure({
      redirectUri: window.location.origin,
      clientId: this.clientId,
      loginUrl: this.adfsUrl + '/oauth2/authorize',
      issuer: this.adfsUrl,
      scope: 'openid profile',
      responseType: 'id_token token',
      oidc: true,
      requestAccessToken: true,
      strictDiscoveryDocumentValidation: false,
      logoutUrl: this.adfsUrl +
        '/ls/?wa=wsignoutcleanup1.0&wreply=' + location.protocol +
        '//' + location.hostname + (location.port ? ':' + location.port : ''),
        postLogoutRedirectUri: location.protocol + '//' +
        location.hostname + (location.port ? ':' + location.port : '')
    });
  }
}
