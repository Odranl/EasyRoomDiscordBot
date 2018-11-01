using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EasyRoomDiscordBot
{
    public class BotCommands
    {
        [Command("rooms")]
        public async Task RoomsAvailability(CommandContext commandContext, string dateString = "", string time = "")
        {
            DateTime date;

            if(string.IsNullOrWhiteSpace(dateString))
            {
                date = DateTime.Now;
            }
            else
            {
                date = DateTime.Parse(dateString+" "+time);
            }

            JSON.JsonHandler jsonHandler = new JSON.JsonHandler(date);

            await commandContext.TriggerTypingAsync();

            await commandContext.RespondAsync(jsonHandler.GetTable(date));
        }
    }
}
