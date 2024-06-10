import { PathConstants } from '../../shared/constants/path.constants';
import { NavData } from '../../_nav';

export const createManagementMenuItems = (userType: string): NavData[] => {
  const path = '/' + PathConstants.Management.Index;
  const roleID = localStorage.getItem('role_id');

  const submissionMenu = [
    {
      name: 'Agency',
      icon: 'icon-user',
      url: `${path}/${PathConstants.Management.Agency.Index}`,
      // children: [
      //   {
      //     name: 'Agency Home',
      //     icon: 'icon-user',
      //     url: `${path}/${PathConstants.Management.Agency.Index}/${PathConstants.Management.Agency.Agencies}`,
      //   },
      // ]
    },
    {
      name: 'Users',
      icon: 'icon-user',
      url: `${path}/${PathConstants.Management.Users.Index}/${PathConstants.Management.Users.User}`,
      //Temporarily Comment Out for future use
      // children: [
      //   {
      //     name: 'Agent List',
      //     icon: 'icon-user',
      //     url: `${path}/${PathConstants.Management.Users.Index}/${PathConstants.Management.Users.Agent}`,
      //   },
      //   {
      //     name: 'System User',
      //     icon: 'icon-user',
      //     url: `${path}/${PathConstants.Management.Users.Index}/${PathConstants.Management.Users.User}`,
      //   },
      //   {
      //     name: 'User Type',
      //     icon: 'icon-user',
      //     url: `${path}/${PathConstants.Management.Users.Index}/${PathConstants.Management.Users.UserType}`,
      //   },
      //   {
      //     name: 'User Group',
      //     icon: 'icon-user',
      //     url: `${path}/${PathConstants.Management.Users.Index}/${PathConstants.Management.Users.UserGroup}`,
      //   },
      // ]
    },
    // {
    //   name: 'Roles',
    //   icon: 'icon-user',
    //   url: `${path}/${PathConstants.Management.Roles.Index}`,
    // },
    {
      name: 'User Type',
      icon: 'icon-user',
      url: `${path}/${PathConstants.Management.Users.Index}/${PathConstants.Management.Users.UserType}`,
    },
    {
      name: 'User Group',
      icon: 'icon-user',
      url: `${path}/${PathConstants.Management.Users.Index}/${PathConstants.Management.Users.UserGroup}`,
    }
  ];


  const submissionMenu1 = [
    {
      name: 'Agency',
      icon: 'icon-user',
      url: `${path}/${PathConstants.Management.Agency.Index}`,
    },
    {
      name: 'Retailer',
      icon: 'icon-user',
      url: `${path}/${PathConstants.Management.Retailer.Index}`,
    },
    {
      name: 'Users',
      icon: 'icon-user',
      url: `${path}/${PathConstants.Management.Users.Index}/${PathConstants.Management.Users.User}`,
    },
  ];

  return roleID === '1' ? submissionMenu : submissionMenu1;
};

