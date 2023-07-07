using System;
namespace DayDifference
{
	public class DayDifference
	{
		private int[] date1 = new int[3];
		private int[] date2 = new int[3];
		private int[] days_each_month = new int[12]
		{
			31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31
		};

		public DayDifference()
		{
		}

		private int countMonthDays(int[] date)
		{
			int month = date[1];
			int count = 0;

			for (int i = 0; i < month - 1; i++)
			{
				count += days_each_month[i];
			}

			return count;
		}

		public int CountDifference()
		{
			int[][] dates = new int[][] { date1, date2 };
			int[] total_days = new int[2];

			for (int i = 0; i < 2; i++)
			{
				int leaps = numberOfLeapYears(dates[i]);
                int days = dates[i][0];
				int months_in_days = countMonthDays(dates[i]);
                int years_in_days = 365 * dates[i][2];

				int sum = days + months_in_days + years_in_days + leaps;
				total_days[i] = sum;
			}

			int difference = total_days[1] - total_days[0];
			return difference;
		}

		private int numberOfLeapYears(int[] date)
		{
			int month = date[1];
			int year = date[2];

			if (month < 3)
				year--;
			int leaps = (year / 4) - (year / 100) + (year / 400);

			return leaps;
		}

		private void fillDates(int[] date, string[] half_arr)
		{
			for (int i = 0; i < half_arr.Length; i++)
			{
				date[i] = Convert.ToInt32(half_arr[i]);
			}
		}

		public void GetDates()
		{
			string[] line = Console.ReadLine().Split();

			fillDates(date1, line.Take(line.Length / 2).ToArray());
            fillDates(date2, line.Skip(line.Length / 2).ToArray());

        }
    }
}

