import { Component } from '@angular/core';
import { IconSelector } from '../icon-selector/icon-selector';
import { Observable, timer } from 'rxjs';
import { map } from 'rxjs/operators';
import { AsyncPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-clock-nso',
  imports: [IconSelector, AsyncPipe, DatePipe],
  templateUrl: './clock-nso.html',
  styleUrl: './clock-nso.scss',
})
export class ClockNso {
  isPressed = true;

  clock$: Observable<Date> = timer(0, 1000).pipe(map(() => new Date()));

  icon$: Observable<string> = this.clock$.pipe(map((date: Date) => this.getIcon(date.getHours())));

  private getIcon(hour: number): string {
    if (hour > 5 && hour < 12) {
      return 'morning';
    } else if (hour > 12 && hour < 18) {
      return 'noon';
    } else {
      return 'night';
    }
  }
}
