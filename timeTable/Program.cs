using System;
using timeTable;

class Program
{
    static Schedule schedule = new Schedule();
    public static void Main(string[] args)
    {
        new ConsoleUI(schedule).MainMenuUI();
    }
}