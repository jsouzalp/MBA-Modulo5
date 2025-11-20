import { Component, ElementRef, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryModel } from 'src/app/pages/conteudo/models/categoria.model';
import { ConteudoService } from 'src/app/services/conteudo.service';
import { CursoCreateModel } from '../../models/curso.model';
import { CursosService } from 'src/app/services/cursos.service';
import { CategoriaAddDialogComponent } from '../../categorias/categoria-add-dialog.component';
import { AulaAddDialogComponent } from '../../aulas/add/aula-add-dialog.component';
import { NgxCurrencyDirective } from "ngx-currency";

@Component({
  selector: 'app-conteudo-add',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule, MatButtonModule, NgxCurrencyDirective],
  templateUrl: './conteudo-add.component.html',
  styleUrl: './conteudo-add.component.scss'
})

export class ConteudoAddComponent extends FormBaseComponent implements OnInit, OnDestroy {
  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];

  form: FormGroup = new FormGroup({});
  cursoModel!: CursoCreateModel;
  submitted = false;
  destroy$: Subject<boolean> = new Subject<boolean>();
  categorias: CategoryModel[] = [];

  constructor(public dialog: MatDialog,
    private fb: FormBuilder,
    private categoriasService: ConteudoService,
    private cursosService: CursosService,
    private toastr: ToastrService,
    private dialogRef: MatDialogRef<ConteudoAddComponent>) {

    super();

    this.validationMessages = {
      nome: {
        required: 'Informe o nome do curso.',
        minlength: 'O nome precisa ter entre 3 e 150 caracteres.',
        maxlength: 'O nome precisa ter entre 3 e 150 caracteres.',
      },
      valor: {
        required: 'Informe o valor do curso.',
        min: 'O valor deve ser maior ou igual a 0.'
      },
      duracaoHoras: {
        required: 'Informe a duração em horas.',
        min: 'A duração deve ser maior que 0.'
      },
      nivel: {
        required: 'Informe o nível do curso.'
      },
      instrutor: {
        required: 'Informe o instrutor.'
      },
      vagasMaximas: {
        required: 'Informe as vagas máximas.',
        min: 'As vagas devem ser maior que 0.'
      },
      categoriaId: {
        required: 'Selecione a categoria.'
      },
      resumo: {
        required: 'Informe o resumo do curso.'
      },
      descricao: {
        required: 'Informe a descrição do curso.'
      },
      objetivos: {
        required: 'Informe os objetivos do curso.'
      }
    };

    super.configureMessagesValidation(this.validationMessages);
  }

  ngOnInit(): void {
    this.form = this.buildForm();
    this.loadCategorias();
  }

  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
  }

  openAddCategory(): void {
    const ref = this.dialog.open(CategoriaAddDialogComponent, {
      width: '840px',
      maxWidth: '95vw',
      disableClose: true,
      autoFocus: false
    });

    ref.afterClosed().subscribe(result => {
      if (result?.inserted)
        this.loadCategorias();
    });
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitted = true;
    const formValue = this.form.value;
    // Converte data para ISO, se presente
    const validoAte = formValue.validoAte ? new Date(formValue.validoAte).toISOString() : undefined;
    this.cursoModel = { ...formValue, validoAte } as CursoCreateModel;
    this.cursosService.create(this.cursoModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {

          if (!result) {
            this.toastr.error('Erro ao salvar o curso.');
            return;
          }

          this.toastr.success('Curso criado com sucesso.');
          // Após criar, abrir diálogo para cadastrar aulas
          const payload: any = result as any;
          const cursoId: string | undefined = typeof payload === 'string' ? payload : (payload?.id ?? payload?.cursoId);
          const cursoNome: string | undefined = typeof payload === 'object' ? (payload?.nome ?? payload?.titulo ?? undefined) : undefined;

          if (cursoId) {
            const ref = this.dialog.open(AulaAddDialogComponent, {
              width: '720px',
              maxWidth: '95vw',
              disableClose: true,
              autoFocus: false,
              data: { cursoId, cursoNome }
            });

            ref.afterClosed().subscribe(() => {
              this.dialogRef.close({ inserted: true });
            });
          } else {
            // Caso o endpoint retorne apenas o GUID e não esteja mapeado
            this.dialogRef.close({ inserted: true });
          }
        },
        error: (fail) => {
          this.submitted = false;
          const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
          this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao salvar o curso.');
        }
      });
  }

  cancel() {
    this.dialogRef.close({ inserted: false });
  }

  private buildForm(): FormGroup {
    return this.fb.group({
      nome: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(150)]],
      valor: [0, [Validators.required]],
      duracaoHoras: [0, [Validators.required]],
      nivel: ['', [Validators.required]],
      instrutor: ['', [Validators.required]],
      vagasMaximas: [0, [Validators.required]],
      imagemUrl: [''],
      validoAte: [''],
      categoriaId: ['', [Validators.required]],
      resumo: ['', [Validators.required]],
      descricao: ['', [Validators.required]],
      objetivos: ['', [Validators.required]],
      preRequisitos: [''],
      publicoAlvo: [''],
      metodologia: [''],
      recursos: [''],
      avaliacao: [''],
      bibliografia: [''],
    });
  }

  private loadCategorias(): void {
    this.categoriasService.getAllCategories()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (cats) => {
          const raw = (cats as any[]) ?? [];
          this.categorias = raw.map((c: any) => ({
            categoryId: c?.id ?? c?.categoryId ?? '',
            userId: '',
            description: c?.nome ?? c?.description ?? c?.descricao ?? '',
            type: 0,
          } as CategoryModel));
        },
        error: () => this.categorias = []
      });
  }

  ngOnDestroy(): void {
    this.destroy$.next(true);
    this.destroy$.complete();
  }

}
