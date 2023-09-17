import { Component, OnInit, OnDestroy, Renderer2 } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { NgbDatepickerConfig } from '@ng-bootstrap/ng-bootstrap';

import { ListenerService } from '../services';
import { AppUtils } from '../helpers/app.utils';
declare var appConfig: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})

export class AppComponent implements OnInit, OnDestroy {
  subscription: Subscription;
  routerSubscription: Subscription;
  currentUrl: string;
  layoutType = 1;

  constructor(private router: Router,
    private renderer2: Renderer2,
    private appUtils: AppUtils,
    private listenerService: ListenerService,
    datepickerConfig: NgbDatepickerConfig) {

    this.subscription = this.listenerService
      .listenLayout
      .subscribe(() => {
        if (this.appUtils.isUserAuthenticated()) {
          this.renderer2.addClass(document.body, 'kt-aside--enabled');
          this.renderer2.addClass(document.body, 'kt-aside--fixed');
        } else {
          this.renderer2.removeClass(document.body, 'kt-aside--enabled');
          this.renderer2.removeClass(document.body, 'kt-aside--fixed');
        }
      });

    this.routerSubscription = this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {

        setTimeout(() => {
          appConfig.adjustContainerHeight();
          this.appUtils.scrollToTop();
        });

        this.currentUrl = event.url;

        if (this.currentUrl.indexOf('/account/login') !== -1) {
          this.layoutType = 0;
          this.renderer2.removeClass(document.body, 'kt-aside--enabled');
          this.renderer2.removeClass(document.body, 'kt-aside--fixed');
        } else if (this.appUtils.isUserAuthenticated()) {
          this.layoutType = 1;
          this.renderer2.addClass(document.body, 'kt-aside--enabled');
          this.renderer2.addClass(document.body, 'kt-aside--fixed');
        }
      }
    });

    const currentDate = new Date();
    datepickerConfig.minDate = { year: 1900, month: 1, day: 1 };
    datepickerConfig.maxDate = { year: currentDate.getFullYear() + 1, month: currentDate.getMonth() + 1, day: currentDate.getDate() };
    datepickerConfig.outsideDays = 'collapsed';
  }

  ngOnInit() {
    setTimeout(() => {
      appConfig.initDefaults();
    });

    window.addEventListener('storage', (event) => {
      if (event.storageArea === localStorage) {
        const token = localStorage.getItem('AuthToken');
        if (token === null || token === undefined) {
          this.router.navigate(['/account/login']);
        } else {
        }
      }
    }, false);
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
    this.routerSubscription.unsubscribe();
  }
}
