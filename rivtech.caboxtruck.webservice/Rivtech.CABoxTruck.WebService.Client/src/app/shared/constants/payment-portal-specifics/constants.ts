import { environment } from "../../../environments/environment";

export const Constants = Object.assign({
    dataUri: "data:image/jpeg;base64,",
    defaultUserProfileImage: "/StaticFiles/default-user-profile-image.jpg",
    emptyGuid:'00000000-0000-0000-0000-000000000000',
    emailFormat: '/^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/',
}, environment);