//Given an input sequence of several integer value modelling the price of a stock in a given day (quotes), write a method / function that calculates the maximum profit 
//that could be done by date by buying 1 unit of the stock at a point in time that day and selling the same stock at a later point in time the same day.
//If no buy and sell combination returns a profit, we can think of the case where the stock is bought and sold fast enough that the price did not change, hence 0 profit.

using System;

namespace MaxProfit
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var quotes = new[] { 1, 2, 3, 4, 5, 6, 7 };
 
            var quotes2 = new[] { 7, 6, 5, 4, 3, 2, 1 };
 
            var quotes3 = new[] { 5, 6, 4, 6, 8, 1 };
 
            var maxProfit1 = MaxProfit(quotes);
            var maxProfit2 = MaxProfit(quotes2);
            var maxProfit3 = MaxProfit(quotes3);
 
            if (maxProfit1 != 6) throw new System.Exception("Error");
            if (maxProfit2 != 0) throw new System.Exception("Error");
            if (maxProfit3 != 4) throw new System.Exception("Error");
            if (MaxProfit(new int[0]) != 0) throw new System.Exception("Error");
            Console.WriteLine("All good");
        }
 
        private static int MaxProfit(int[] quotes)
        {
			int profit = 0;
			int bestProfit = 0;
			
			if (quotes.Length < 0)
				return 0;

			for(int i = 0; i < quotes.Length; i++)
			{
				for(int j = i; j < quotes.Length; j++)
				{
					profit = quotes[j] - quotes[i];
					
					if (profit > 0 && profit > bestProfit)
						bestProfit = profit;
				}
			}
			
			Console.WriteLine(bestProfit);
			
			return bestProfit;
        }
    }
}
