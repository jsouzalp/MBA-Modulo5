import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { MaterialModule } from 'src/app/material.module';
import { AulaModel } from 'src/app/pages/conteudo/models/aula.model';
import { CursoModel } from '../../models/curso.model';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { AulaEditDialogComponent } from '../edit/aula-edit-dialog.component';
import { AulaAddDialogComponent } from '../add/aula-add-dialog.component';
import { CursosService } from '../../../../services/cursos.service';
import { MatriculasService } from '../../../../services/matriculas.service';

interface DialogData {
  curso: CursoModel;
  matriculaId?: string;
  pagamentoRealizado?: boolean;
}

@Component({
  selector: 'app-aulas-list-dialog',
  standalone: true,
  imports: [CommonModule, MatDialogModule, MaterialModule],
  template: `
    <h3 mat-dialog-title>Aulas de {{ data.curso.nome }}</h3>
    <mat-dialog-content class="content">
      <ng-container *ngIf="aulas?.length; else empty">
        <div class="aula" *ngFor="let a of aulas; let i = index">
          <div class="aula-header">
            <span class="ordem">#{{ a.numero }}</span>
            <span class="nome">{{ a.nome }}</span>
            <span class="duracao" *ngIf="a.duracaoMinutos !== undefined">
              <span class="iconify" data-icon="mdi:clock-outline" data-width="18" data-height="18"></span>
              {{ a.duracaoMinutos }} min
            </span>
          </div>
          <div class="descricao" *ngIf="a.descricao">{{ a.descricao }}</div>
          <div class="meta">
            <span *ngIf="a.videoUrl && !!data.matriculaId && data.pagamentoRealizado">
              <span class="iconify" data-icon="mdi:play-circle-outline" data-width="18" data-height="18"></span>
              <button mat-button mat-raised-button color="primary" (click)="assistirAula(a)" [disabled]="a.aulaRealizada">Assistir Aula</button>
            </span>
            <span *ngIf="a.videoUrl && !!data.matriculaId && data.pagamentoRealizado" [ngClass]="{
              'status-pendente': !a.aulaRealizada,
              'status-concluida': a.aulaRealizada
            }">
              {{ a.aulaRealizada ? 'Concluída' : 'Pendente' }}
            </span>
          </div>
          <div class="actions bottom" *ngIf="isUserAdmin">
            <button mat-stroked-button color="primary" (click)="editarAula(a)">editar</button>
          </div>
        </div>
      </ng-container>
      <ng-template #empty>
        <div class="empty-state">
          <p>Nenhuma aula cadastrada para este curso.</p>
          <button mat-raised-button color="primary" (click)="adicionarAula()">Adicionar Aula</button>
        </div>
      </ng-template>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-raised-button color="primary" [disabled]="cursoFinalizado" (click)="finalizarCurso()" *ngIf="!isUserAdmin && concluiuCurso()">
        <mat-icon>{{ cursoFinalizado ? 'check_circle' : 'school' }}</mat-icon>
        {{ cursoFinalizado ? 'Curso Concluído' : 'Concluir Curso' }}
      </button>
      <button mat-raised-button mat-dialog-close color="secondary">Fechar</button>
    </mat-dialog-actions>
  `,
  styles: [
    `.content{max-height:70vh;min-width:300px;display:block}`,
    `.aula{padding:8px 0;border-bottom:1px solid rgba(0,0,0,.08)}`,
    `.aula:last-child{border-bottom:none}`,
    `.aula-header{display:flex;gap:12px;align-items:center;justify-content:space-between;flex-wrap:wrap}`,
    `.ordem{font-weight:600;color:#1976d2}`,
    `.nome{font-weight:600;flex:1 1 auto;font-size:1.05rem}`,
    `.duracao{color:#555;display:flex;align-items:center;gap:4px}`,
    `.descricao{margin:6px 0;font-size:.98rem;color:#444}`,
    `.meta{display:flex;gap:16px;color:#555;margin-top:4px;align-items:center}`,
    `.meta span{display:flex;align-items:center;gap:6px}`,
    `.meta a{display:inline-flex;align-items:center;text-decoration:none;color:#007ACC}`,
    `.status-pendente{color:#f57c00;font-weight:500}`,
    `.status-concluida{color:#388e3c;font-weight:500}`,
    `.actions{margin-top:12px}`,
    `.actions.bottom{align-self:flex-start}`
  ]
})
export class AulasListDialogComponent implements OnInit {
  aulas: (AulaModel & { status?: string })[] = [];
  isUserAdmin = false;
  userId: string | null = null;
  cursoFinalizado = false;

  constructor(
    @Inject(MAT_DIALOG_DATA) public data: DialogData,
    private dialogRef: MatDialogRef<AulasListDialogComponent>,
    private dialog: MatDialog,
    private cursosService: CursosService,
    private matriculaService: MatriculasService,
    private toastr: ToastrService
  ) {
    this.aulas = (data.curso.aulas || []) as (AulaModel & { status?: string })[];
    this.isUserAdmin = new LocalStorageUtils().isUserAdmin();
    this.userId = new LocalStorageUtils().getUser()?.usuarioToken?.id;
  }

  ngOnInit(): void {
    this.loadAulas(this.data.curso.id);
    this.loadAulasByMatricula();
    this.loadMatricula();
  }

  close(): void {
    this.dialogRef.close();
  }

  adicionarAula(): void {
    const ref = this.dialog.open(AulaAddDialogComponent, {
      width: '720px',
      maxWidth: '95vw',
      data: { cursoId: this.data.curso.id }
    });

    ref.afterClosed().subscribe(result => {
      if (result?.createdCount) {
        this.loadAulas(this.data.curso.id);
      }
    });
  }

  editarAula(aula: AulaModel): void {
    const ref = this.dialog.open(AulaEditDialogComponent, {
      width: '720px',
      maxWidth: '95vw',
      disableClose: true,
      autoFocus: false,
      data: { cursoId: aula.cursoId, cursoNome: this.data.curso.nome, aula }
    });
    
    ref.afterClosed().subscribe(result => {
      if (result?.updated) {
        this.loadAulas(this.data.curso.id);
      }
    });
  }

  assistirAula(aula: AulaModel): void {
    if (!this.data.matriculaId || !this.userId || !aula.id || this.isUserAdmin)
      return;

   this.matriculaService.registrarHistoricoAprendizado(this.data.matriculaId, this.userId, aula.id, new Date().toISOString()).subscribe({
     next: () => {
      this.toastr.success('Progresso atualizado com sucesso!');
      this.loadAulasByMatricula();
     },
     error: (err) => {
       const errors = this.extractErrors(err);
       this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao atualizar progresso.');
     }
   });
  }

  concluiuCurso(): boolean {
    return this.aulas.every(a => a.aulaRealizada);
  }

  finalizarCurso(): void {
    if (!this.data.matriculaId || !this.userId || this.isUserAdmin)
      return;

    this.matriculaService.concluirCurso(this.userId, this.data.matriculaId).subscribe({
      next: () => {
        this.toastr.success('Curso concluído com sucesso!');
        this.loadAulasByMatricula();
        this.cursoFinalizado = true;
      },
      error: (err) => {
        const errors = this.extractErrors(err);
        this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao concluir curso.');
      }
    });
  }

  private loadAulas(cursoId: string): void {
    if (!this.isUserAdmin)
      return;

    this.cursosService.getAulasByCurso(cursoId).subscribe({
      next: ({ aulas }) => this.aulas = aulas,
      error: (err) => {
        const errors = this.extractErrors(err);
        this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao carregar aulas.');
      }
    });
  }

  private loadAulasByMatricula(): void {
    if (!this.data.matriculaId || this.isUserAdmin)
      return;

    this.matriculaService.obterAulasPorMatricula(this.data.matriculaId).subscribe({
      next: (aulasDto) => {
        this.aulas = (this.data.curso.aulas || []).map(aula => {
          const aulaDto = aulasDto.find(ad => ad.aulaId === aula.id);
          return {
            id: aulaDto?.aulaId,
            cursoId: aulaDto?.cursoId,
            nome: aulaDto?.nomeAula,
            numero: aula.numero,
            dataInicio: aulaDto?.dataInicio,
            dataTermino: aulaDto?.dataTermino,
            aulaRealizada: aulaDto?.aulaJaIniciadaRealizada,
            videoUrl: aula.videoUrl,
            descricao: aula.descricao,
            duracaoMinutos: aula.duracaoMinutos,
            tipoAula: aula.tipoAula,
            status: aula?.status
          } as AulaModel;
        });
      },
      error: (err) => {
        const errors = this.extractErrors(err);
        this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao carregar aulas.');
      }
    });
  }

  private loadMatricula(): void {
    if (!this.data.matriculaId || this.isUserAdmin)
      return;

    this.matriculaService.obterMatricula(this.data.matriculaId).subscribe({
      next: ({dataConclusao}) => {
        this.cursoFinalizado = !!dataConclusao;
      },
      error: (err) => {
        const errors = this.extractErrors(err);
        this.toastr.error(Array.isArray(errors) ? errors.join('\n') : 'Erro ao carregar matrícula.');
      }
    });
  }

  private extractErrors(err: any): string[] {
    return (err?.error?.errors ?? err?.errors ?? []) as string[];
  }
}
