import {
  ApplicationConfig,
  provideZoneChangeDetection,
  importProvidersFrom,
  LOCALE_ID,
  DEFAULT_CURRENCY_CODE,
} from '@angular/core';
import {
  provideHttpClient,
  withInterceptorsFromDi,
  HTTP_INTERCEPTORS,
} from '@angular/common/http';
import { routes } from './app.routes';
import {
  provideRouter,
  withComponentInputBinding,
  withInMemoryScrolling,
} from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideClientHydration } from '@angular/platform-browser';

// icons
import { TablerIconsModule } from 'angular-tabler-icons';
import * as TablerIcons from 'angular-tabler-icons/icons';

// perfect scrollbar
import { NgScrollbarModule } from 'ngx-scrollbar';
//Import all material modules
import { MaterialModule } from './material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { provideAnimations } from '@angular/platform-browser/animations';
import { provideToastr } from 'ngx-toastr';
import { AuthGuard } from './auth.guard';
import { LocalStorageUtils } from './utils/localstorage';
import { registerLocaleData } from '@angular/common';
import { GlobalErrorInterceptor } from './interceptors/global-error.interceptor';
import localePt from '@angular/common/locales/pt';
import { PaginatorPtBr } from './components/PaginatorPtBr';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { NgxMaskConfig, provideEnvironmentNgxMask } from 'ngx-mask';

registerLocaleData(localePt, 'pt-BR');
const maskConfig: Partial<NgxMaskConfig> = { validation: false };
export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(
      routes,
      withInMemoryScrolling({
        scrollPositionRestoration: 'enabled',
        anchorScrolling: 'enabled',
      }),
      withComponentInputBinding(),
    ),
    provideHttpClient(withInterceptorsFromDi()),
    provideClientHydration(),
    provideAnimationsAsync(),

    importProvidersFrom(
      FormsModule,
      ReactiveFormsModule,
      MaterialModule,
      TablerIconsModule.pick(TablerIcons),
      NgScrollbarModule,
    ),
    provideAnimations(),
    provideToastr({
      timeOut: 5000,
      preventDuplicates: true,
      progressBar: true,
      closeButton: true,
    }),
    provideEnvironmentNgxMask(maskConfig),
    LocalStorageUtils,
    AuthGuard,
    { provide: LOCALE_ID, useValue: 'pt-BR' }, // Set the locale globally
    { provide: DEFAULT_CURRENCY_CODE, useValue: 'BRL' }, // Set the default currency to BRL
    { provide: HTTP_INTERCEPTORS, useClass: GlobalErrorInterceptor, multi: true },
    { provide: MatPaginatorIntl, useClass: PaginatorPtBr }
  ],
};
