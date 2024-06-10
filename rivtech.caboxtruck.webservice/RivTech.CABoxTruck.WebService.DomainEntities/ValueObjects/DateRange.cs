using System;
using System.Collections.Generic;
using System.Text;

namespace RivTech.CABoxTruck.WebService.DomainEntities.ValueObjects
{
    public class DateRange
    {
        private DateRange() { }

        public DateRange(DateTime start, DateTime end)
        {
            Start = start;
            End = end;
        }

        public DateRange(string start, string end)
        {
            Start = DateTime.Parse(start);
            End = DateTime.Parse(end);
        }

        public DateTime Start { get; protected set; }
        public DateTime End { get; protected set; }
        protected int Days => (End - Start).Days;
        /// <summary>
        /// When Start date is later than End date, it always return 0 instead of negative value.
        /// </summary>
        public int DaysNonNegative => Start > End ? 0 : Days;

        /// <summary>
        /// Returns true if the given date is greater/equal to Start and lesser than End else return false.
        /// </summary>
        public bool IsDateWithinRange(DateTime date)
        {
            return Start <= date && date < End;
        }

        /// <summary>
        /// Returns true if End date is equal/lesser than the given date else returns false.
        /// </summary>
        public bool IsEarlierThan(DateTime date)
        {
            return End <= date;
        }

        internal DateRange ChangeStart(DateTime start)
        {
            return new DateRange
            {
                Start = start,
                End = End
            };
        }

        /// <summary>
        /// Returns a new instance with same Start and new End.
        /// </summary>
        public DateRange WithNewEnd(DateTime date)
        {
            return new DateRange(Start, date);
        }

        public override string ToString()
        {
            return $"{Start} - {End}";
        }

        public override bool Equals(object obj)
        {
            var other = obj as DateRange;
            return other != null
                && other.Start.Equals(Start)
                && other.End.Equals(End);

        }

        public DateRange Clone()
        {
            return new DateRange
            {
                Start = Start,
                End = End
            };
        }
    }
}
