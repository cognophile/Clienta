using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Clienta.Services
{
    public class FormattingService
    {
        /// <summary>
        /// Format a DateTime object into the dd/MM/yyy format
        /// </summary>
        /// <param name="date">DateTime</param>
        /// <returns>string: The formatted date</returns>
        public static string ConvertToShortUkFormat(DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }
    }
}
