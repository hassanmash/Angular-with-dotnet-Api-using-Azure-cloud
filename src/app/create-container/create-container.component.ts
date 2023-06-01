import { Component } from '@angular/core';
import { AzurehttpService } from '../azurehttp.service';

@Component({
  selector: 'app-create-container',
  templateUrl: './create-container.component.html',
  styleUrls: ['./create-container.component.css']
})
export class CreateContainerComponent {

  public containerName: string = "";

  constructor(private svc: AzurehttpService){}

  public createContainer() {
    if(this.containerName != ""){
      this.svc.createContainer(this.containerName).subscribe({
        next: (data) => alert(data.text),
        error: (data) => alert(data.error.text),
        complete: ()=> console.log("Create container request completed...")
      });
    }
    else{
      alert("Container name cannot be empty!")
    }
  }

}
