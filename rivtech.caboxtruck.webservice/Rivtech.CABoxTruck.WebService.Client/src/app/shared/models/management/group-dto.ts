export class GroupDTO {
    id: number | undefined;
    name: string;
    description: string;
    programId: number | undefined;
    isInternalGroup: boolean;
    removedProcessDate: Date | undefined;

    userGroups: UserGroupDTO[] | undefined;
}

export class UserGroupDTO {
    id: number | undefined;
    groupId: number;
    userName: string;
    removedProcessDate: Date | undefined;
}