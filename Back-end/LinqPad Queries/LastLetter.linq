<Query Kind="Program" />

void Main()
{
	string temp = "2222222A";
	char last = temp[temp.Length - 1];
	
	var exceptLast = temp.Remove(temp.Length - 1);
	
	last.Dump("Last Letter");
	exceptLast.Dump("Removing Last Letter");
}

