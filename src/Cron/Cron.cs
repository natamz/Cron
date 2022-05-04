namespace Cron
{
    public class Cron
    {
        private static readonly Dictionary<string, int> _monthDict = new()
        {
            { "jan", 1 },
            { "feb", 2 },
            { "mar", 3 },
            { "apr", 4 },
            { "may", 5 },
            { "june", 6 },
            { "july", 7 },
            { "aug", 8 },
            { "sept", 9 },
            { "oct", 10 },
            { "nov", 11 },
            { "dec", 12 },
        };

        private static readonly Dictionary<string, int> _dayOfWeekDict = new()
        {
            { "sun", 0 },
            { "mon", 1 },
            { "tue", 2 },
            { "wed", 3 },
            { "thu", 4 },
            { "fri", 5 },
            { "sat", 6 },
        };

        private readonly string _expression;

        public Cron(string expression)
        {
            _expression = expression;
        }

        public bool Verify(DateTime dt)
        {
            var expressions = _expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (expressions.Length != 5) return false;

            return VerifyMinute(expressions[0], dt.Minute) &&
                VerifyHour(expressions[1], dt.Hour) &&
                VerifyDay(expressions[2], dt.Day) &&
                VerifyMonth(expressions[3], dt.Month) &&
                VerifyDayOfWeeks(expressions[4], (int)dt.DayOfWeek);
        }

        private bool VerifyMinute(string expression, int minute)
        {
            return ConvertCronValue(expression, 0, 59).Contains(minute);
        }
        private bool VerifyHour(string expression, int hour)
        {
            return ConvertCronValue(expression, 0, 23).Contains(hour);
        }
        private bool VerifyDay(string expression, int day)
        {
            return ConvertCronValue(expression, 1, 31).Contains(day);
        }
        private bool VerifyMonth(string expression, int month)
        {
            expression = ReplaceDictionaryValue(expression.ToLower(), _monthDict);
            return ConvertCronValue(expression, 1, 12).Contains(month);
        }
        private bool VerifyDayOfWeeks(string expression, int dayOfWeeks)
        {
            expression = ReplaceDictionaryValue(expression.ToLower(), _dayOfWeekDict);
            return ConvertCronValue(expression, 0, 6).Contains(dayOfWeeks);
        }

        private int[] ConvertCronValue(string expression, int min, int max)
        {
            if (expression == "*")
            {
                return Enumerable.Range(min, max + 1 - min).ToArray();
            }

            if (expression.Contains(','))
            {
                return expression.Split(',').SelectMany(x => ConvertCronValue(x, min, max)).Distinct().ToArray();
            }

            if (expression.Contains('/'))
            {
                var (First, Second) = Split(expression, "/");
                if (!int.TryParse(Second, out int divisor))
                {
                    throw new Exception();
                }

                return Division(ConvertCronValue(First, min, max), divisor);
            }

            if (expression.Contains('-'))
            {
                var (First, Second) = Split(expression, "-");
                if (!int.TryParse(First, out int _min) || !int.TryParse(Second, out int _max))
                {
                    throw new Exception();
                }

                return Enumerable.Range(_min, _max + 1 - _min).ToArray();
            }

            if (!int.TryParse(expression, out int num))
            {
                throw new Exception();
            }

            return new int[] { num };
        }

        private static (string First, string Second) Split(string target, string separator)
        {
            int index = target.IndexOf(separator);
            if (index < 0) throw new Exception();

            return (
                target.Remove(index),
                target.Remove(0, index + separator.Length)
                );
        }

        private static string ReplaceDictionaryValue(string target, Dictionary<string, int> dict)
        {
            foreach (var d in dict)
            {
                if (target.Contains(d.Key))
                {
                    target = target.Replace(d.Key, d.Value.ToString());
                }
            }
            return target;
        }

        private static int[] Division(int[] array, int divisor)
        {
            return array.Where((_, idx) => idx % divisor == 0).ToArray();
        }
    }
}
