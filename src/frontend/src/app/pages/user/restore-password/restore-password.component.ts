import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormsModule,
  ReactiveFormsModule,
  FormControlName,
  FormBuilder,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MaterialModule } from '../../../material.module';
import { MatButtonModule } from '@angular/material/button';
import { ToastrService } from 'ngx-toastr';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-restore-password',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MaterialModule,
    FormsModule,
    ReactiveFormsModule,
    MatButtonModule,
  ],
  templateUrl: './restore-password.component.html',
})
export class RestorePasswordComponent extends FormBaseComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];
  form: FormGroup;

  constructor(private router: Router, private toastr: ToastrService, private fb: FormBuilder) {
    super();

    this.validationMessages = {
      password: {
        required: 'Informe a senha',
        pattern: 'A senha deve ter entre 8 e 50 caracteres, incluindo números e símbolos.',
      },
      confirmPassword: {
        required: 'Confirme sua senha',
        notMatching: 'As senhas não coincidem.',
      },
    };

    super.configureMessagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    this.form = this.fb.group(
      {
        password: new FormControl('', [
          Validators.required,
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/),
        ]),
        confirmPassword: ['', Validators.required],
      },
      { validators: this.passwordsMatch }
    );

  }

  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
  }

  get f() {
    return this.form.controls;
  }

  submit() {
    console.log(this.form.value);
    this.toastr.success('Senha alterada, faça login.');
    this.router.navigate(['/login']);

  }

  ngOnDestroy(): void {
  }

}
