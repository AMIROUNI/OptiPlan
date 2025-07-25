import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

import { importProvidersFrom } from '@angular/core';
import { ToastrModule } from 'ngx-toastr';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

bootstrapApplication(AppComponent, {
  ...appConfig,
  providers: [
    ...(appConfig.providers || []),
    provideHttpClient(),
    importProvidersFrom(
      BrowserAnimationsModule,       // ✅ animations nécessaires pour toastr
      ToastrModule.forRoot({         // ✅ configuration toastr (optionnelle)
        positionClass: 'toast-bottom-right',
        timeOut: 3000,
        closeButton: true
      })
    )
  ]
}).catch((err) => console.error(err));
