import { MatriculaCreateModel } from './../../../../models/matricula.model';
import { PagamentoCreateModel } from './../../../../models/pagamento.model';
import { AfterViewInit, Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatriculasService } from '../../../../services/matriculas.service';
import { CursoModel } from '../../../conteudo/models/curso.model';
import { ToastrService } from 'ngx-toastr';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from '../../../../material.module';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { PagamentosService } from '../../../../services/pagamentos.service';
import { NgxMaskDirective, provideNgxMask } from 'ngx-mask';
import { MatStepper } from '@angular/material/stepper';

interface DialogData {
  curso: CursoModel;
  alunoId: string;
  pagamentoPodeSerRealizado: boolean;
  matriculaId: string | null;
}

@Component({
  selector: 'app-matricula-add',
  standalone: true,
  imports: [CommonModule, MaterialModule, MatDialogModule, ReactiveFormsModule, NgxMaskDirective],
  providers: [provideNgxMask()],
  templateUrl: './matricula-add.component.html',
  styleUrl: './matricula-add.component.scss'
})
export class MatriculaAddComponent implements OnInit, AfterViewInit {
  @ViewChild('stepper') stepper!: MatStepper;

  formMatricula!: FormGroup;
  formPagamento!: FormGroup;

  matriculaCriada = false;
  pagamentoRealizado = false;
  pagamentoPodeSerRealizado = true;
  saving = false;

  constructor(
    private matriculas: MatriculasService,
    private pagamentos: PagamentosService,
    private toastr: ToastrService,
    private fb: FormBuilder,
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    public dialogRef: MatDialogRef<MatriculaAddComponent>
  ) { }

  ngOnInit(): void {
    this.formMatricula = this.fb.group({
      cursoId: [this.data.curso.id, Validators.required],
      alunoId: [this.data.alunoId, Validators.required],
      observacao: ['']
    });

    this.formPagamento = this.fb.group({
      total: [this.data.curso.valor],
      numeroCartao: ['', Validators.required],
      nomeCartao: ['', Validators.required],
      cvvCartao: ['', Validators.required],
      expiracaoCartao: ['', Validators.required],
    });
    this.pagamentoPodeSerRealizado = this.data.pagamentoPodeSerRealizado ?? true;
  }

  ngAfterViewInit(): void {
    if (this.data.matriculaId && this.pagamentoPodeSerRealizado) {
      this.matriculaCriada = true;
      this.formMatricula.disable();
      this.formPagamento.enable();

      this.stepper.steps.toArray()[0].completed = true;
      this.stepper.selectedIndex = 1;
      this.stepper.steps.toArray()[0].editable = false;
    }
  }


  criarMatricula() {
    const data = this.formMatricula.value as MatriculaCreateModel;
    this.matriculas.criarMatricula(data).subscribe({
      next: (matriculaId) => {
        this.saving = false;
        this.matriculaCriada = true;
        this.formMatricula.disable();
        this.stepper.next();
        this.data.matriculaId = matriculaId;
        this.toastr.success('Matrícula realizada com sucesso.');
      },
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) {
          this.toastr.error(errors.join('\n'));
        } else {
          this.toastr.error('Não foi possível realizar a matrícula.');
        }
        this.saving = false;
        this.matriculaCriada = false;
      }
    })
  }

  realizarPagamento() {
    this.saving = true;
    const { cursoId, alunoId } = this.formMatricula.value;
    const { expiracaoCartao, ...payload } = this.formPagamento.value;
    const expiracaoformat = `${expiracaoCartao.substr(0, 2)}/${expiracaoCartao.substr(2, 2)}`;
    const datapagamento: PagamentoCreateModel = {
      ...payload,
      expiracaoCartao: expiracaoformat,
      matriculaId: this.data.matriculaId,
      alunoId,
      cursoId
    };
    this.pagamentos.realizarPagamento(datapagamento).subscribe({
      next: () => {
        this.toastr.success('Pagamento realizado com sucesso.');
        this.saving = false;
        this.pagamentoRealizado = true;
        this.formPagamento.disable();
        this.stepper.next();
      },
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) {
          this.toastr.error(errors.join('\n'));
        } else {
          this.toastr.error('Não foi possível realizar a matrícula/pagamento.');
        }
        this.saving = false;
        this.pagamentoRealizado = false;
      }
    });
  }

  saveMatricula() {
    if (this.formMatricula.invalid) return;
    this.saving = true;
    this.criarMatricula();
  }

  savePagamento() {
    if (this.formPagamento.invalid || !this.matriculaCriada) return;
    this.saving = true;
    this.realizarPagamento();
  }

  retryPagamento() {
    this.formPagamento.enable();
    this.pagamentoRealizado = false;
    this.stepper.selectedIndex = 1;
  }

  canGoBack() {
    return !this.pagamentoRealizado && !this.matriculaCriada;
  }
}
