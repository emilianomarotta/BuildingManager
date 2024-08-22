export interface ReportStaffReturnModel {
    staffName: string;
    openTasks: number;
    inProgressTasks: number;
    closedTasks: number;
    averageCloseTime: number;
}