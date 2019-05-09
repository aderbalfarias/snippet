void Main()
{
	Random randNum = new Random();
	var ln = new List<int>();
        int cont = 0;
	int num;
	
	while (cont < 6)
	{
		num = randNum.Next(60);
		
		if (num != 0 && !ln.Contains(num))
		{
			ln.Add(num);
			cont++;
		}
	}	
	
	ln.Sort();		
	ln.Dump("Números da Mega-Sena");
}
