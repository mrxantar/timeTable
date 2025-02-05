namespace timeTable;
public abstract class MenuOption
{
    public string Name { get; }

    protected MenuOption(string name)
    {
        Name = name;
    }

    public abstract void Execute();
}

public class CreateMeetingOption : MenuOption
{
    private readonly Schedule schedule;

    public CreateMeetingOption(Schedule schedule) : base("Создать встречу")
    {
        this.schedule = schedule;
    }

    public override void Execute()
    {
        var meeting = DateTimeMeetingUI();
        if (schedule.AddMeeting(meeting))
        {
            ConsoleHelper.ShowMessage("Встреча успешно добавлена");
        }
        else
        {
            ConsoleHelper.ShowMessage("На это время назначена другая встреча. Новая встреча не была создана");
        }
    }

    private Meeting DateTimeMeetingUI()
    {
        DateTime startTime = GetDateTime("Укажите дату и время начала встречи (дд.мм.гггг чч:мм):");
        DateTime endTime = GetDateTime("Укажите дату и время конца встречи (дд.мм.гггг чч:мм):", startTime);

        int? notifMin = GetNotificationMinutes();
        return notifMin.HasValue
            ? new Meeting(startTime, endTime, notifMin.Value)
            : new Meeting(startTime, endTime);
    }

    private DateTime GetDateTime(string prompt, DateTime? minDate = null)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Console.Write(">>");

            if (DateTime.TryParse(Console.ReadLine(), out var dateTime) &&
                (!minDate.HasValue || dateTime > minDate.Value))
            {
                return dateTime;
            }

            Console.WriteLine("Неправильно введена дата, попробуйте снова.");
            ConsoleHelper.PressAnyKey();
        }
    }

    private int? GetNotificationMinutes()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Укажите время до начала встречи, за которое нужно уведомить (в минутах).");
            Console.WriteLine("Если уведомление не нужно, введите 'Н':");
            Console.Write(">>");

            var input = Console.ReadLine();
            if (input.Equals("Н", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            if (int.TryParse(input, out int minutes))
            {
                return minutes;
            }

            Console.WriteLine("Неправильно введено число, попробуйте снова.");
            ConsoleHelper.PressAnyKey();
        }
    }
}

public class ViewMeetingsOption : MenuOption
{
    private readonly Schedule schedule;

    public ViewMeetingsOption(Schedule schedule) : base("Посмотреть все встречи за конкретную дату")
    {
        this.schedule = schedule;
    }

    public override void Execute()
    {
        DateOnly chosenDate = GetDate("Введите дату, за которую нужно вывести все встречи (или '1' для сегодняшней даты):");
        MeetingHelper.ListMeetings(schedule, chosenDate);
    }

    private DateOnly GetDate(string prompt)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Console.Write(">>");

            var input = Console.ReadLine();
            if (input == "1")
            {
                return DateOnly.FromDateTime(DateTime.Now);
            }

            if (DateOnly.TryParse(input, out var date))
            {
                return date;
            }

            Console.WriteLine("Неправильно введена дата, попробуйте снова.");
            ConsoleHelper.PressAnyKey();
        }
    }
}

public class ExitOption : MenuOption
{
    public ExitOption() : base("Закрыть приложение") { }

    public override void Execute()
    {
        Environment.Exit(0);
    }
}

public class MenuManager
{
    private readonly List<MenuOption> options;

    public MenuManager(List<MenuOption> options)
    {
        this.options = options;
    }

    public void Run()
    {
        int selectedIndex = 0;

        while (true)
        {
            WriteMenu(options, options[selectedIndex]);

            var keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex + 1) % options.Count;
            }
            else if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex - 1 + options.Count) % options.Count;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                options[selectedIndex].Execute();
                selectedIndex = 0;
            }
        }
    }

    private void WriteMenu(List<MenuOption> options, MenuOption selectedOption)
    {
        Console.Clear();
        Console.WriteLine("Приложение управления встречами. Выберите опцию:");
        foreach (var option in options)
        {
            if (option == selectedOption)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("> ");
            }
            else
            {
                Console.ResetColor();
                Console.Write("  ");
            }
            Console.WriteLine(option.Name);
        }
        Console.ResetColor();
    }
}

public class MeetingMenuOption : MenuOption
{
    private readonly Schedule schedule;
    private readonly Meeting meeting;
    private readonly DateOnly chosenDate;
    private readonly int originalIndex;

    public MeetingMenuOption(Schedule schedule, Meeting meeting, DateOnly chosenDate, int originalIndex)
        : base($"Меню встречи ({meeting.StartTime} - {meeting.EndTime})")
    {
        this.schedule = schedule;
        this.meeting = meeting;
        this.chosenDate = chosenDate;
        this.originalIndex = originalIndex;
    }

    public override void Execute()
    {
        var options = new List<MenuOption>
        {
            new EditMeetingOption(schedule, meeting, chosenDate, originalIndex),
            new DeleteMeetingOption(schedule, meeting, chosenDate, originalIndex),
            new BackToMeetingsOption(chosenDate, schedule)
        };

        var menuManager = new MenuManager(options);
        menuManager.Run();
    }
}

public class EditMeetingOption : MenuOption
{
    private readonly Schedule schedule;
    private readonly Meeting meeting;
    private readonly DateOnly chosenDate;
    private readonly int id;

    public EditMeetingOption(Schedule schedule, Meeting meeting, DateOnly chosenDate, int id)
        : base("Изменить встречу")
    {
        this.schedule = schedule;
        this.meeting = meeting;
        this.chosenDate = chosenDate;
        this.id = id;
    }

    public override void Execute()
    {
        var newMeeting = DateTimeMeetingUI();
        if (schedule.CorrectMeeting(newMeeting, id))
        {
            ConsoleHelper.ShowMessage("Встреча была успешно изменена");
        }
        else
        {
            ConsoleHelper.ShowMessage("На это время назначена другая встреча. Встреча не была изменена");
        }
        MeetingHelper.ListMeetings(schedule, chosenDate);
    }

    private Meeting DateTimeMeetingUI()
    {
        DateTime startTime = GetDateTime("Укажите дату и время начала встречи (дд.мм.гггг чч:мм):");
        DateTime endTime = GetDateTime("Укажите дату и время конца встречи (дд.мм.гггг чч:мм):", startTime);
        int? notifMin = GetNotificationMinutes();
        return notifMin.HasValue
            ? new Meeting(startTime, endTime, notifMin.Value)
            : new Meeting(startTime, endTime);
    }

    private DateTime GetDateTime(string prompt, DateTime? minDate = null)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(prompt);
            Console.Write(">>");
            if (DateTime.TryParse(Console.ReadLine(), out var dateTime) &&
                (!minDate.HasValue || dateTime > minDate.Value))
            {
                return dateTime;
            }
            Console.WriteLine("Неправильно введена дата, попробуйте снова.");
            ConsoleHelper.PressAnyKey();
        }
    }

    private int? GetNotificationMinutes()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Укажите время до начала встречи, за которое нужно уведомить (в минутах).");
            Console.WriteLine("Если уведомление не нужно, введите 'Н':");
            Console.Write(">>");
            var input = Console.ReadLine();
            if (input.Equals("Н", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }
            if (int.TryParse(input, out int minutes))
            {
                return minutes;
            }
            Console.WriteLine("Неправильно введено число, попробуйте снова.");
            ConsoleHelper.PressAnyKey();
        }
    }
}

public class DeleteMeetingOption : MenuOption
{
    private readonly Schedule schedule;
    private readonly Meeting meeting;
    private readonly DateOnly chosenDate;
    private readonly int id;

    public DeleteMeetingOption(Schedule schedule, Meeting meeting, DateOnly chosenDate, int id)
        : base("Удалить встречу")
    {
        this.schedule = schedule;
        this.meeting = meeting;
        this.chosenDate = chosenDate;
        this.id = id;
    }

    public override void Execute()
    {
        ConfirmDeletion(id, chosenDate, meeting);
    }

    private void ConfirmDeletion(int id, DateOnly chosenDate, Meeting meeting)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Вы уверены что хотите удалить эту встречу?");
            Console.WriteLine("Введите Да/Нет");
            Console.Write(">>");

            var input = Console.ReadLine();
            if (input.Equals("Да", StringComparison.OrdinalIgnoreCase))
            {
                schedule.DeleteMeeting(id);
                ConsoleHelper.ShowMessage("Удаление успешно.");
                MeetingHelper.ListMeetings(schedule, chosenDate);
                return;
            }
            else if (input.Equals("Нет", StringComparison.OrdinalIgnoreCase))
            {
                new MeetingMenuOption(schedule, meeting, chosenDate, id).Execute();
                return;
            }
            else
            {
                Console.WriteLine("Неверный ввод, попробуйте снова");
                Thread.Sleep(3000);
            }
        }
    }
}

public class BackToMeetingsOption : MenuOption
{
    private readonly DateOnly chosenDate;
    private readonly Schedule schedule;

    public BackToMeetingsOption(DateOnly chosenDate, Schedule schedule)
        : base("Назад")
    {
        this.chosenDate = chosenDate;
        this.schedule = schedule;
    }

    public override void Execute()
    {
        MeetingHelper.ListMeetings(schedule, chosenDate);
    }
}

public static class MeetingHelper
{
    public static void ListMeetings(Schedule schedule, DateOnly chosenDate)
    {
        while (true)
        {
            var meetingsWithIndices = schedule.GetMeetings(chosenDate);
            if (meetingsWithIndices.Count == 0)
            {
                ConsoleHelper.ShowMessage("На эту дату нет запланированных встреч.");
                return;
            }

            Console.Clear();
            Console.WriteLine("Список встреч:");
            for (int i = 0; i < meetingsWithIndices.Count; i++)
            {
                var (meeting, _) = meetingsWithIndices[i];
                Console.WriteLine($"{i}: {meeting.StartTime} - {meeting.EndTime}");
            }

            Console.WriteLine("\nВведите номер встречи для выбора, 'М' для возврата в главное меню или 'Э' для экспорта встреч:");
            Console.Write(">>");

            var input = Console.ReadLine();
            if (input.Equals("М", StringComparison.OrdinalIgnoreCase))
            {
                var mainMenuOptions = new List<MenuOption>
                {
                    new CreateMeetingOption(schedule),
                    new ViewMeetingsOption(schedule),
                    new ExitOption()
                };
                var menuManager = new MenuManager(mainMenuOptions);
                menuManager.Run();
                return;
            }
            else if (input.Equals("Э", StringComparison.OrdinalIgnoreCase))
            {
                ExportMeetingsToFile(meetingsWithIndices, chosenDate);
                ConsoleHelper.ShowMessage("Экспорт завершен. Нажмите любую клавишу для продолжения.");
                continue;
            }

            if (int.TryParse(input, out int id) && id >= 0 && id < meetingsWithIndices.Count)
            {
                var (selectedMeeting, originalIndex) = meetingsWithIndices[id];
                new MeetingMenuOption(schedule, selectedMeeting, chosenDate, originalIndex).Execute();
                return;
            }
            else
            {
                Console.WriteLine("Неверный ввод, попробуйте снова.");
                Thread.Sleep(3000);
            }
        }
    }
    
    private static void ExportMeetingsToFile(List<(Meeting Meeting, int Index)> meetings, DateOnly chosenDate)
    {
        string filePath = $"meetings_{chosenDate}.txt";
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine($"Встречи за {chosenDate}:");
                foreach (var (meeting, _) in meetings)
                {
                    writer.WriteLine($"- {meeting.StartTime} - {meeting.EndTime}. Уведомление за: {meeting.Notification} минут");
                }
            }
        }
        catch (Exception ex)
        {
            ConsoleHelper.ShowMessage($"Ошибка при экспорте встреч: {ex.Message}");
        }
    }
}

public static class ConsoleHelper
{
    public static void PressAnyKey()
    {
        Console.WriteLine("\nНажмите любую клавишу для продолжения");
        Console.Write(">>");
        Console.ReadKey();
    }

    public static void ShowMessage(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        PressAnyKey();
    }
}