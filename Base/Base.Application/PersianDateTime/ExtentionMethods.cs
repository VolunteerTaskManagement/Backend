using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Utilities
{
    public static class ExtentionMethods
    {
        #region گرفتن تاریخ در قالب ابجکت تاریخ شمسی
        /// <summary>
        /// گرفتن تاریخ در قالب ابجکت تاریخ شمسی
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static PersianDateTime? ToPersianDateTime(this DateTime? Date)
        {
            if (Date == null)
                return null;
            return ((DateTime)Date).ToPersianDateTime();
        }

        /// <summary>
        /// گرفتن تاریخ در قالب ابجکت تاریخ شمسی
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static PersianDateTime ToPersianDateTime(this DateTime Date)
        {
            var pd = new PersianDateTime(Date) { EnglishNumber = true };
            pd.EnglishNumber = true;
            return pd;
        }
        #endregion
    }
}