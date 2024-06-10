export class ApplicationUserDto {
    id: string;
    userName: string;
    public constructor(init?: Partial<ApplicationUserDto>) {
        Object.assign(this, init);
    }
}