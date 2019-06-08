using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace SmsPanelSms.Restful
{
	internal static class DateTimeExtensions
	{
		public static DateTime? GetEnglishDateTime(this string helper)
		{
			DateTime? nullable;
			DateTime? nullable1;
			string str = "(?>((?>13|14)\\d\\d)|(\\d\\d))\\/(0?[1-9]|1[012])\\/([12][0-9]|3[01]|0?[1-9])";
			try
			{
				if (!Regex.IsMatch(helper, str))
				{
					nullable1 = null;
					return nullable1;
				}
				else
				{
					Regex regex = new Regex(str);
					int num = int.Parse(regex.Match(helper).Groups[1].Value);
					int num1 = int.Parse(regex.Match(helper).Groups[3].Value);
					int num2 = int.Parse(regex.Match(helper).Groups[4].Value);
					nullable = new DateTime?((new PersianCalendar()).ToDateTime(num, num1, num2, 0, 0, 0, 0));
				}
			}
			catch (Exception exception)
			{
				nullable1 = null;
				nullable = nullable1;
			}
			return nullable;
		}

		public static DateTime GetEnglishDateTimeNonNullable(this string helper)
		{
			DateTime now;
			string str = "(?>((?>13|14)\\d\\d)|(\\d\\d))\\/(0?[1-9]|1[012])\\/([12][0-9]|3[01]|0?[1-9])";
			try
			{
				if (!Regex.IsMatch(helper, str))
				{
					now = DateTime.Now;
				}
				else
				{
					Regex regex = new Regex(str);
					int num = int.Parse(regex.Match(helper).Groups[1].Value);
					int num1 = int.Parse(regex.Match(helper).Groups[3].Value);
					int num2 = int.Parse(regex.Match(helper).Groups[4].Value);
					now = (new PersianCalendar()).ToDateTime(num, num1, num2, 0, 0, 0, 0);
				}
			}
			catch (Exception exception)
			{
				now = DateTime.Now;
			}
			return now;
		}

		public static string GetPersianDate(this DateTime? helper)
		{
			return DateTime.Parse(helper.ToString()).GetPersianDate();
		}

		public static string GetPersianDate(this DateTime helper)
		{
			if (helper.Year < 1000)
			{
				helper = DateTime.Now;
			}
			PersianCalendar persianCalendar = new PersianCalendar();
			StringBuilder stringBuilder = new StringBuilder("yyyy/mm/dd".ToLower());
			int year = persianCalendar.GetYear(helper);
			StringBuilder stringBuilder1 = stringBuilder.Replace("yyyy", year.ToString());
			year = persianCalendar.GetMonth(helper);
			StringBuilder stringBuilder2 = stringBuilder1.Replace("mm", year.ToString("00"));
			year = persianCalendar.GetDayOfMonth(helper);
			return stringBuilder2.Replace("dd", year.ToString("00")).ToString();
		}

		public static string GetPersianDateTime(this DateTime? helper)
		{
			return helper.Value.GetPersianDateTime("yyyy/mm/dd");
		}

		public static string GetPersianDateTime(this DateTime helper, string format = "yyyy/mm/dd")
		{
			if (helper.Year < 1000)
			{
				helper = DateTime.Now;
			}
			PersianCalendar persianCalendar = new PersianCalendar();
			StringBuilder stringBuilder = new StringBuilder(format.ToLower());
			int year = persianCalendar.GetYear(helper);
			StringBuilder stringBuilder1 = stringBuilder.Replace("yyyy", year.ToString());
			year = persianCalendar.GetMonth(helper);
			StringBuilder stringBuilder2 = stringBuilder1.Replace("mm", year.ToString("00"));
			year = persianCalendar.GetDayOfMonth(helper);
			StringBuilder stringBuilder3 = stringBuilder2.Replace("dd", year.ToString("00"));
			stringBuilder3.Append(string.Concat(" ", helper.ToString("HH:mm:ss")));
			return stringBuilder3.ToString();
		}

		public static string GetPersianDateWithTime(this DateTime helper)
		{
			if (helper.Year < 1000)
			{
				helper = DateTime.Now;
			}
			PersianCalendar persianCalendar = new PersianCalendar();
			StringBuilder stringBuilder = new StringBuilder("yyyy/mm/dd hh:jj:ss".ToLower());
			int hour = helper.Hour;
			StringBuilder stringBuilder1 = stringBuilder.Replace("hh", hour.ToString());
			hour = helper.Minute;
			StringBuilder stringBuilder2 = stringBuilder1.Replace("jj", hour.ToString());
			hour = helper.Second;
			StringBuilder stringBuilder3 = stringBuilder2.Replace("ss", hour.ToString());
			hour = persianCalendar.GetYear(helper);
			StringBuilder stringBuilder4 = stringBuilder3.Replace("yyyy", hour.ToString());
			hour = persianCalendar.GetMonth(helper);
			StringBuilder stringBuilder5 = stringBuilder4.Replace("mm", hour.ToString("00"));
			hour = persianCalendar.GetDayOfMonth(helper);
			return stringBuilder5.Replace("dd", hour.ToString("00")).ToString();
		}
	}
}