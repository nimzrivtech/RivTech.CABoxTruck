import { ITableFormControl } from '../models/dynamic/table.interface';

export const MaritalStatus = [{
  code: 1,
  description: 'Never Married'
},
{
  code: 2,
  description: 'Married'
},
{
  code: 3,
  description: 'Widowed'
},
{
  code: 4,
  description: 'Divorced'
}];

export const Gender = [{
  code: 1,
  description: 'Male'
},
{
  code: 2,
  description: 'Female'
},
{
  code: 3,
  description: 'Not to Answer'
}];

export const AddressType = [{
  code: 1,
  description: 'Primary'
},
{
  code: 2,
  description: 'Secondary'
}];

export const City = [{
  code: 1,
  description: 'Angeles'
},
{
  code: 2,
  description: 'Hialeah'
}];

export const State = [{
  code: 1,
  description: 'California'
},
{
  code: 2,
  description: 'Florida'
}];

export const Programs = [{
  code: 1,
  description: 'Program 1'
},
{
  code: 2,
  description: 'Program 2'
}];

export const Roles = [{
  code: 1,
  description: 'Role 1'
},
{
  code: 2,
  description: 'Role 2'
}];

export const RoleGroups = [{
  code: 1,
  description: 'Role Group 1'
},
{
  code: 2,
  description: 'Role Group 2'
}];


export const AccessCategory = [
{
  code: 1,
  description: 'Admin'
},
{
  code: 2,
  description: 'CoAdmin'
},
{
  code: 3,
  description: 'Member'
}
];

export const TableFormcontrols: ITableFormControl[]  = [
  {
    name: 'field_1',
    type: 'string',
  },
  {
    name: 'field_2',
    type: 'string',
  },
  {
    name: 'field_3',
    type: 'string',
  },
  {
    name: 'field_4',
    type: 'string',
  },
  {
    name: 'field_5',
    type: 'string',
  },
  {
    name: 'field_6',
    type: 'string',
  }
];
