
export type ReturnsAcceptDto = {
    borrowalId: number;
    isOverdue: boolean;
    paid: boolean;
    amountAccepted: number;
    
    lateDays: number;
    perDayLateFeeDollars: number;
    // accepted user
    email: string;
    
}