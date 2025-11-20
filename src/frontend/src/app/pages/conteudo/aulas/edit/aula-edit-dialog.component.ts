import { CommonModule } from '@angular/common';
import { Component, Inject } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogActions, MatDialogContent, MatDialogTitle, MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from 'src/app/material.module';
import { ToastrService } from 'ngx-toastr';
import { CursosService } from 'src/app/services/cursos.service';
import { AulaEditModel } from 'src/app/pages/conteudo/models/aula.model';

interface DialogData {
  cursoId: string;
  cursoNome?: string;
  aula?: AulaEditModel;
}

@Component({
  selector: 'app-aula-edit-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MaterialModule, MatDialogTitle, MatDialogContent, MatDialogActions],
  templateUrl: './aula-edit-dialog.component.html',
  styleUrls: ['./aula-edit-dialog.component.scss']
})
export class AulaEditDialogComponent {
  form: FormGroup = new FormGroup({
    cursoId: new FormControl(this.data.cursoId),
    nome: new FormControl('', [Validators.required, Validators.minLength(3)]),
    descricao: new FormControl('', [Validators.required, Validators.minLength(3)]),
    duracaoMinutos: new FormControl(0, [Validators.required, Validators.min(1)]),
    videoUrl: new FormControl('', [Validators.required]),
    tipoAula: new FormControl('', [Validators.required]),
    numero: new FormControl(0, [Validators.required, Validators.min(1)])
  });

  saving = false;
  updated = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private dialogRef: MatDialogRef<AulaEditDialogComponent>,
    private cursosService: CursosService,
    private toastr: ToastrService
  ) { }

  ngOnInit(): void {
    const aula = this.data?.aula;
    if (aula) {
      this.patchValueForEdit(aula);
    }
  }

  private patchValueForEdit(aula: AulaEditModel): void {
    this.form.patchValue({
      cursoId: aula.cursoId ?? '',
      nome: aula.nome ?? '',
      descricao: aula.descricao ?? '',
      duracaoMinutos: aula.duracaoMinutos ?? 0,
      videoUrl: aula.videoUrl ?? '',
      tipoAula: aula.tipoAula ?? '',
      numero: aula.numero ?? 0
    });
  }

  private buildPayload(): AulaEditModel {
    const v = this.form.value;
    return {
      id: this.data.aula?.id,
      cursoId: this.data.cursoId,
      nome: v.nome,
      descricao: v.descricao,
      duracaoMinutos: Number(v.duracaoMinutos ?? 0),
      videoUrl: v.videoUrl,
      tipoAula: v.tipoAula,
      numero: Number(v.numero ?? 0)
    } as AulaEditModel;
  }

  save(): void {
    if (this.form.invalid || this.saving) {
      this.form.markAllAsTouched();
      return;
    }

    const payload = this.buildPayload();
    this.saving = true;

    if (!this.data.aula || !this.data.aula.id) {
      this.toastr.error('Dados da aula não encontrados para edição.');
      this.saving = false;
      return;
    }

    this.cursosService.updateAula(this.data.cursoId, this.data.aula.id, payload).subscribe({
      next: () => {
        this.toastr.success('Aula atualizada com sucesso.');
        this.updated = true;
        this.dialogRef.close({ updated: this.updated });
      },
      error: (e) => {
        const errors = (e?.error?.errors ?? e?.errors ?? []) as string[];
        this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao salvar a aula.');
        this.saving = false;
      },
      complete: () => (this.saving = false)
    });
  }

  close(): void {
    this.dialogRef.close({ updated: this.updated });
  }
}


