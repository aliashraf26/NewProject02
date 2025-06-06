using System.Globalization;
using System.Text.RegularExpressions;

namespace VisualTimeTracking.UI.Helper
{
    public class Formater
    {
        #region Format Values
        // Converts a value to a decimal with 2 decimal places
        public static string ToDecimal2(decimal val)
        {
            return val.ToString("F2", CultureInfo.InvariantCulture);
        }

        // Converts a value to a currency format
        public static string ToNumber(double? amount )
        {
            if (amount == null || amount == 0)
            {
                return "0";
            }

            string formattedAmount;
             
                formattedAmount = Math.Round(amount.Value,2).ToString(CultureInfo.InvariantCulture);
             

            var parts = formattedAmount.Split('.');
            var x1 = parts[0];
            var x2 = parts.Length > 1 ? "." + parts[1] : "";

            x1 = Regex.Replace(x1, @"(\d)(?=(\d{3})+$)", "$1,");
            return "" + x1 + x2;
        }

        // Formats a number with commas as thousand separators
        public static string ToNumberWithComma(decimal? val)
        {
            return val?.ToString("#,##0", CultureInfo.InvariantCulture) ?? "";
        }

        // Formats a value as a percentage with 2 decimal places
        public static string FormatAsPercentage(double? value)
        {
            return $"{value?.ToString("F2", CultureInfo.InvariantCulture)}%" ?? "0%";
        }

        public static string GetValue(string? department, string? classValue, string? subClass)
        {
            if (!string.IsNullOrEmpty(department))
            {
                return department;
            }
            else if (!string.IsNullOrEmpty(classValue))
            {
                return classValue;
            }
            else if (!string.IsNullOrEmpty(subClass))
            {
                return subClass;
            }
            else
            {
                return string.Empty;
            }
        }

        #endregion
    }
}
