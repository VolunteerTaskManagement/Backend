using System;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Domain.Entities.State;
using VolunteerTaskManagement.Domain.Enums;
using Xunit;

namespace VolunteerTaskManagement.Application.UnitTests.State;

public class InProgressStateTests
{
    [Fact]
    public void Confirm_Should_Change_State_To_Confirmed()
    {
        // Arrange
        var task = new VolunteerTask();
        task.State = new InProgressState();
        task.Status = VolunteerTaskStatus.InProgress;

        // Act
        task.State.Confirm(task);

        // Assert
        Assert.IsType<ConfirmedState>(task.State);
        Assert.Equal(VolunteerTaskStatus.Confirmed, task.Status);
    }

    [Fact]
    public void Cancel_Should_Change_State_To_Cancelled()
    {
        // Arrange
        var task = new VolunteerTask();
        task.State = new InProgressState();
        task.Status = VolunteerTaskStatus.InProgress;

        // Act
        task.State.Cancel(task);

        // Assert
        Assert.IsType<CancelledState>(task.State);
        Assert.Equal(VolunteerTaskStatus.Cancelled, task.Status);
    }

    [Fact]
    public void Start_Should_Throw_InvalidOperationException()
    {
        // Arrange
        var task = new VolunteerTask();
        task.State = new InProgressState();
        task.Status = VolunteerTaskStatus.InProgress;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            task.State.Start(task));

        Assert.Equal("تغییر وضعیت مجاز نیست", exception.Message);
    }
}