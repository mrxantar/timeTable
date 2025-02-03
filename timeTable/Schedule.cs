namespace timeTable;

public class Schedule
{
    private List<Meeting> meetings = new List<Meeting>();

    public void AddMeeting(DateTime startTime, DateTime endTime, Schedule schedule, int notification = 0)
    {
        if (!meetings.Any(meeting => startTime < meeting.EndTime && endTime > meeting.StartTime))
        {
            meetings.Add(new Meeting(startTime, endTime, notification));
            new ConsoleUI().CreationSuccess(schedule);
        }
        else
            new ConsoleUI().CreationError(schedule);
    }
}