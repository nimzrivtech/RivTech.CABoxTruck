export const Month = (id, name) => {
  return {
    id: id,
    name: name,
    toString: () => {
      return `${id} - ${name}`;
    }
  };
};

export const MonthConstants = {
  January: Month('1', 'January'),
  February: Month('2', 'February'),
  March: Month('3', 'March'),
  April: Month('4', 'April'),
  May: Month('5', 'May'),
  June: Month('6', 'June'),
  July: Month('7', 'July'),
  August: Month('8', 'August'),
  September: Month('9', 'September'),
  October: Month('10', 'October'),
  November: Month('11', 'November'),
  December: Month('12', 'December')
};
