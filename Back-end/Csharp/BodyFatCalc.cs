public class BodyFatCalc
{
	public static void Main()
	{
		// Body fat calc
		
		int subscapular = 8;
		int tricep = 7;
		int chest = 3;
		int midaxillary = 6;
		int abdominal = 12;
		int suprailiac = 14;
		int thigh = 16;
		int age = 27;
		
		int total = subscapular + tricep + chest + midaxillary + abdominal + suprailiac + thigh;
	
		var dc = 1.112 - 0.00043499 * total + 0.00000055 * total * 2 - 0.00028826 * age;
	
		var bf = ((4.95 / dc) - 4.50) * 100;
		
		Console.WriteLine($"Body Fat: { Math.Round(bf, 2) }%");
	}
}
