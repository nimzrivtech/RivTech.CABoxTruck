const _users = [
    {
        userId: 2,
        userName: 'boxtruckuser',
        displayName: 'Carl Bersales'
    },
    {
        userId: 165,
        userName: 'avivian',
        displayName: 'Alex Vivian'
    },
    {
        userId: 299,
        userName: 'btagent',
        displayName: 'Agent'
    },
    {
        userId: 315,
        userName: 'btunderwriter',
        displayName: 'Underwriter'
    },
    {
        userId: 316,
        userName: 'btspecialistua',
        displayName: 'Specialist UA'
    },
    {
        userId: 317,
        userName: 'btsuperuser',
        displayName: 'Super User'
    },
    {
        userId: 319,
        userName: 'btunderwriter2',
        displayName: 'Underwriter 2'
    }
];

export const UserIdentity = {
    getUserIdentity(userId: number): any {
        return _users.find(user => user.userId == userId);
    }
};
