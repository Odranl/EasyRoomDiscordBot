using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRoomDiscordBot.Model
{
    class Lesson
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string RoomCode { get; set; }

        public Lesson(string startTime, string endTime, string roomCode)
        {
            StartTime = DateTime.Parse(startTime);
            EndTime = DateTime.Parse(endTime);
            RoomCode = roomCode;
        }
        
    }
}
