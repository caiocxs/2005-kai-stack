import { Component } from '@angular/core';
import { ButtonBarNso } from '../../atoms/button-bar-nso/button-bar-nso';
import { PipeBarNso } from '../../atoms/pipe-bar-nso/pipe-bar-nso';
import { ClockNso } from '../../atoms/clock-nso/clock-nso';
import { CalendarNso } from '../../atoms/calendar-nso/calendar-nso';
import { RouteBarNso } from '../../atoms/route-bar-nso/route-bar-nso';

@Component({
  selector: 'app-footer-home',
  imports: [ButtonBarNso, PipeBarNso, ClockNso, CalendarNso, RouteBarNso],
  templateUrl: './footer-home.html',
  styleUrl: './footer-home.scss',
})
export class FooterHome {}
