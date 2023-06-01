import { Component } from '@angular/core';
import { AzurehttpService } from '../azurehttp.service';

@Component({
  selector: 'app-delete-blob',
  templateUrl: './delete-blob.component.html',
  styleUrls: ['./delete-blob.component.css']
})
export class DeleteBlobComponent {

  public containerName: string = "";
  public containerList: string[] = [];
  public blobName: string = "";
  public blobList: string[] = [];

  constructor(private svc: AzurehttpService) {
    svc.getAllContainers().subscribe({
      next: (data)=> this.containerList = data,
      error: (data) => alert(data.error.text),
      complete: () => console.log()
    });
  }

  public onChangeContainer() {
    this.svc.getAllBlobs(this.containerName).subscribe({
      next: (data)=> this.blobList = data,
      error: (data) => alert(data.error.text),
      complete: () => console.log()
    });
  }

  public onClickDelete() {
    let confirmation = confirm("Are you sure you want to delete blob '"+this.blobName+"' ?");
    if(confirmation){
      this.svc.DeleteBlob(this.containerName,this.blobName).subscribe({
        next: (data)=> alert(data.text),
        error: (data) => alert(data.error.text),
        complete: () => console.log()
      });
    }
  }

}
