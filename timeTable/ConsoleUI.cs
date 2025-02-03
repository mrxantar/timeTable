namespace timeTable;

public class ConsoleUI ()
{
    public void MainMenuUI(Schedule schedule)
    {
        Console.Clear();
        Console.WriteLine("Приложение управления встречами. Выберите опцию с помощью клавиш с цифрами:");
        Console.WriteLine("1. Создать встречу");
        Console.WriteLine("2. Посмотреть все встречи за конкретную дату");
        Console.WriteLine("9. Закрыть приложение");
        Console.Write(">>");
        var keyInput = Console.ReadKey();

        switch (keyInput.Key)
        {
            case ConsoleKey.D1:
                CreationUI(schedule);
                break;
            case ConsoleKey.D2:
                ChooseDate(schedule);
                break;
            case ConsoleKey.D9:
                return;
            default:
                Console.Clear();
                Console.WriteLine("Такой опции не существует. Попробуйте снова");
                Thread.Sleep(3000);
                MainMenuUI(schedule);
                break;
        }
    }

    public void CreationUI(Schedule schedule)
    {
        DateTime startTime;
        DateTime endTime;
        int notifMin;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Укажите дату и время начала встречи");
            Console.WriteLine("в формате дд:мм:гггг чч:мм");
            Console.Write(">>");
            if (DateTime.TryParse(Console.ReadLine(), out startTime))
                break;
            Console.WriteLine("Неправильно введена дата, попробуйте снова.");
            Console.WriteLine("Нажмите любую клавишу для продолжения");
            Console.ReadKey();
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Укажите дату и время конца встречи встречи");
            Console.WriteLine("в формате дд:мм:гггг чч:мм");
            Console.Write(">>");
            if (DateTime.TryParse(Console.ReadLine(), out endTime))
                break;
            
            Console.WriteLine("Неправильно введена дата, попробуйте снова.");
            Console.WriteLine("Нажмите любую клавишу для продолжения");
            Console.Write(">>");
            Console.ReadKey();
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Укажите время до начала встречи, за которое нужно уведомить (в минутах)");
            Console.WriteLine("Если уведомление не нужно, введите Н(ет)");
            Console.Write(">>");
            
            var input = Console.ReadLine();
            switch (input)
            {
                case "Н":
                    Console.Clear();
                    schedule.AddMeeting(startTime, endTime, schedule);
                    Thread.Sleep(3000);
                    MainMenuUI(schedule);
                    break;
                default:
                    if (int.TryParse(input, out notifMin))
                    {
                        Console.Clear();
                        schedule.AddMeeting(startTime, endTime, schedule, notifMin);
                        Thread.Sleep(3000);
                        MainMenuUI(schedule);
                        break;
                    }
                    
                    Console.WriteLine("Неправильно введено число, попробуйте снова.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения");
                    Console.Write(">>");
                    Console.ReadKey();
                    continue;
            }
        }
    }

    public void CreationError(Schedule schedule)
    {
        Console.Clear();
        Console.WriteLine("На это время назначена другая встреча. Новая встреча не была создана.");
        Console.WriteLine("Нажмите любую клавишу для продолжения");
        Console.Write(">>");
        Console.ReadKey();
        MainMenuUI(schedule);
    }

    public void CreationSuccess(Schedule schedule)
    {
        Console.Clear();
        Console.WriteLine("Встреча успешно добавлена");
        Console.WriteLine("Нажмите любую клавишу для продолжения");
        Console.Write(">>");
        Console.ReadKey();
        MainMenuUI(schedule);
    }

    public void ChooseDate(Schedule schedule)
    {
        Console.Clear();
        Console.WriteLine("Введите дату, за которую нужно вывести все встречи");
        if (DateOnly.TryParse(Console.ReadLine(), out DateOnly choosenDate))
            ListMeetings(schedule, choosenDate);
        else
        {
            Console.WriteLine("Неправильно введена дата, попробуйте снова");
            Thread.Sleep(3000);
            ChooseDate(schedule);
        }
    }

    public void ListMeetings(Schedule schedule, DateOnly choosenDate)
    {
        Console.Clear();
        Console.WriteLine("Выберите номер встречи, введя её номер");
        Console.WriteLine("Для выбора другой даты, введите Д(ата)");
        Console.WriteLine("Для возвращения в главное меню, введите М(еню)");
        int i = 0;
        foreach (var meeting in schedule.GetMeetings(schedule, choosenDate))
        {
            Console.WriteLine($"{i++}: {meeting.StartTime} - {meeting.EndTime}. Уведомление за: {meeting.Notification} минут");
        }

        Console.Write(">>");
        var input = Console.ReadLine();
        switch (input)
        {
            case "М":
                MainMenuUI(schedule);
                break;
            case "Д":
                ChooseDate(schedule);
                break;
            default:
                if (int.TryParse(input, out i))
                {
                    var choosenMeeting = schedule.GetMeeting(schedule, i);
                    MeetingOptions(schedule, choosenMeeting, choosenDate);
                    break;
                }
                
                Console.WriteLine("Такой опции нет. Попробуйте снова");
                Console.WriteLine("Нажмите любую клавишу для продолжения");
                Console.Write(">>");
                Console.ReadKey();
                ListMeetings(schedule, choosenDate);
                break;
        }
    }

    public void MeetingOptions(Schedule schedule, Meeting meeting, DateOnly choosenDate)
    {
        Console.Clear();
        Console.WriteLine($"Начало встречи:{meeting.StartTime}");
        Console.WriteLine($"Конец встречи:{meeting.EndTime}");
        Console.WriteLine($"Уведомить за {meeting.Notification} минут до начала\n\n");
        
        Console.WriteLine("1. Изменить встречу");
        Console.WriteLine("5. Удалить встречу");
        Console.WriteLine("8. Вернуться к списку встреч");
        Console.WriteLine("9. Вернуться в главное меню");
        Console.Write(">>");
        
        var input = Console.ReadKey();
        switch (input.Key)
        {
            case ConsoleKey.D1:
                //Изменить встречу
                break;
            case ConsoleKey.D5:
                //Удалить встречу
            case ConsoleKey.D8:
                ListMeetings(schedule, choosenDate);
                break;
            case ConsoleKey.D9:
                MainMenuUI(schedule);
                break;
            default:
                Console.WriteLine("Такой опции нет. Попробуйте снова");
                Console.WriteLine("Нажмите любую клавишу для продолжения");
                Console.ReadKey();
                MeetingOptions(schedule, meeting, choosenDate);
                break;
        }
    }
}