using Timer = System.Timers.Timer;
using System.Timers;
namespace timeTable;


public class Schedule
{
    private List<Meeting> meetings = new List<Meeting>();
    
    private Timer timer = new Timer(60000);

    public Schedule()
    {
        timer.Elapsed += CheckNotification;
        timer.Start();
    }

    public bool AddMeeting(Meeting bufferMeeting)
    {
        if (!meetings.Any(meeting => bufferMeeting.StartTime < meeting.EndTime && bufferMeeting.EndTime > meeting.StartTime))
        {
            meetings.Add(bufferMeeting);
            return true;
        }
        return false;
    }

    public List<(Meeting Meeting, int Index)> GetMeetings(DateOnly chosenDate)
    {
        var chosenMeetings = new List<(Meeting Meeting, int Index)>();
        for (int i = 0; i < meetings.Count; i++)
        {
            if (DateOnly.FromDateTime(meetings[i].StartTime.Date) == chosenDate)
            {
                chosenMeetings.Add((meetings[i], i));
            }
        }
        return chosenMeetings;
    }

    public Meeting GetMeeting(int id)
    {
        return (meetings[id]);
    }

    public bool CorrectMeeting(Meeting bufferMeeting, int originalIndex)
    {
        if (!meetings.Where((meeting, index) => index != originalIndex)
                .Any(meeting => bufferMeeting.StartTime < meeting.EndTime && bufferMeeting.EndTime > meeting.StartTime))
        {
            meetings[originalIndex].StartTime = bufferMeeting.StartTime;
            meetings[originalIndex].EndTime = bufferMeeting.EndTime;
            meetings[originalIndex].Notification = bufferMeeting.Notification;
            return true;
        }
        return false;
    }

    public void DeleteMeeting(int id)
    {
        meetings.RemoveAt(id);
    }

    private void CheckNotification(object? sender, ElapsedEventArgs elapsedEventArgs)
    {
        
    }

    private string CreateNotification()
    {
        Meeting notifMeeting = meetings.Where(meetings => meetings.Notification > 0)
            .FirstOrDefault(meetings => (meetings.StartTime - DateTime.Now).TotalMinutes < meetings.Notification);
        if (notifMeeting == null)
            return "";
        
        string notification = notifMeeting.StartTime.ToString("HH:mm") + " - " + notifMeeting.EndTime.ToString("HH:mm");
        return notification;
    }

}