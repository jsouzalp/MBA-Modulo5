import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatriculasService } from '../../../services/matriculas.service';
import { MatriculaModel } from '../../../models/matricula.model';
import { SolicitarCertificadoRequest } from '../../../models/certificado.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  standalone: true,
  selector: 'app-matriculas',
  templateUrl: './matriculas.component.html',
  styleUrls: ['./matriculas.component.css'],
  imports: [CommonModule, MatTableModule, MatButtonModule, MatProgressBarModule]
})
export class MatriculasComponent {
  cols = ['curso', 'status', 'progresso', 'acoes'];
  matriculas: MatriculaModel[] = [];

  constructor(private service: MatriculasService, private toastr: ToastrService) { }

  ngOnInit() {
    if (!this.service.LocalStorage.isUserAdmin()) {
      this.load();
    }
  }

  load() {
    this.service.listarMatriculas().subscribe({
      next: d => this.matriculas = d,
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) this.toastr.error(errors.join('\n'));
        else this.toastr.error('Falha ao carregar suas matrículas.');
      }
    });
  }

  solicitarCertificado(m: MatriculaModel) {
    const request: SolicitarCertificadoRequest = {
      alunoId: m.alunoId,
      matriculaCursoId: m.id
    };

    this.service.solicitarCertificado(request).subscribe({
      next: () => {
        this.toastr.success('Certificado solicitado com sucesso!');
        this.load(); 
      },
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) this.toastr.error(errors.join('\n'));
        else this.toastr.error('Não foi possível solicitar o certificado.');
      }
    });
  }

  finalizar(m: MatriculaModel) {
    this.service.concluirCurso(m.alunoId, m.id).subscribe({
      next: () => {
        this.toastr.success('Curso finalizado com sucesso.');
        this.load();
      },
      error: (fail) => {
        const errors = (fail?.error?.errors ?? fail?.errors ?? []) as string[];
        if (Array.isArray(errors) && errors.length > 0) this.toastr.error(errors.join('\n'));
        else this.toastr.error('Não foi possível finalizar o curso.');
      }
    });
  }
}


