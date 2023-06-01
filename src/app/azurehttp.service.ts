import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AzurehttpService {

  private url = "http://localhost:5194/blobs/"

  constructor(private http: HttpClient) { }

  public getAllContainers(): Observable<any> {
    return this.http.get(this.url + "containers");
  }

  public createContainer(containerName: string): Observable<any> {
    return this.http.get(this.url + "createcontainer/" + containerName);
  }

  public getAllBlobs(containerName: string): Observable<any> {
    return this.http.get(this.url + containerName);
  }

  public downloadBlob(containerName:string,BlobName:string): Observable<any> {
    return this.http.get(this.url + "download/" + containerName + "/" + BlobName);
  }

  public UploadBlob(containerName:string ,formData: FormData): Observable<any> {
    const headers = new HttpHeaders();
    headers.append('Content-Type', 'multipart/form-data');
    headers.append('Accept', 'multipart/form-data');
    const options =  {
        headers: headers
    };
    return this.http.post(this.url + "upload/" + containerName,formData,options);
  }

  public DeleteBlob(containerName:string, blobName:string): Observable<any> {
    return this.http.delete(this.url+"deleteblob/"+containerName+"/"+blobName);
  }

  public DeleteContainer(containerName:string): Observable<any> {
    return this.http.delete(this.url+"deletecontainer/"+containerName);
  }
}
