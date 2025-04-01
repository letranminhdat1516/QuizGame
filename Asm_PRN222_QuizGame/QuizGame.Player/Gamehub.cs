using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace QuizGame.Player
{
    public class GameHub : Hub
    {
        // Khi player tham gia, gửi thông báo cho tất cả các client
        //public async Task PlayerJoined(string gamePin, string playerName)
        //{
        //    await Clients.All.SendAsync("PlayerJoined", playerName); // Gửi thông báo đến tất cả client
        //}
        public async Task JoinGameGroup(string gamePin)
        {
            // Logic to add player to the group using gamePin
            await Groups.AddToGroupAsync(Context.ConnectionId, gamePin);
            // Optionally, notify other clients that a player has joined
            await Clients.Group(gamePin).SendAsync("PlayerJoined", Context.User.Identity.Name);
        }

        // Gửi thông báo khi một player tham gia game
        public async Task NotifyPlayerJoined(int gameId, string playerName)
        {
            // Gửi thông báo cho tất cả người chơi trong nhóm game
            await Clients.Group(gameId.ToString()).SendAsync("PlayerJoined", playerName);
        }
        // Khi game bắt đầu, gửi thông báo tới tất cả người chơi
        public async Task GameStarted(int gameId)
        {
            await Clients.All.SendAsync("GameStarted", gameId); // Gửi cho tất cả người chơi
        }

        // Gửi câu hỏi đến tất cả người chơi
        public async Task SendQuestionToPlayers(int gameId, string questionText)
        {
            await Clients.All.SendAsync("ReceiveQuestion", gameId, questionText);
        }
    }
}
