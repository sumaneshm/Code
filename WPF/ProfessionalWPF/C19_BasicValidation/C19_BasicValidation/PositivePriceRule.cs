using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace C19_BasicValidation
{
    public class PositivePriceRule : ValidationRule
    {
        private decimal min = 0;
        private decimal max = Decimal.MaxValue;

        public decimal Min { get { return min; } set { min = value; } }

        public decimal Max { get { return max; } set { max = value; } }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            decimal price = 0;

            if (!Decimal.TryParse((string)value, out price))
            {
                return new ValidationResult(false, "Illegal characters");
            }

            if (price < Min || price > Max)
                return new ValidationResult(false, "Not in range within " + Min + " and " + Max);

            return new ValidationResult(true, null);
        }
    }
}
