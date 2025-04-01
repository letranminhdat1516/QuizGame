using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace QuizGame.Player
{
    public class GameHub : Hub
    {
        // Khi player tham gia, gửi thông báo cho tất cả các client
        public async Task PlayerJoined(string playerName)
        {
            await Clients.All.SendAsync("PlayerJoined", playerName); // Gửi cho tất cả người chơi
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
