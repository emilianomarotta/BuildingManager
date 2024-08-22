export interface InvitationReturnModel{
    id: number,
    name: string,
    email: string,
    role: number,
    expiration: Date,
    status: number
}