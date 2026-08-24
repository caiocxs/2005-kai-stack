import { Component, inject } from '@angular/core';
import { IconSelector } from '../icon-selector/icon-selector';
import { Observable, timer } from 'rxjs';
import { map } from 'rxjs/operators';
import { AsyncPipe, DatePipe } from '@angular/common';
import { ClockService } from '../../../core/services/clock-service/clock.service';

@Component({
  selector: 'app-clock-nso',
  imports: [IconSelector, AsyncPipe, DatePipe],
  templateUrl: './clock-nso.html',
  styleUrl: './clock-nso.scss',
})
export class ClockNso {
  isPressed = true;

  private clockService = inject(ClockService);

  clock$ = this.clockService.clock$;
  icon$ = this.clockService.icon$;
}
