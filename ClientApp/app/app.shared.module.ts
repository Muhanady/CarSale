import { AuthService } from './services/auth.service';
import { BrowserXhr } from '@angular/http';
import { ProgressService, BrowserXhrWithProgress } from './services/progress.service';
import { PhotoService } from './services/photo.service';
import { AppErrorHandler } from './app.error-handler';
import { NgModule, ErrorHandler } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


import { MakeService } from './services/make.service';
import { VehicleService } from './services/vehicle.service';


import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { FetchDataComponent } from './components/fetchdata/fetchdata.component';
import { CounterComponent } from './components/counter/counter.component';
import { VehicleFormComponent } from './components/vehicle-form/vehicle-form.component';
import { ToastrModule, ToastNoAnimation, ToastNoAnimationModule } from 'ngx-toastr';
import * as Raven from 'raven-js';
import { VehiclesComponent } from './components/vehicles/vehicles.component';
import { ViewVehicleComponent } from './components/view-vehicle/view-vehicle.component';

Raven
    .config('https://edea48e4f3ef4252bd75cc81c1b268c4@sentry.io/1208445')
    .install();
@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        CounterComponent,
        FetchDataComponent,
        HomeComponent,
        VehicleFormComponent,
        VehiclesComponent,
        ViewVehicleComponent
    ],
    imports: [
        BrowserAnimationsModule,
        ToastrModule.forRoot({
            timeOut: 3000,
            positionClass: 'toast-top-right',
            preventDuplicates: false

        }),
        CommonModule,
        HttpClientModule,
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'vehicles', pathMatch: 'full' },
            { path: 'callback', component: VehiclesComponent },
            { path: 'vehicles', component: VehiclesComponent },
            { path: 'vehicle/view/:id', component: ViewVehicleComponent },
            { path: 'vehicle/new', component: VehicleFormComponent },
            { path: 'vehicle/edit/:id', component: VehicleFormComponent },

            { path: 'home', component: HomeComponent },
            { path: 'counter', component: CounterComponent },
            { path: 'fetch-data', component: FetchDataComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        { provide: ErrorHandler, useClass: AppErrorHandler },
        { provide: BrowserXhr, useClass: BrowserXhrWithProgress },
        MakeService,
        VehicleService,
        PhotoService,
        ProgressService,
        AuthService
        
    ]
})
export class AppModuleShared {
}
