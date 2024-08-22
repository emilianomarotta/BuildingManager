import { ApartmentReturnModel } from "../apartment/apartmentReturnModel";
import { CompanyReturnModel } from "../company/companyReturnModel";
import { ManagerReturnModel } from "../managerReturnModel";

export interface BuildingReturnModel {
    id: number,
    name: string,
    manager: ManagerReturnModel | null,
    address: string,
    location: string,
    fees: number,
    company: CompanyReturnModel,
    apartments: [ApartmentReturnModel] | []
}