namespace timeTable;

public class Schedule
{
    private List<Meeting> meetings = new List<Meeting>();

    public bool AddMeeting(Meeting bufferMeeting)
    {
        if (!meetings.Any(meeting => bufferMeeting.StartTime < meeting.EndTime && bufferMeeting.EndTime > meeting.StartTime))
        {
            meetings.Add(bufferMeeting);
            return true;
        }
        return false;
    }

    public List<Meeting> GetMeetings(DateOnly choosenDate)
    {
        var choosenMeetings = new List<Meeting>();
        choosenMeetings = meetings.Where(meetings => DateOnly.FromDateTime(meetings.StartTime.Date) == choosenDate)
            .ToList();
        return choosenMeetings;
    }

    public Meeting GetMeeting(int id)
    {
        return (meetings[id]);
    }

    public bool CorrectMeeting(Meeting bufferMeeting, int id)
    {
        if (!meetings.Any(meeting => bufferMeeting.StartTime < meeting.EndTime && bufferMeeting.EndTime > meeting.StartTime))
        {
            meetings[id].StartTime = bufferMeeting.StartTime;
            meetings[id].EndTime = bufferMeeting.EndTime;
            meetings[id].Notification = bufferMeeting.Notification;
            return true;
        }
        return false;
    }

    public void DeleteMeeting(int id)
    {
        meetings.RemoveAt(id);
    }
}