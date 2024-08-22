export interface TaskReturnModel {
    id: number;
    categoryId: number;
    apartmentId: number;
    staffId: number;
    description: string;
    creationDate: Date;
    startDate: Date;
    endDate: Date;
    cost: number;
}