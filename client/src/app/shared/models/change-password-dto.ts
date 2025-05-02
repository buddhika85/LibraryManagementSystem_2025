export type ChangePasswordDto = {
    username: string;
    oldPassword: string;
    newPassword: string;
    confirmPassword: string;
}