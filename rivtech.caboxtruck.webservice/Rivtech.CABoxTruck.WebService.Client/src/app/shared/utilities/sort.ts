export class Sort {
  private sortOrder = 1;
  private collator = new Intl.Collator(undefined, {
    numeric: true,
    sensitivity: 'base'
  });

  constructor() { }

  public startSort(property, order, type = '', from = null) {
    if(!property)
      return;

    if (order === 'desc') {
      this.sortOrder = -1;
    }
    return (a, b) => {
      if (property.indexOf('.') > -1) {
        let properties = property.split('.');
        if (type === 'date') {
          return this.sortData(new Date(a[properties[0]][properties[1]]), new Date(b[properties[0]][properties[1]]));
        } else {
          return this.collator.compare(a[properties[0]][properties[1]], b[properties[0]][properties[1]]) * this.sortOrder;
        }
      } else {
        if (type === 'date') {
          if (from === 1)
            return this.sortData(a[property], b[property]);
          else
          return this.sortData(new Date(a[property]), new Date(b[property]));
        } else if (type === 'text') {
          return this.collator.compare(a[property]?.replace(',', ''), b[property]?.replace(',', '')) * this.sortOrder;
        } else {
          return this.collator.compare(a[property], b[property]) * this.sortOrder;
        }
      }
    };
  }

  private sortData(a, b) {
    if (a < b) {
      return -1 * this.sortOrder;
    } else if (a > b) {
      return 1 * this.sortOrder;
    } else {
      return 0 * this.sortOrder;
    }
  }
}
