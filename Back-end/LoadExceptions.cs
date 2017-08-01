using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LoadExceptions
{
    class Program
    {
        static void Main(string[] args)
        {
            var listEntity = new List<Entity>();
            var listErros = new List<string>();
            var hash = new Criptografia();

            var diretorio = new DirectoryInfo(@"C:\\Users\\aderbal.filho\\Desktop\\OI\\LoadExceptions\\Exceptions");
            FileInfo[] files = diretorio.GetFiles("*", SearchOption.AllDirectories);

            foreach (var file in files)
            {
                try
                {
                    var fileName = file.Name.Split('-');
                    listEntity.Add( new Entity
                    {
                        //Id = Convert.ToInt32(hash.UrlDecrypt(File.ReadAllText(file.FullName).Split(new[] { "| Id: " }, StringSplitOptions.None)[1].Replace("\r\n", ""))),
                        Id = Convert.ToInt32(hash.UrlDecrypt(File.ReadAllLines(file.FullName).Last().Split(new[] { "| Id: " }, StringSplitOptions.RemoveEmptyEntries)[1])),
                        Data = string.Format("{0:yyyy-MM-dd HH:mm:ss}", new DateTime(Convert.ToInt32(fileName[0].Substring(0, 4)),
                            Convert.ToInt32(fileName[0].Substring(4, 2)), Convert.ToInt32(fileName[0].Substring(6, 2)), 
                            Convert.ToInt32(fileName[1].Substring(0, 2)), Convert.ToInt32(fileName[1].Substring(2, 2)), 
                            Convert.ToInt32(fileName[1].Substring(4, 2))))
                    });

                    //File.Move(file.FullName, Path.Combine("C:\\Users\\aderbal.filho\\Desktop\\OI\\LoadExceptions\\ExceptionsOk", file.Name));
                }
                catch (Exception)
                {
                    listErros.Add(file.Name);
                }
            }
        }
    }
}



class ReadFromFile
{
    static void Main()
    {
        // The files used in this example are created in the topic 
        // How to: Write to a Text File. You can change the path and 
        // file name to substitute text files of your own. 

        var diretorio = new DirectoryInfo(@"C:\\xx");
        FileInfo[] files = diretorio.GetFiles("*", SearchOption.AllDirectories);

        foreach (var file in files)
        {
        
        }

        // Example #1 
        // Read the file as one string. 
        string text = System.IO.File.ReadAllText(@"C:\Users\Public\TestFolder\WriteText.txt");

        // Display the file contents to the console. Variable text is a string.
        System.Console.WriteLine("Contents of WriteText.txt = {0}", text);

        // Example #2 
        // Read each line of the file into a string array. Each element 
        // of the array is one line of the file. 
        string[] lines = System.IO.File.ReadAllLines(@"C:\Users\Public\TestFolder\WriteLines2.txt");

        // Display the file contents by using a foreach loop.
        System.Console.WriteLine("Contents of WriteLines2.txt = ");
        foreach (string line in lines)
        {
            // Use a tab to indent each line of the file.
            Console.WriteLine("\t" + line);
        }

        // Keep the console window open in debug mode.
        Console.WriteLine("Press any key to exit.");
        System.Console.ReadKey();
    }
       
}