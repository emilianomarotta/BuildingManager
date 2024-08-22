export enum Role {
    Manager = 0,
    CompanyAdmin = 1
  }
  
  export const RoleDescriptions: { [key: number]: string } = {
    [Role.Manager]: 'Manager',
    [Role.CompanyAdmin]: 'Company Administrator'
  };
  