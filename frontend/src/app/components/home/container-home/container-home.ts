import { Component, inject } from '@angular/core';
import { ClockService } from '../../../core/services/clock-service/clock.service';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-container-home',
  imports: [AsyncPipe],
  templateUrl: './container-home.html',
  styleUrl: './container-home.scss',
})
export class ContainerHome {
  private clockService = inject(ClockService);

  icon$ = this.clockService.icon$;
}
