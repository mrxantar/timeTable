namespace timeTable;

public class ConsoleUI (Schedule schedule)
{
    public void PressAnyKey()
    {
        Console.WriteLine("Нажмите любую клавишу для продолжения");
        Console.Write(">>");
        Console.ReadKey();
    }

    public void MainMenuUI()
    {
        while (true)
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
                    if (schedule.AddMeeting(DateTimeMeetingUI()))
                    {
                        CreationSuccess();
                        MainMenuUI();
                    }
                    else
                    {
                        CreationError();
                        MainMenuUI();
                    }
                    break;
            
                case ConsoleKey.D2:
                    ChooseDate();
                    break;
            
                case ConsoleKey.D9:
                    return;
            
                default:
                    Console.Clear();
                    Console.WriteLine("Такой опции не существует. Попробуйте снова");
                    Thread.Sleep(3000);
                    continue;
            }
        }
        
    }

    public Meeting DateTimeMeetingUI()
    {
        DateTime startTime;
        DateTime endTime;
        int notifMin;
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Укажите дату и время начала встречи");
            Console.WriteLine("в формате дд.мм.гггг чч:мм");
            Console.Write(">>");
            if (DateTime.TryParse(Console.ReadLine(), out startTime))
                break;
            Console.WriteLine("Неправильно введена дата, попробуйте снова.");
            PressAnyKey();
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Укажите дату и время конца встречи встречи");
            Console.WriteLine("в формате дд.мм.гггг чч:мм");
            Console.Write(">>");
            if (DateTime.TryParse(Console.ReadLine(), out endTime))
                break;
            
            Console.WriteLine("Неправильно введена дата, попробуйте снова.");
            PressAnyKey();
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
                    var bufferMeeting = new Meeting(startTime, endTime);
                    return bufferMeeting;
                default:
                    if (int.TryParse(input, out notifMin))
                    {
                        Console.Clear();
                        var bufferMeetingWithNotif = new Meeting(startTime, endTime, notifMin);
                        return bufferMeetingWithNotif;
                    }
                    
                    Console.WriteLine("Неправильно введено число, попробуйте снова.");
                    PressAnyKey();
                    continue;
            }
        }
    }

    public void CreationError()
    {
        Console.Clear();
        Console.WriteLine("На это время назначена другая встреча. Новая встреча не была создана.");
        PressAnyKey();
    }

    public void CreationSuccess()
    {
        Console.Clear();
        Console.WriteLine("Встреча успешно добавлена");
        PressAnyKey();
    }

    public void CorrectionError()
    {
        Console.Clear();
        Console.WriteLine("На это время назначена другая встреча. Встреча не была изменена.");
        PressAnyKey();
    }

    public void CorrectionSuccess()
    {
        Console.Clear();
        Console.WriteLine("Встреча была успешно изменена");
        PressAnyKey();
    }

    public void ConfirmDeletion(int id, DateOnly choosenDate, Meeting meeting)
    {
        Console.Clear();
        Console.WriteLine("Вы уверены что хотите удалить эту встречу?");
        Console.WriteLine("Введите Да/Нет");
        Console.Write(">>");
        var input = Console.ReadLine();
        switch (input)
        {
            case "Да":
                schedule.DeleteMeeting(id);
                Console.WriteLine("Удаление успешно.");
                PressAnyKey();
                ListMeetings(choosenDate);
                break;
            case "Нет":
                MeetingOptions(meeting, choosenDate, id);
                break;
            default:
                Console.WriteLine("Неверный ввод, попробуйте снова");
                Thread.Sleep(3000);
                ConfirmDeletion(id, choosenDate, meeting);
                break;
        }
    }

    public void ChooseDate()
    {
        Console.Clear();
        Console.WriteLine("Введите дату, за которую нужно вывести все встречи");
        if (DateOnly.TryParse(Console.ReadLine(), out DateOnly choosenDate))
            ListMeetings(choosenDate);
        else
        {
            Console.WriteLine("Неправильно введена дата, попробуйте снова");
            Thread.Sleep(3000);
            ChooseDate();
        }
    }

    public void ListMeetings(DateOnly choosenDate)
    {
        Console.Clear();
        Console.WriteLine("Выберите номер встречи, введя её номер");
        Console.WriteLine("Для выбора другой даты, введите Д(ата)");
        Console.WriteLine("Для возвращения в главное меню, введите М(еню)");
        int i = 0;
        foreach (var meeting in schedule.GetMeetings(choosenDate))
        {
            Console.WriteLine($"{i++}: {meeting.StartTime} - {meeting.EndTime}. Уведомление за: {meeting.Notification} минут");
        }

        Console.Write(">>");
        var input = Console.ReadLine();
        switch (input)
        {
            case "М":
                MainMenuUI();
                break;
            case "Д":
                ChooseDate();
                break;
            default:
                if (int.TryParse(input, out i))
                {
                    var choosenMeeting = schedule.GetMeeting(i);
                    MeetingOptions(choosenMeeting, choosenDate, i);
                    break;
                }
                
                Console.WriteLine("Такой опции нет. Попробуйте снова");
                PressAnyKey();
                ListMeetings(choosenDate);
                break;
        }
    }

    public void MeetingOptions(Meeting meeting, DateOnly choosenDate, int id)
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
                if (schedule.CorrectMeeting(DateTimeMeetingUI(), id))
                {
                    CorrectionSuccess();
                    ListMeetings(choosenDate);
                }
                else
                {
                    CorrectionError();
                    MeetingOptions(meeting, choosenDate, id);
                }
                break;
            
            case ConsoleKey.D5:
                ConfirmDeletion(id, choosenDate, meeting);
                break;
            
            case ConsoleKey.D8:
                ListMeetings(choosenDate);
                break;
            
            case ConsoleKey.D9:
                MainMenuUI();
                break;
            
            default:
                Console.WriteLine("Такой опции нет. Попробуйте снова");
                PressAnyKey();
                MeetingOptions(meeting, choosenDate, id);
                break;
        }
    }
    
}