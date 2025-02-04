using timeTable;

class Program
{
    
    public static void Main(string[] args)
    {
        Schedule schedule = new Schedule();
        new ConsoleUI(schedule).MainMenuUI();
    }
}