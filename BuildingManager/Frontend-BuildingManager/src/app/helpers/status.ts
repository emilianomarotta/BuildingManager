export enum InvitationStatus {
    Accepted = 0,
    Expired = 1,
    Pending = 2,
    Rejected = 3
  }
  
  export const InvitationStatusDescriptions: { [key: number]: string } = {
    [InvitationStatus.Accepted]: 'Accepted',
    [InvitationStatus.Expired]: 'Expired',
    [InvitationStatus.Pending]: 'Pending',
    [InvitationStatus.Rejected]: 'Rejected'
  };