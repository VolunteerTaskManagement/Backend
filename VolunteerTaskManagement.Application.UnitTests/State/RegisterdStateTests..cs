using System;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Domain.Entities.State;
using VolunteerTaskManagement.Domain.Enums;
using Xunit;

namespace VolunteerTaskManagement.Application.UnitTests.State;

public class RegisterdStateTests
{
    [Fact]
    public void Start_Should_Change_State_To_InProgress()
    {
        // Arrange
        var task = new VolunteerTask();
        task.State = new RegisterdState();
        task.Status = VolunteerTaskStatus.Registered;

        // Act
        task.State.Start(task);

        // Assert
        Assert.IsType<InProgressState>(task.State);
        Assert.Equal(VolunteerTaskStatus.InProgress, task.Status);
    }

    [Fact]
    public void Cancel_Should_Change_State_To_Cancelled()
    {
        // Arrange
        var task = new VolunteerTask();
        task.State = new RegisterdState();
        task.Status = VolunteerTaskStatus.Registered;

        // Act
        task.State.Cancel(task);

        // Assert
        Assert.IsType<CancelledState>(task.State);
        Assert.Equal(VolunteerTaskStatus.Cancelled, task.Status);
    }

    [Fact]
    public void Confirm_Should_Throw_InvalidOperationException()
    {
        // Arrange
        var task = new VolunteerTask();
        task.State = new RegisterdState();
        task.Status = VolunteerTaskStatus.Registered;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            task.State.Confirm(task));

        Assert.Equal("تغییر وضعیت مجاز نیست", exception.Message);
    }
}