using System;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Domain.Entities.State;
using VolunteerTaskManagement.Domain.Enums;
using Xunit;

namespace VolunteerTaskManagement.Application.UnitTests.State;

public class ConfirmedStateTests
{
    [Fact]
    public void Start_Should_Throw_InvalidOperationException()
    {
        // Arrange
        var task = new VolunteerTask();
        task.State = new ConfirmedState();
        task.Status = VolunteerTaskStatus.Confirmed;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            task.State.Start(task));

        Assert.Equal("تغییر وضعیت مجاز نیست", exception.Message);
    }

    [Fact]
    public void Confirm_Should_Throw_InvalidOperationException()
    {
        // Arrange
        var task = new VolunteerTask();
        task.State = new ConfirmedState();
        task.Status = VolunteerTaskStatus.Confirmed;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            task.State.Confirm(task));

        Assert.Equal("تغییر وضعیت مجاز نیست", exception.Message);
    }

    [Fact]
    public void Cancel_Should_Throw_InvalidOperationException()
    {
        // Arrange
        var task = new VolunteerTask();
        task.State = new ConfirmedState();
        task.Status = VolunteerTaskStatus.Confirmed;

        // Act & Assert
        var exception = Assert.Throws<InvalidOperationException>(() =>
            task.State.Cancel(task));

        Assert.Equal("تغییر وضعیت مجاز نیست", exception.Message);
    }
}