import { Component } from '@angular/core';
import { AzurehttpService } from '../azurehttp.service';

@Component({
  selector: 'app-delete-container',
  templateUrl: './delete-container.component.html',
  styleUrls: ['./delete-container.component.css']
})
export class DeleteContainerComponent {

  public containerName: string = "";
  public containerList: string[] = [];

  constructor(private svc: AzurehttpService) {
    svc.getAllContainers().subscribe({
      next: (data)=> this.containerList = data,
      error: (data) => alert(data.error.text),
      complete: () => console.log()
    });
  }

  public onClickDelete() {
    let confirmation = confirm("Are you sure you want to delete container '"+this.containerName+"' ?");
    if(confirmation){
      this.svc.DeleteContainer(this.containerName).subscribe({
        next: (data)=> alert(data.text),
        error: (data) => alert(data.error.text),
        complete: () => console.log()
      });
    }
  }

}
