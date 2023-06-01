import { Component } from '@angular/core';
import { AzurehttpService } from '../azurehttp.service';

@Component({
  selector: 'app-all-blobs',
  templateUrl: './all-blobs.component.html',
  styleUrls: ['./all-blobs.component.css']
})
export class AllBlobsComponent {

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
  public getContainer(){
    this.svc.getAllContainers().subscribe({
      next: (data) => this.containerList = data,
      error: (data)=> alert(data.error.text),
      complete: () => console.log("container All blobs")
    });
  }

  public getBlobs(){
    this.svc.getAllBlobs(this.containerName).subscribe({
      next: (data) => this.BlobList = data,
      error: (data) => alert(data.error.text),
      complete: () => console.log("blob request completed...")
    });
    
  }

}
