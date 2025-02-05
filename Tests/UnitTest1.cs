using timeTable;

namespace Tests;

public class UnitTest1
{
    [Fact]
    public void AddMeeting_ShouldAddMeeting_WhenNoOverlap()
    {
        var schedule = new Schedule();
        var meeting = new Meeting(
            startTime: new DateTime(2023, 10, 10, 15, 0, 0),
            endTime: new DateTime(2023, 10, 10, 16, 0, 0)
        );
        
        bool result = schedule.AddMeeting(meeting);
        
        Assert.True(result);
    }

    [Fact]
    public void AddMeeting_ShouldNotAddMeeting_WhenOverlapExists()
    {
        var schedule = new Schedule();
        var meeting1 = new Meeting(
            startTime: new DateTime(2023, 10, 10, 15, 0, 0),
            endTime: new DateTime(2023, 10, 10, 16, 0, 0)
        );
        var meeting2 = new Meeting(
            startTime: new DateTime(2023, 10, 10, 15, 30, 0),
            endTime: new DateTime(2023, 10, 10, 16, 30, 0)
        );

        schedule.AddMeeting(meeting1);
        
        bool result = schedule.AddMeeting(meeting2);
        
        Assert.False(result);
    }

    [Fact]
    public void GetMeetings_ShouldReturnMeetingsForChosenDate()
    {
        var schedule = new Schedule();
        var meeting1 = new Meeting(
            startTime: new DateTime(2023, 10, 10, 15, 0, 0),
            endTime: new DateTime(2023, 10, 10, 16, 0, 0)
        );
        var meeting2 = new Meeting(
            startTime: new DateTime(2023, 10, 11, 10, 0, 0),
            endTime: new DateTime(2023, 10, 11, 11, 0, 0)
        );

        schedule.AddMeeting(meeting1);
        schedule.AddMeeting(meeting2);
        
        var meetings = schedule.GetMeetings(new DateOnly(2023, 10, 10));
        
        Assert.Single(meetings);
        Assert.Equal(meeting1.StartTime, meetings[0].Meeting.StartTime);
    }

    [Fact]
    public void DeleteMeeting_ShouldRemoveMeeting()
    {
        var schedule = new Schedule();
        var meeting = new Meeting(
            startTime: new DateTime(2023, 10, 10, 15, 0, 0),
            endTime: new DateTime(2023, 10, 10, 16, 0, 0)
        );

        schedule.AddMeeting(meeting);
        
        schedule.DeleteMeeting(0);
        var meetings = schedule.GetMeetings(new DateOnly(2023, 10, 10));
        
        Assert.Empty(meetings);
    }
    
    [Fact]
    public void Meeting_ShouldInitializeCorrectly()
    {
        var meeting = new Meeting(
            startTime: new DateTime(2023, 10, 10, 15, 0, 0),
            endTime: new DateTime(2023, 10, 10, 16, 0, 0),
            notification: 15
        );
        
        Assert.Equal(new DateTime(2023, 10, 10, 15, 0, 0), meeting.StartTime);
        Assert.Equal(new DateTime(2023, 10, 10, 16, 0, 0), meeting.EndTime);
        Assert.Equal(15, meeting.Notification);
    }
    
    [Fact]
    public void CorrectMeeting_ShouldUpdateMeeting_WhenNoOverlap()
    {
        var schedule = new Schedule();
        var meeting1 = new Meeting(
            startTime: new DateTime(2023, 10, 10, 15, 0, 0),
            endTime: new DateTime(2023, 10, 10, 16, 0, 0)
        );
        var meeting2 = new Meeting(
            startTime: new DateTime(2023, 10, 10, 17, 0, 0),
            endTime: new DateTime(2023, 10, 10, 18, 0, 0)
        );

        schedule.AddMeeting(meeting1);
        schedule.AddMeeting(meeting2);

        var updatedMeeting = new Meeting(
            startTime: new DateTime(2023, 10, 10, 16, 30, 0),
            endTime: new DateTime(2023, 10, 10, 17, 0, 0)
        );
        
        bool result = schedule.CorrectMeeting(updatedMeeting, 0);
        
        Assert.True(result);
        Assert.Equal(updatedMeeting.StartTime, schedule.GetMeetings(new DateOnly(2023, 10, 10))[0].Meeting.StartTime);
        Assert.Equal(updatedMeeting.EndTime, schedule.GetMeetings(new DateOnly(2023, 10, 10))[0].Meeting.EndTime);
    }
}