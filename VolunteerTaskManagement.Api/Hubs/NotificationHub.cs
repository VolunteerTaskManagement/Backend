using Base.Application.Contracts;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace VolunteerTaskManagement.Api.Hubs
{
    [EnableCors("notif")]
    public class NotificationHub(IServiceProvider serviceProvider) : Hub
    {
        private static readonly ConcurrentDictionary<long, HashSet<string>> _userConnections = new();
        private readonly IJwtManager jwtManager = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<IJwtManager>();

        protected long? GetUserIdFromToken()
        {
            try
            {
                var accessToken = Context.GetHttpContext()?.Request.Query["access_token"];
                return jwtManager.GetUserId(accessToken);
            }
            catch
            {
                return null;
            }
        }

        public override Task OnConnectedAsync()
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId.HasValue)
                {
                    _userConnections.AddOrUpdate(
                        userId.Value,
                        [Context.ConnectionId],
                        (key, existingSet) =>
                        {
                            existingSet.Add(Context.ConnectionId);
                            return existingSet;
                        }
                    );
                }

                return base.OnConnectedAsync();
            }
            catch
            {
                // در صورت خطا، همچنان باید یک Task برگردونیم
                return base.OnConnectedAsync();
            }
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            try
            {
                var userId = GetUserIdFromToken();
                if (userId.HasValue)
                {
                    if (_userConnections.TryGetValue(userId.Value, out var connections))
                    {
                        connections.Remove(Context.ConnectionId);

                        if (connections.Count == 0)
                        {
                            _userConnections.TryRemove(userId.Value, out _);
                        }
                    }
                }
            }
            catch
            {
                // خطا رو نادیده بگیر
            }
            finally
            {
                await base.OnDisconnectedAsync(exception);
            }
        }

        public async Task SendNotification(string text, List<long> usersId)
        {
            try
            {
                foreach (var userId in usersId)
                    if (_userConnections.TryGetValue(userId, out var connectionIds))
                    {
                        foreach (var connectionId in connectionIds)
                        {
                            await Clients.Client(connectionId).SendAsync("ReceiveNotification", text);
                        }
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending notification: {ex.Message}");
            }
        }
    }
}