import { Component } from '@angular/core';
import { Observable, timer } from 'rxjs';
import { map } from 'rxjs/operators';
import { AsyncPipe, DatePipe } from '@angular/common';

@Component({
  selector: 'app-calendar-nso',
  imports: [AsyncPipe, DatePipe],
  templateUrl: './calendar-nso.html',
  styleUrl: './calendar-nso.scss',
})
export class CalendarNso {
  isPressed = false;

  date$: Observable<Date> = timer(0, 1000).pipe(map(() => new Date()));
}
