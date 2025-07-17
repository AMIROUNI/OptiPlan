import { Component, EventEmitter, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { PopupComponent } from "../../popup/popup.component";
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-login',
  standalone: true,
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
  imports: [PopupComponent, ReactiveFormsModule,CommonModule]
})
export class LoginComponent {
// popup variables ///////////////////////////////////////////////////////////////
  showPopup = false;
  popupTitle = '';
  popupMessage = '';
  popupIsSuccess = false;
  popupRedirectPath: string | null = null;
  showCancelButton = false;


  //data: string[] = [];

   @Output() onSubmitLoginEvent = new EventEmitter();
   login :string='';
   password :string='';
  constructor(private authService: AuthService, private router: Router) { }

  formLogin=new FormGroup({
    username:new FormControl('',[Validators.required,Validators.minLength(4)]),
    password:new FormControl('',[Validators.required, Validators.minLength(8)]),
  })




  isInvalidAndTouchedOrDirty(FormControl: FormControl): boolean {
   return FormControl.invalid && (FormControl.touched || FormControl.dirty);
  }
  errorMessage : string = '';

 onSubmitLogin(): void {
  if (this.formLogin.invalid) return;

  const credentials = {
    username: this.formLogin.value.username || '',
    password: this.formLogin.value.password || ''
  };

  this.authService.login(credentials).subscribe({
    next: (res) => {
      console.log("Full login response:", res);
      
      // Pass the entire response to saveToken
      this.authService.saveToken(res);
      this.authService.redirectByRole();
    },
    error: (err) => {
      console.error("Login error:", err);
      if (err.status === 0) {
        this.showErrorPopup("Cannot connect to server. Please check your connection.");
      } else {
        this.showErrorPopup(err.error?.message || "Login failed");
      }
    }
  });
}

  /// popup methods //////////////////////////////////////////

  showSuccessPopup() {
    this.popupTitle = 'Account Created!';
    this.popupMessage = 'Your account has been successfully created.';
    this.popupIsSuccess = true;
    this.popupRedirectPath = '/login';
    this.showCancelButton = false;
    this.showPopup = true;
  }

  showErrorPopup(errorMessage: string) {
    this.popupTitle = 'Login Failed';
    this.popupMessage = errorMessage;
    this.popupIsSuccess = false;
    this.popupRedirectPath = null;
    this.showCancelButton = true;
    this.showPopup = true;
  }

  closePopup() {
    this.showPopup = false;
  }
////////////////////////////////////



}
