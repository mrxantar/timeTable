namespace timeTable;

public class Meeting
{
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int Notification { get; set; }
    
    public Meeting(DateTime startTime, DateTime endTime, int notification = 0)
    {
        StartTime = startTime;
        EndTime = endTime;
        Notification = notification;
    }
}