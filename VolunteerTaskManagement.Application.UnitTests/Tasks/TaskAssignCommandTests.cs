using Base.Application.Contracts;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System.Threading.Tasks;
using VolunteerTaskManagement.Application.Contracts;
using VolunteerTaskManagement.Domain.Entities;
using VolunteerTaskManagement.Infrastructure.Persistence.Context;
using Xunit;

namespace VolunteerTaskManagement.Application.UnitTests
{
    public class TaskAssignCommandTests
    {
        [Fact]
        public async Task VolunteerTask_Should_Throw_DbUpdateConcurrencyException_When_Updated_Concurrently()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<VolunteerTaskManagementContext>()
                .UseSqlServer("Server=localhost,1400;Database=VolunteerTaskManagement;User Id=sa;Password=@VolunteerTaskManagement04;TrustServerCertificate=True;Encrypt=False")
                .Options;

            var jwtManager = Substitute.For<IJwtManager>();
            jwtManager.GetUserId().Returns(20014);

            using var context1 = new VolunteerTaskManagementContext(options, jwtManager);
            using var context2 = new VolunteerTaskManagementContext(options, jwtManager);

            var task1 = await context1.Set<VolunteerTask>()
                .FirstAsync(x => x.Id == 20002);

            var task2 = await context2.Set<VolunteerTask>()
                .FirstAsync(x => x.Id == 20002);

            // Context دوم رکورد را تغییر می‌دهد
            task2.VolunteerCount++;
            await context2.SaveChangesAsync();

            // Context اول روی نسخه قدیمی همان رکورد تغییر اعمال می‌کند
            task1.VolunteerCount++;

            // Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () =>
            {
                await context1.SaveChangesAsync();
            });
        }
    }
}