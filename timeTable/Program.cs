using timeTable;

class Program
{
    public static void Main(string[] args)
    {
        var schedule = new Schedule();

        var options = new List<MenuOption>
        {
            new CreateMeetingOption(schedule),
            new ViewMeetingsOption(schedule),
            new ExitOption()
        };

        var menuManager = new MenuManager(options);
        menuManager.Run();
    }
}