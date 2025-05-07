
export type ReturnsAcceptDto = {
    borrowalId: number;
    isOverdue: boolean;
    paid: boolean;
    amountAccepted: number;

    // accepted user
    email: string;
    
}