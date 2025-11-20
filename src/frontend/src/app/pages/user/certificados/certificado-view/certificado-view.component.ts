import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, Router } from '@angular/router';
import { CertificadoModel } from '../../../../models/certificado.model';

@Component({
  standalone: true,
  selector: 'app-certificado-view',
  templateUrl: './certificado-view.component.html',
  styleUrls: ['./certificado-view.component.css'],
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule]
})
export class CertificadoViewComponent implements OnInit {
  certificado: CertificadoModel | null = null;
  qrCodeData: string = '';

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    const state = history.state;
    if (state && state.certificado) {
      this.certificado = state.certificado;
      if (this.certificado) {
        this.qrCodeData = `CERT-${this.certificado.id}`;
      }
    } else {
      // Se não houver state, redireciona de volta
      this.router.navigate(['/pages/certificados']);
    }
  }

  voltar() {
    this.router.navigate(['/pages/certificados']);
  }
}
