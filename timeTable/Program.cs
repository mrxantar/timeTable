using System;
using System.Text.RegularExpressions;
using timeTable;

class Program
{
    public static void Main(string[] args)
    {
        List<Meeting> meetings = new List<Meeting>();
        
        new ConsoleUI().MainMenuUI();
    }
}