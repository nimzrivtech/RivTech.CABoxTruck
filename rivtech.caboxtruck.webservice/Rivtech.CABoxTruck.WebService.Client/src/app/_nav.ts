interface NavAttributes {
  [propName: string]: any;
}
interface NavWrapper {
  attributes: NavAttributes;
  element: string;
}
interface NavBadge {
  text: string;
  variant: string;
}
interface NavLabel {
  class?: string;
  variant: string;
}

export interface NavData {
  name?: string;
  url?: string;
  icon?: string;
  badge?: NavBadge;
  title?: boolean;
  children?: NavData[];
  variant?: string;
  attributes?: NavAttributes;
  divider?: boolean;
  class?: string;
  label?: NavLabel;
  wrapper?: NavWrapper;
}

export const navItems: NavData[] = [
  {
    name: 'Submission',
    url: '/submission/index',
    icon: 'icon-doc'
  },
  {
    name: 'Underwriting',
    url: '/underwriting',
    icon: 'icon-briefcase',
    children: [
      {
        name: 'Rating',
        url: '/submission/rating',
        icon: 'icon-flag'
      },
      {
        name: 'Endorsement',
        url: '/submission/endorsement',
        icon: 'icon-note'
      },
      {
        name: 'UW Assessment',
        url: '/submission/uw-assessment',
        icon: 'icon-user'
      },
      {
        name: 'Quote',
        url: '/submission/quote',
        icon: 'icon-calculator'
      }
    ]
  },
  // {
  //   name: 'Additional Info',
  //   url: '/submission/additional-info',
  //   icon: 'icon-list'
  // },
  {
    name: 'Bind',
    url: '/submission/bind',
    icon: 'icon-notebook'
  },
  // {
  //   name: 'Contacts',
  //   url: '/submission/contacts',
  //   icon: 'icon-people'
  // },
  {
    name: 'Notes',
    url: '/submission/notes',
    icon: 'icon-pencil'
  },
  {
    name: 'Documents',
    url: '/submission/documents',
    icon: 'icon-docs',
  },

  {
    name: 'Save & Exit',
    url: '/submission/save-exit',
    icon: 'icon-check'
  },
  {
    name: 'Reports',
    url: '/submission/reports',
    icon: 'icon-book-open'
  }
];

export const navItemsManagement: NavData[] = [
  {
    name: 'Users',
    url: '/management/users',
    icon: 'icon-people'
  },
  {
    name: 'Agencies',
    url: '/management/agencies',
    icon: 'icon-notebook'
  },
  {
    name: 'Roles',
    url: '/management/roles',
    icon: 'icon-list'
  },{
    name: 'Marital-Gender',
    url: '/management/maritalgender',
    icon: 'icon-notebook'
  },{
    name: 'Eil-Deductible',
    url: '/management/eildeductible',
    icon: 'icon-notebook'
  },{
    name: 'Medical-Expenses-Limit',
    url: '/management/medical-expenses-limit',
    icon: 'icon-notebook'
  },
];