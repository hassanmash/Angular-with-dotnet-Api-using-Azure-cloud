import { Component } from '@angular/core';
import { AzurehttpService } from '../azurehttp.service';
import * as FileSaver from 'file-saver';

@Component({
  selector: 'app-download-blob',
  templateUrl: './download-blob.component.html',
  styleUrls: ['./download-blob.component.css']
})
export class DownloadBlobComponent {

  public containerName: string = "";
  public containerList: string[] = [];
  public BlobList: string[] = [];

  constructor(private svc: AzurehttpService) {
    svc.getAllContainers().subscribe({
      next: (data) => this.containerList = data,
      error: (data)=> alert(data.error.text),
      complete: () => console.log("container All blobs")
    });
  }

  public onChange() {
    this.svc.getAllBlobs(this.containerName).subscribe({
      next: (data) => this.BlobList = data,
      error: (data) => alert(data.error.text),
      complete: () => console.log("completed getting blob to download...")
    });
  }

  public downloadLink(blobName: string) {
    this.svc.downloadBlob(this.containerName,blobName).subscribe({
      next: (data) => {
        console.log(data);
        let blob = new Blob([data], { type: 'application/octet-stream' });
        let file = new File([blob],blobName);
        FileSaver.saveAs(file,blobName);
      },
      error: (data) => {
        console.log(data);
        let blob = new Blob([data.error.text], { type: 'application/octet-stream' });
        let file = new File([blob],blobName);
        FileSaver.saveAs(file,blobName);
        
      },
      complete: () => console.log("download request completed...")      
    });
  }

}
