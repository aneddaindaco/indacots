import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { PoiActions, PoiSelectors } from '@indaco/poi';
import { Store } from '@ngrx/store';
import { Subscription } from 'rxjs';
import { AdminService } from './admin.service';
import { ChartConfiguration, ChartData, ChartType } from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';

@Component({
  selector: 'indaco-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css'],
})
export class AdminComponent implements OnInit, OnDestroy {
  private subscription: Subscription | undefined;
  public pieChartType: ChartType = 'pie';
  @ViewChild(BaseChartDirective) chart: BaseChartDirective | undefined;

  public pieChartData: ChartData<'pie', number[], string | string[]> = {
    labels: ['Download Sales', 'In Store Sales', 'Mail Sales'],
    datasets: [
      {
        data: [300, 500, 100],
      },
    ],
  };

  public pieChartOptions: ChartConfiguration['options'] = {
    responsive: true, 
    maintainAspectRatio: false,
    plugins: {
      legend: {
        display: true,
        position: 'top',
      },
      /*datalabels: {
        formatter: (value, ctx) => {
          if (ctx.chart.data.labels) {
            return ctx.chart.data.labels[ctx.dataIndex];
          }
        },
      },*/
    },
  };

  constructor(private store: Store, private adminService: AdminService) {}

  ngOnInit(): void {
    this.subscription = this.store
      .select(PoiSelectors.getAllPoi)
      .subscribe((pois) => {
        this.pieChartData.labels = pois.map(poi =>poi.name)
        this.pieChartData.datasets = [{data: this.adminService.getStatistics(pois) }] 
        this.chart?.update()
      });
    this.store.dispatch(PoiActions.init());

    //this.chart?.update()
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
