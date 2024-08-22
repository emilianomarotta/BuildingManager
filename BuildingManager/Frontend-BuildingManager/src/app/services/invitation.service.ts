import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { InvitationReturnModel } from '../models/invitation/invitationReturnModel';
import { environment } from '../environments/environment';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { InvitationEndpoints } from '../networking/endpoints';
import { InvitationCreateModel } from '../models/invitation/invitationCreateModel';
import { InvitationPutModel } from '../models/invitation/invitationPutModel';

interface IInvitationService {
  getInvitations(): Observable<InvitationReturnModel[]>
}

@Injectable({
  providedIn: 'root'
})
export class InvitationService implements IInvitationService{

  constructor(private http: HttpClient) { }

  getInvitations(): Observable<InvitationReturnModel[]> {
      return this.http.get<InvitationReturnModel[]>(InvitationEndpoints.INVITATIONS);
  }

  addInvitation(invitation: InvitationCreateModel): Observable<InvitationCreateModel> {
    return this.http.post<InvitationCreateModel>(InvitationEndpoints.INVITATIONS, invitation);
  }

  respondToInvitation(id: string, response: InvitationPutModel): Observable<any> {
    return this.http.put(`${InvitationEndpoints.INVITATION}${id}`, response);
  }
}
