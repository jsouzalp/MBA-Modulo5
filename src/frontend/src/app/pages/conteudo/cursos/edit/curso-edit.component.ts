import { Component, ElementRef, Inject, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Subject, takeUntil } from 'rxjs';
import { MaterialModule } from 'src/app/material.module';
import { CommonModule } from '@angular/common';
import { FormBaseComponent } from 'src/app/components/base-components/form-base.component';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { FormBuilder, FormControlName, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryModel } from 'src/app/pages/conteudo/models/categoria.model';
import { CursoModel } from '../../models/curso.model';
import { ConteudoService } from 'src/app/services/conteudo.service';
import { CursoCreateModel } from '../../models/curso.model';
import { CursosService } from 'src/app/services/cursos.service';
import { CategoriaAddDialogComponent } from '../../categorias/categoria-add-dialog.component';
import { NgxCurrencyDirective } from "ngx-currency";

@Component({
  selector: 'app-curso-edit',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule, MaterialModule, NgxCurrencyDirective],
  templateUrl: './curso-edit.component.html',
  styleUrls: ['./curso-edit.component.scss']
})

export class CursoEditComponent extends FormBaseComponent implements OnInit, OnDestroy {
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
    private dialogRef: MatDialogRef<CursoEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: CursoModel | null) {

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
    this.patchForm();
    this.loadCategorias();
  }

  ngAfterViewInit(): void {
    super.configureValidationFormBase(this.formInputElements, this.form);
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
    this.cursoModel = { ...formValue, validoAte, id: this.data?.id } as CursoCreateModel;
    
    this.cursosService.update(this.data?.id ?? '', this.cursoModel)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (result) => {
          if (!result) {
            this.toastr.error('Erro ao salvar o curso.');
            return;
          }

          this.toastr.success('Curso atualizado com sucesso.');
          this.dialogRef.close({ inserted: true })
        },
        error: (fail) => {
          this.submitted = false;
          const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
          this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao salvar o curso.');
        }
      });
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

  private patchForm(): void {
    if (!this.data) return;

    const curso = this.data;
    this.form.patchValue({
        nome: curso.nome ?? '',
        valor: curso.valor ?? 0,
        duracaoHoras: curso.duracaoHoras ?? 0,
        nivel: curso.nivel ?? '',
        instrutor: curso.instrutor ?? '',
        vagasMaximas: curso.vagasMaximas ?? 0,
        imagemUrl: curso.imagemUrl ?? '',
        validoAte: curso.validoAte ? new Date(curso.validoAte).toISOString().slice(0, 10) : null,
        categoriaId: curso.categoriaId ?? '',
        resumo: curso.resumo ?? '',
        descricao: curso.descricao ?? '',
        objetivos: curso.objetivos ?? '',
        preRequisitos: curso.preRequisitos ?? '',
        publicoAlvo: curso.publicoAlvo ?? '',
        metodologia: curso.metodologia ?? '',
        recursos: curso.recursos ?? '',
        avaliacao: curso.avaliacao ?? '',
        bibliografia: curso.bibliografia ?? '',
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
