import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppComponent } from './app.component';
import { AllContainersComponent } from './all-containers/all-containers.component';
import { CreateContainerComponent } from './create-container/create-container.component';
import { AllBlobsComponent } from './all-blobs/all-blobs.component';
import { UploadBlobComponent } from './upload-blob/upload-blob.component';
import { DownloadBlobComponent } from './download-blob/download-blob.component';
import { FormsModule } from '@angular/forms';
import { FileSaverModule } from 'ngx-filesaver';
import { DeleteBlobComponent } from './delete-blob/delete-blob.component';
import { DeleteContainerComponent } from './delete-container/delete-container.component';

@NgModule({
  declarations: [
    AppComponent,
    AllContainersComponent,
    CreateContainerComponent,
    AllBlobsComponent,
    UploadBlobComponent,
    DownloadBlobComponent,
    DeleteBlobComponent,
    DeleteContainerComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    FileSaverModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
