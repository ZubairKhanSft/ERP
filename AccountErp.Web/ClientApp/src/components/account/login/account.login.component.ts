import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { BlockUI, NgBlockUI } from 'ng-block-ui';
import { LoginModel } from '../../../models';
import { ListenerService, AccountService } from '../../../services';
import { AppUtils } from '../../../helpers';

@Component({
    selector: 'app-login',
    templateUrl: './account.login.component.html',
    styleUrls: ['./account.login.component.css']
})

export class LoginComponent {
    @BlockUI('container-blockui') blockUI: NgBlockUI;

    returnUrl: string;
    model: LoginModel = new LoginModel();


    constructor(private router: Router,
        private route: ActivatedRoute,
        private toastr: ToastrService,
        private appUtils: AppUtils,
        private listenerService: ListenerService,
        private accountService: AccountService) {

        if (this.appUtils.isUserAuthenticated()) {
          this.router.navigate(['/calendar']);
            return;
        }

        this.route.queryParams.subscribe(params => {
            this.returnUrl = params['returnUrl'];
        });

        // this.model.userName = 'admin';
        // this.model.password = 'Password@123';
    }

    login() {
        this.blockUI.start();
        this.accountService.login(this.model).subscribe(
            (data: any) => {
                this.blockUI.stop();

                setTimeout(() => {
                    localStorage.setItem('AuthToken', data.toString());

                    this.listenerService.topNavlistner.next();
                    this.listenerService.layoutlistner.next();

                    if (this.returnUrl) {
                        this.router.navigate([this.returnUrl]);
                    } else {
                      this.router.navigate(['/calendar']);
                    }
                }, 100);

            },
            error => {
                this.blockUI.stop();
                this.appUtils.ProcessErrorResponse(this.toastr, error);
            });
    }

}
