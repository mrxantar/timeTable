namespace timeTable;

public class ConsoleUI (Schedule schedule)
{
    private void PressAnyKey()
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
                        ShowMessage("Встреча успешно добавлена");
                        MainMenuUI();
                    }
                    else
                    {
                        ShowMessage("На это время назначена другая встреча. Новая встреча не была создана");
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

    private Meeting DateTimeMeetingUI()
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
                if (endTime > startTime)
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
    
    private void ShowMessage(string message)
    {
        Console.Clear();
        Console.WriteLine(message);
        PressAnyKey();
    }
    
    private void ConfirmDeletion(int id, DateOnly chosenDate, Meeting meeting)
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
                ShowMessage("Удаление успешно.");
                ListMeetings(chosenDate);
                break;
            case "Нет":
                MeetingOptions(meeting, chosenDate, id);
                break;
            default:
                Console.WriteLine("Неверный ввод, попробуйте снова");
                Thread.Sleep(3000);
                ConfirmDeletion(id, chosenDate, meeting);
                break;
        }
    }

    private void ChooseDate()
    {
        DateOnly chosenDate;
        Console.Clear();
        Console.WriteLine("Введите дату, за которую нужно вывести все встречи");
        Console.WriteLine("Или введите 1 для выбора сегодняшней даты");
        Console.WriteLine("Введите Н(азад) для возврата в главное меню");
        Console.Write(">>");
        
        var input = Console.ReadLine();
        while (true)
        {
            switch (input)
            {
                case "1":
                    chosenDate  = DateOnly.FromDateTime(DateTime.Now);
                    ListMeetings(chosenDate);
                    break;
                case "Н":
                    MainMenuUI();
                    break;
                default:
                    if (DateOnly.TryParse(Console.ReadLine(), out chosenDate))
                    {
                        ListMeetings(chosenDate);
                        break;
                    }
                    Console.WriteLine("Неправильно введена дата, попробуйте снова");
                    Thread.Sleep(3000);
                    continue;
            }
        }
    }

    private void ListMeetings(DateOnly chosenDate)
    {
        Console.Clear();
        Console.WriteLine("Выберите номер встречи, введя её номер");
        Console.WriteLine("Для выбора другой даты, введите Д(ата)");
        Console.WriteLine("Для возвращения в главное меню, введите М(еню)");
        int i = 0;
        foreach (var meeting in schedule.GetMeetings(chosenDate))
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
                    MeetingOptions(choosenMeeting, chosenDate, i);
                    break;
                }
                
                Console.WriteLine("Такой опции нет. Попробуйте снова");
                PressAnyKey();
                ListMeetings(chosenDate);
                break;
        }
    }

    private void MeetingOptions(Meeting meeting, DateOnly chosenDate, int id)
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
                    ShowMessage("Встреча была успешно изменена");
                    ListMeetings(chosenDate);
                }
                else
                {
                    ShowMessage("На это время назначена другая встреча. Встреча не была изменена");
                    MeetingOptions(meeting, chosenDate, id);
                }
                break;
            
            case ConsoleKey.D5:
                ConfirmDeletion(id, chosenDate, meeting);
                break;
            
            case ConsoleKey.D8:
                ListMeetings(chosenDate);
                break;
            
            case ConsoleKey.D9:
                MainMenuUI();
                break;
            
            default:
                Console.WriteLine("Такой опции нет. Попробуйте снова");
                PressAnyKey();
                MeetingOptions(meeting, chosenDate, id);
                break;
        }
    }

    public void Notification(string notification)
    {
        Console.Beep();
        Console.WriteLine();
    }
}