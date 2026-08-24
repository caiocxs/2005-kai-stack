import { Injectable } from '@angular/core';
import { Observable, timer, map, shareReplay } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ClockService {
  public clock$: Observable<Date> = timer(0, 1000).pipe(
    map(() => new Date()),
    shareReplay(1),
  );

  public icon$: Observable<string> = this.clock$.pipe(
    map((date: Date) => this.getIcon(date.getHours())),
    shareReplay(1),
  );

  private getIcon(hour: number): string {
    if (hour >= 5 && hour < 12) {
      return 'morning';
    } else if (hour >= 12 && hour < 18) {
      return 'noon';
    } else {
      return 'night';
    }
  }
}
