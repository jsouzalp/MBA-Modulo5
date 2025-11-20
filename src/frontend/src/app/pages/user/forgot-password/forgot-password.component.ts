import { AfterViewInit, Component, ElementRef, OnInit, ViewChildren } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormsModule,
  ReactiveFormsModule,
  FormControlName,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MaterialModule } from '../../../material.module';
import { MatButtonModule } from '@angular/material/button';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { CommonModule } from '@angular/common';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
  ],
  templateUrl: './forgot-password.component.html',
})
export class ForgotPasswordComponent extends FormBaseComponent implements OnInit, AfterViewInit {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];
  form: FormGroup = new FormGroup({});

  constructor(private router: Router, private toastr: ToastrService, private localStorageUtils: LocalStorageUtils) {
    super();

    this.validationMessages = {
      email: {
        required: 'Informe o email.',
        email: 'E-mail inválido'
      },
    };

    super.configureMessagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    const email = this.localStorageUtils.getEmail();
    this.form = new FormGroup({
      email: new FormControl(email, [Validators.required, Validators.email]),
    });
  }

  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
  }

  get f() {
    return this.form.controls;
  }

  submit() {
    console.log(this.form.value);
    this.toastr.success('E-mail de recuperação enviado');
    this.router.navigate(['/login']);
  }
}
