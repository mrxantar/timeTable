namespace timeTable;

public class ConsoleUI
{
    public void MainMenuUI()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Приложение управления встречами. Выберите опцию с помощью клавиш с цифрами:");
            Console.WriteLine("1. Создать встречу");
            Console.WriteLine("2. Посмотреть все встречи за конкретную дату");
            Console.WriteLine("9. Закрыть приложение");
            var keyInput = Console.ReadKey();
            
            switch (keyInput.Key)
            {
                case ConsoleKey.D1:
                    CreationUI();
                    break;
                case ConsoleKey.D2:
                    // ChangeMeeting();
                    break;
                case ConsoleKey.D9:
                    break;
                default:
                    Console.WriteLine("Такой опции не существует. Попробуйте снова");
                    Thread.Sleep(3000);
                    continue;
            }
            return;
        }
    }

    public void CreationUI()
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
            Console.ReadKey();
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Укажите время до начала встречи, за которое нужно уведомить (в минутах)");
            Console.WriteLine("Если уведомление не нужно, введите Н(ет)");
            
            var input = Console.ReadLine();
            switch (input)
            {
                case "Н":
                    Console.Clear();
                    AddMeeting(startTime, endTime);
                    Console.WriteLine("Встреча успешно добавлена");
                    Thread.Sleep(5000);
                    MainMenuUI();
                    break;
                default:
                    if (int.TryParse(input, out notifMin))
                    {
                        Console.Clear();
                        var meetingWithNotif = new Meeting(startTime, endTime, notifMin);
                        Console.WriteLine("Встреча успешно добавлена");
                        Thread.Sleep(7000);
                        MainMenuUI();
                        break;
                    }
                    
                    Console.WriteLine("Неправильно введено число, попробуйте снова.");
                    Console.WriteLine("Нажмите любую клавишу для продолжения");
                    Console.ReadKey();
                    continue;
            }
        }
    }
}