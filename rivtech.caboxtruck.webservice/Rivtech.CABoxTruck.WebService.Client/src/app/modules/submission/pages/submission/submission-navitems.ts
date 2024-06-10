import {NavData} from '../../../../_nav';

export function createSubmissionMenuItems (id?: number): NavData[] {
  let paramId = '';

  if (id != null && id > 0) {
    paramId = `/${id}`;
  }

  const path = '/' + 'submission'
    + '/' + 'applicant'
    +  paramId;

  return [
    {
      name: 'APPLICANT',
      url:  path + '/' + 'applicant',
      icon: 'icon-user',
      children: [
        {
          name: 'General Info',
          icon: 'icon-info',
          url: path
            + '/' + 'applicant'
            + '/' + 'general-info'
        },
        {
          name: 'Personnel',
          icon: 'icon-people',
          url: path
            + '/' + 'applicant'
            + '/' + 'personnel',
        },
      ]
    },
    {
      name: 'Broker',
      url: `/submission/${paramId}/broker`,
      icon: 'icon-people',
    },
    {
      name: 'Coverages',
      url: `/submission/${paramId}/coverages`,
      icon: 'icon-people',
    },
    {
      name: 'FILING',
      url: `/submission/${paramId}/filing`,
      icon: 'icon-people',
    },
    {
      name: 'RISK SPECIFICS',
      url: `/submission/${paramId}/risk-specifics`,
      icon: 'icon-people',
    },
    {
      name: 'DOCUMENTS',
      url: `/submission/${paramId}/risk-specifics`,
      icon: 'icon-people',
    },
    {
      name: 'NOTES',
      url: `/submission/${paramId}/risk-specifics`,
      icon: 'icon-people',
    },
    {
      name: 'TASKS',
      url: `/submission/${paramId}/risk-specifics`,
      icon: 'icon-people',
    }
  ];
}
