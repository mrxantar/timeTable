using System;
using System.Text.RegularExpressions;
using timeTable;

class Program
{
    static Schedule schedule = new Schedule();
    public static void Main(string[] args)
    {
        new ConsoleUI().MainMenuUI(schedule);
    }
}