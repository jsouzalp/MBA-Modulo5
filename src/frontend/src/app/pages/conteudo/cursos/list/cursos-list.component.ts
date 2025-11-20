import { FilterCurso, PagedResult } from '../../../../models/paged-result.model';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { CursosService } from '../../../../services/cursos.service';
import { CursoModel } from '../../models/curso.model';
import { MessageService } from 'src/app/services/message.service ';
import { ToastrService } from 'ngx-toastr';
import { BreakpointObserver, LayoutModule } from '@angular/cdk/layout';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ConteudoAddComponent } from '../add/conteudo-add.component';
import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { CursoEditComponent } from '../edit/curso-edit.component';
import { AulasListDialogComponent } from '../../aulas/list/aulas-list-dialog.component';
import { MaterialModule } from "src/app/material.module";
import { MatriculaAddComponent } from '../../../user/matriculas/add/matricula-add.component';
import { MatriculasService } from '../../../../services/matriculas.service';

@Component({
  standalone: true,
  selector: 'app-cursos-list',
  templateUrl: './cursos-list.component.html',
  styleUrls: ['./cursos-list.component.scss'],
  imports: [CommonModule, RouterModule, LayoutModule, MatDialogModule, MaterialModule]
})
export class CursosListComponent {
  gridCols = 2;
  isUserAdmin = false;
  pagedResult: PagedResult<CursoModel> = {
    items: [],
    pageSize: 0,
    pageIndex: 0,
    totalResults: 0
  };
  filterCurso: FilterCurso = {
    pageSize: 6,
    pageIndex: 0,
    query: '',
    includeAulas: true,
    ativos: true
  };
  loading = false;
  userId = new LocalStorageUtils().getUser()?.usuarioToken?.id;
  matriculasAluno: any[] = [];

  constructor(
    private cursosService: CursosService,
    private messageService: MessageService,
    private toastr: ToastrService,
    private breakpointObserver: BreakpointObserver,
    private dialog: MatDialog,
    private matriculas: MatriculasService,
  ) { }

  ngOnInit() {
    this.isUserAdmin = new LocalStorageUtils().isUserAdmin();
    this.breakpointObserver
      .observe(['(max-width: 992px)'])
      .subscribe(state => this.gridCols = state.matches ? 1 : 2);
    this.loadCursos();

    if (!this.isUserAdmin) {
      this.loadMatriculas();
    }
  }

  private loadCursos(): void {
    this.loading = true;
    this.cursosService.listar(this.filterCurso).subscribe({
      next: d => {
        this.pagedResult = d;
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        const errors = (err?.error?.errors ?? err?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) {
          errors.forEach(message => this.messageService.setMessage('Erro', message));
          this.toastr.error(errors.join('\n'));
        } else {
          this.messageService.setMessage('Erro', 'Falha ao carregar a lista de cursos. Tente novamente.');
          this.toastr.error('Falha ao carregar a lista de cursos. Tente novamente.');
        }
      }
    });
  }

  private loadMatriculas(): void {
    this.matriculas.listarMatriculas().subscribe({
      next: (matriculas) => {
        this.matriculasAluno = matriculas;
      },
      error: (err) => {
        console.error('Erro ao carregar matrículas do aluno:', err);
        this.matriculasAluno = [];
      }
    });
  }

  openAddDialog(): void {
    const ref = this.dialog.open(ConteudoAddComponent, {
      width: '1100px',
      maxWidth: '95vw',
      panelClass: ['dialog-fullwidth'],
      disableClose: true,
      autoFocus: false
    });

    ref.afterClosed().subscribe(result => {
      if (result?.inserted) {
        this.loadCursos();
      }
    });
  }

  openEditDialog(curso: CursoModel): void {
    const ref = this.dialog.open(CursoEditComponent, {
      width: '1000px',
      maxWidth: '100vw',
      panelClass: ['dialog-fullwidth'],
      disableClose: true,
      autoFocus: false,
      data: curso
    });

    ref.afterClosed().subscribe(result => {
      if (result?.inserted) {
        this.loadCursos();
      }
    });
  }

  openAulasDialog(curso: CursoModel): void {
    this.dialog.open(AulasListDialogComponent, {
      width: '800px',
      maxWidth: '95vw',
      data: { curso, matriculaId: this.findMatricula(curso.id)?.id, pagamentoRealizado: this.pagamentoRealizado(curso.id) },
    });
  }

  hasMatricula(cursoId: string): boolean {
    return !!this.findMatricula(cursoId);
  }

  findMatricula(cursoId: string) {
    return this.matriculasAluno.find(m => m.cursoId === cursoId && m.alunoId === this.userId);
  }

  pagamentoRealizado(cursoId: string): boolean {
    const matricula = this.findMatricula(cursoId);
    return matricula ? !matricula.pagamentoPodeSerRealizado : false;
  }

  matricular(c: CursoModel) {
    const matricula = this.findMatricula(c.id);
    const pagamentoPodeSerRealizado = matricula ? matricula.pagamentoPodeSerRealizado : true;

    if (matricula && !pagamentoPodeSerRealizado) {
      this.toastr.warning('Pagamento já realizado ou não pode ser feito novamente para este curso.');
      return;
    }

    const ref = this.dialog.open(MatriculaAddComponent, {
      width: '600px',
      maxWidth: '95vw',
      data: {
        curso: c,
        alunoId: this.userId,
        matriculaId: matricula?.id,
        pagamentoPodeSerRealizado
      }
    });

    ref.afterClosed().subscribe(() => {
        this.loadMatriculas();
        this.loadCursos();
    });
  }

  onPageChange(event: any) {
    this.filterCurso.pageSize = event.pageSize;
    this.filterCurso.pageIndex = event.pageIndex + 1;
    this.loadCursos();
  }
}


