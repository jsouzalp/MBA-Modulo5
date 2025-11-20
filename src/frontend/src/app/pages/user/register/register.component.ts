import { AfterViewInit, Component, ElementRef, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import {
  FormGroup,
  FormControl,
  Validators,
  FormsModule,
  ReactiveFormsModule,
  FormBuilder,
  FormControlName,
} from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { MaterialModule } from '../../../material.module';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { CommonModule } from '@angular/common';
import { UserService } from 'src/app/services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { UserRegisterModel } from '../models/user-register.model';
import { MY_DATE_FORMATS } from 'src/app/shared/constants';
import { provideMomentDateAdapter } from '@angular/material-moment-adapter';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, RouterModule, MaterialModule, FormsModule, ReactiveFormsModule],
  templateUrl: './register.component.html',
  providers: [provideMomentDateAdapter(MY_DATE_FORMATS)]
})
export class UserRegisterComponent extends FormBaseComponent implements OnInit, AfterViewInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];
  form: FormGroup;
  userRegisterModel: UserRegisterModel;
  destroy$: Subject<boolean> = new Subject<boolean>();

  constructor(private router: Router, private fb: FormBuilder, private localStorageUtils: LocalStorageUtils, private userService: UserService, private toastr: ToastrService) {
    super();

    this.validationMessages = {
      nome: {
        required: 'Informe o nome.',
        minlength: 'Informe o nome completo.'
      },
      email: {
        required: 'Informe o email.',
        email: 'E-mail inválido'
      },
      dataNascimento: {
        required: 'Informe a data de nascimento.',
      },
      genero: {
        required: 'Informe o gênero.',
      },
      cpf: {
        required: 'Informe o CPF.',
      },
      telefone: {
        required: 'Informe o telefone.',
      },
      cidade: {
        required: 'Informe a cidade.',
      },
      estado: {
        required: 'Informe a UF.',
      },
      cep: {
        required: 'Informe o CEP.',
      },
      foto: {
        required: 'Informe a foto.',
      },
      senha: {
        required: 'Informe a senha',
        pattern: 'A senha deve ter entre 8 e 50 caracteres, incluindo números e símbolos.',
      },
      confirmaSenha: {
        required: 'Confirme sua senha',
        notMatching: 'As senhas não coincidem.',
      },
    };

    super.configureMessagesValidation(this.validationMessages);

  }

  ngOnInit(): void {

    this.form = this.fb.group(
      {
        nome: new FormControl<string>('', [Validators.required, Validators.minLength(6)]),
        email: new FormControl<string>('', [Validators.required, Validators.email]),
        senha: new FormControl<string>('', [
          Validators.required,
          Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})/),
        ]),
        dataNascimento: new FormControl<string>('', [Validators.required]),
        genero: new FormControl<string>('', [Validators.required]),
        telefone: new FormControl<string>('', [Validators.required]),
        cpf: new FormControl<string>('', [Validators.required]),
        cidade: new FormControl<string>('', [Validators.required]),
        estado: new FormControl<string>('', [Validators.required]),
        cep: new FormControl<string>('', [Validators.required]),
        foto: new FormControl<string>('', [Validators.required]),
        confirmaSenha: new FormControl<string>('', [Validators.required]),
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
    this.userRegisterModel = this.form.value;
    
    // Formata a data de nascimento para 'yyyy-mm-dd'
    if (this.userRegisterModel.dataNascimento) {
      const date = new Date(this.userRegisterModel.dataNascimento);
      this.userRegisterModel.dataNascimento = date.toISOString().split('T')[0];
    }

    this.userService.register(this.userRegisterModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (response) => {
          this.localStorageUtils.setUser(response);
          this.router.navigate(['/pages/dashboard']);
        },
        error: (fail) => {
          this.processFail(fail, this.toastr, this.form, ['senha', 'confirmaSenha']);
        }
      });

  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }
}
