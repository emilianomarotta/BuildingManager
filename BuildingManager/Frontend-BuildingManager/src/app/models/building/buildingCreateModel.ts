export interface BuildingCreateModel {
    name: string;
    address: string;
    location: string;
    companyId: number;
    fees: number;
    managerId: number | null;
}