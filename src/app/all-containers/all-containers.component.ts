import { Component } from '@angular/core';
import { AzurehttpService } from '../azurehttp.service';

@Component({
  selector: 'app-all-containers',
  templateUrl: './all-containers.component.html',
  styleUrls: ['./all-containers.component.css']
})
export class AllContainersComponent {

  public containerList: string[] = [];

  constructor(private svc: AzurehttpService) {}

  public getContainer() {
    this.svc.getAllContainers().subscribe({
      next: (data) => this.containerList = data,
      error: (data) => alert(data.error.text),
      complete: () =>  console.log('Get All containers completed...')
    });
  }
  
}
