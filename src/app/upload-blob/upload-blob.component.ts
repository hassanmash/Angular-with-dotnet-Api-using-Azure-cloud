import { Component } from '@angular/core';
import { AzurehttpService } from '../azurehttp.service';

@Component({
  selector: 'app-upload-blob',
  templateUrl: './upload-blob.component.html',
  styleUrls: ['./upload-blob.component.css']
})
export class UploadBlobComponent {

  public containerName: string = "";
  public containerList: string[] = [];
  public file: File = new File([],"");

  constructor(private svc: AzurehttpService){
    svc.getAllContainers().subscribe({
      next: (data) => this.containerList = data,
      error: (data)=> alert(data.error.text),
      complete: () => console.log("container All blobs")
    });
  }

  public onChange(event:any) {
    this.file = event.target.files[0];
  }

  public uploadBlob() {
    console.log(this.file);
    let formData = new FormData();
    formData.append('blob',this.file,this.file.name);

    if(this.file.name != "" && this.containerName != ""){
      this.svc.UploadBlob(this.containerName,formData).subscribe({
        next: (data) => alert(data.text),
        error: (data) => alert(data.error.text),
        complete: () => console.log("Upload Request completed...")
      });
    }
    else{
      alert("Please input file or mention container name!");
    }
  }

}
