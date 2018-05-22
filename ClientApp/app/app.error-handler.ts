import { ErrorHandler, Injectable, Inject, Injector, NgZone, isDevMode } from "@angular/core";
import { ToastrService } from "ngx-toastr";
import * as Raven from 'raven-js';

@Injectable()
export class AppErrorHandler implements ErrorHandler {
    handleError(error: any): void {
        if (!isDevMode())
            Raven.captureException(error.originalError || error);
            else{
                throw error;
            }
        this.ngZone.run(() => { this.toastrService.error('Exception', 'Some Thing went wrong'); });

    }
    constructor(private ngZone: NgZone, @Inject(Injector) private injector: Injector) {

    }
    private get toastrService(): ToastrService {
        return this.injector.get(ToastrService);
    }

}