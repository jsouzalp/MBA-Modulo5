import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { MaterialModule } from 'src/app/material.module';
import { ConteudoService } from 'src/app/services/conteudo.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-categoria-add-dialog',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, MaterialModule],
  template: `
    <h3 mat-dialog-title>Cadastrar nova categoria</h3>
    <mat-dialog-content>
      <form [formGroup]="form" (ngSubmit)="submit()" class="category-form">
        <div class="form-grid">
          <mat-form-field appearance="outline" floatLabel="always" class="col-12 nome-field">
            <mat-label>Nome</mat-label>
            <input matInput formControlName="nome" maxlength="100" />
            <mat-error *ngIf="form.get('nome')?.hasError('required')">Informe o nome.</mat-error>
          </mat-form-field>

          <mat-form-field appearance="outline" floatLabel="always" class="col-12">
            <mat-label>Descrição</mat-label>
            <textarea matInput rows="2" formControlName="descricao" maxlength="200"></textarea>
          </mat-form-field>

          <div class="row-color">
            <mat-form-field appearance="outline" floatLabel="always">
              <mat-label>Cor</mat-label>
              <input matInput formControlName="cor" placeholder="#2196F3" />
            </mat-form-field>

            <button mat-icon-button type="button" aria-label="Selecionar cor" class="palette-btn" (click)="colorInput.click()">
              <mat-icon>palette</mat-icon>
            </button>
            <input #colorInput type="color" class="color-overlay" [value]="form.value.cor || '#2196F3'" (input)="onPickColor($event)" />

            <mat-form-field appearance="outline" floatLabel="always">
              <mat-label>Ícone URL</mat-label>
              <input matInput formControlName="iconeUrl" placeholder="https://..." />
            </mat-form-field>
          </div>

          <mat-form-field appearance="outline" floatLabel="always" class="col-12">
            <mat-label>Ordem</mat-label>
            <input matInput type="number" formControlName="ordem" min="0" />
          </mat-form-field>
        </div>
      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button type="button" mat-raised-button (click)="close(false)">Cancelar</button>
      <button type="submit" mat-raised-button color="primary" [disabled]="form.invalid || submitted" (click)="submit()">
        <mat-icon class="rotating" *ngIf="submitted">loop</mat-icon>
        Salvar
      </button>
    </mat-dialog-actions>
  `,
  styles: [
    `.form-grid{display:grid;grid-template-columns:1fr;gap:16px}`,
    `.nome-field{margin-top:8px}`,
    `.row-color{display:grid;grid-template-columns:1fr auto 1fr;gap:16px;align-items:center}`,
    `.palette-btn{width:48px;height:48px;align-self:start}`,
    `.color-overlay{position:absolute;opacity:0;width:0;height:0}`,
    `mat-form-field{padding:0px}`
  ]
})
export class CategoriaAddDialogComponent {
  form = new FormGroup({
    nome: new FormControl('', [Validators.required, Validators.minLength(2)]),
    descricao: new FormControl<string>(''),
    cor: new FormControl<string>(''),
    iconeUrl: new FormControl<string>(''),
    ordem: new FormControl<number>(0, [Validators.min(0)])
  });
  submitted = false;

  constructor(
    private dialogRef: MatDialogRef<CategoriaAddDialogComponent>,
    private conteudoService: ConteudoService,
    private toastr: ToastrService,
  ) {}

  close(inserted: boolean) {
    this.dialogRef.close({ inserted });
  }

  submit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.submitted = true;
    const payload = {
      nome: this.form.value.nome!,
      descricao: this.form.value.descricao ?? '',
      cor: this.form.value.cor ?? '',
      iconeUrl: this.form.value.iconeUrl ?? '',
      ordem: this.form.value.ordem ?? 0
    };

    this.conteudoService.createCategoryApi(payload).subscribe({
      next: (result) => {
        if (!result) {
          this.toastr.error('Erro ao salvar a categoria.');
          this.submitted = false;
          return;
        }
        this.toastr.success('Categoria criada com sucesso.');
        this.close(true);
      },
      error: (fail) => {
        this.submitted = false;
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao salvar a categoria.');
      }
    });
  }

  onPickColor(ev: Event) {
    const input = ev.target as HTMLInputElement;
    if (input?.value) {
      this.form.patchValue({ cor: input.value });
    }
  }
}


