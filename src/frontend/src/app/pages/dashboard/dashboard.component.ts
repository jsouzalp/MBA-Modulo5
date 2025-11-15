import { Component, ViewEncapsulation } from '@angular/core';
import { MaterialModule } from '../../material.module';
import { CommonModule } from '@angular/common';
import { DashboardService } from '../../services/dashboard.service';
import { DashboardAlunoModel, ProgressoGeralModel } from '../../models/dashboard-aluno.model';
import { DashboardAdminModel } from '../../models/dashboard-admin.model';
import { NgApexchartsModule } from 'ng-apexcharts';
import { ApexNonAxisChartSeries, ApexChart, ApexPlotOptions, ApexStroke, ApexFill } from 'ng-apexcharts';
import { ToastrService } from 'ngx-toastr';
import { LocalStorageUtils } from '../../utils/localstorage';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    MaterialModule,
    CommonModule,
    NgApexchartsModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  encapsulation: ViewEncapsulation.None,
})
export class DashboardComponent {
  progressoGeral?: ProgressoGeralModel;
  dashboardAdmin?: DashboardAdminModel;
  isAdmin = false;
  carregando = false;

  // Configuração do gráfico radial (percentual concluído)
  chartSeries: ApexNonAxisChartSeries = [0];
  chartOptions: { chart: ApexChart; plotOptions: ApexPlotOptions; fill: ApexFill; stroke: ApexStroke; labels: string[] } = {
    chart: { type: 'radialBar', height: 320 },
    plotOptions: {
      radialBar: {
        hollow: { size: '60%' },
        track: { background: '#efefef' },
        dataLabels: {
          name: { show: true, color: '#666', offsetY: -10 },
          value: { show: true, fontSize: '28px', formatter: (val: number) => `${Math.round(val)}%` }
        }
      }
    },
    fill: { type: 'gradient', gradient: { shade: 'light', gradientToColors: ['#4caf50'], stops: [0, 100] } },
    stroke: { lineCap: 'round' },
    labels: ['Concluído']
  };

  constructor(
    private dashboard: DashboardService, 
    private toastr: ToastrService,
    private localStorageUtils: LocalStorageUtils
  ) { }

  ngOnInit() {
    this.isAdmin = this.localStorageUtils.isUserAdmin();
    this.carregando = true;

    if (this.isAdmin) {
      this.carregarDashboardAdmin();
    } else {
      this.carregarDashboardAluno();
    }
  }

  private carregarDashboardAdmin() {
    this.dashboard.getDashboardAdmin().subscribe({
      next: (data: DashboardAdminModel) => {
        this.dashboardAdmin = data;
      },
      error: (error) => {
        this.processFailAdmin(error, this.toastr);
      },
      complete: () => this.carregando = false
    });
  }

  private carregarDashboardAluno() {
    this.dashboard.getDashboardAluno().subscribe({
      next: (data: DashboardAlunoModel) => {
        this.progressoGeral = data?.progressoGeral ?? {
          cursosMatriculados: 0,
          cursosConcluidos: 0,
          certificadosEmitidos: 0,
          percentualConcluidoGeral: 0,
          horasEstudadas: 0
        };

        const percentual = Number(this.progressoGeral.percentualConcluidoGeral ?? 0);
        this.chartSeries = [isNaN(percentual) ? 0 : Math.max(0, Math.min(100, percentual))];
      },
      error: (error) => {
        this.processFail(error, this.toastr);
      },
      complete: () => this.carregando = false
    });
  }

  protected processFail(fail: any, toastr: ToastrService): void {
    const error = (Array.isArray(fail?.error?.errors) ? fail.error.errors.join('\n') : fail.error.errors) || (fail?.error?.message || fail?.message || 'Erro desconhecido');
    toastr.error(error);
    this.progressoGeral = {
        cursosMatriculados: 0,
        cursosConcluidos: 0,
        certificadosEmitidos: 0,
        percentualConcluidoGeral: 0,
        horasEstudadas: 0
    };
    this.chartSeries = [0];
  }

  protected processFailAdmin(fail: any, toastr: ToastrService): void {
    const error = (Array.isArray(fail?.error?.errors) ? fail.error.errors.join('\n') : fail.error.errors) || (fail?.error?.message || fail?.message || 'Erro desconhecido');
    toastr.error(error);
    this.dashboardAdmin = undefined;
  }

  // Método para traduzir status das vendas
  protected traduzirStatus(status: string): string {
    const statusMap: { [key: string]: string } = {
      'Approved': 'Aprovado',
      'Processing': 'Processando',
      'Cancelled': 'Cancelado',
      'Pending': 'Pendente',
      'Completed': 'Concluído',
      'Failed': 'Falhou',
      'Refunded': 'Reembolsado',
      'Aprovado': 'Aprovado',
      'Processando': 'Processando',
      'Cancelado': 'Cancelado',
      'Pendente': 'Pendente',
      'Concluído': 'Concluído',
      'Falhou': 'Falhou',
      'Reembolsado': 'Reembolsado'
    };
    
    return statusMap[status] || status;
  }
}
