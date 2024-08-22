export interface ApartmentReturnModel {
    id: number;
    floor: number;
    number: number;
    buildingId: number;
    ownerId: number;
    bedrooms: number;
    bathrooms: number;
    balcony: boolean;
}