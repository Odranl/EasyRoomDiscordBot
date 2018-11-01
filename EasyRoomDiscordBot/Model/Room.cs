using System;
using System.Collections.Generic;
using System.Text;

namespace EasyRoomDiscordBot.Model
{
    class Room
    {
        public string Code { get; set; }
        
        public List<Lesson> Lessons { get; set; }

        public Room(string code)
        {
            Code = code;
            Lessons = new List<Lesson>();
        }

        public bool IsFree(DateTime time)
        {
            bool free = true;
            foreach (var lesson in Lessons)
            {
                if ((time >= lesson.StartTime) && (time <= lesson.EndTime))
                {
                    free = false;
                }
            }

            return free;
        }

        public string FreeUntil(DateTime time, out DateTime closestLessonTime)
        {
            DateTime _closestLessonTime = DateTime.MaxValue;
            foreach (var lesson in Lessons)
            {
                if (lesson.StartTime < _closestLessonTime && lesson.StartTime > time)
                {
                    _closestLessonTime = lesson.StartTime;
                }
            }

            closestLessonTime = _closestLessonTime;

            string returnString = "";

            if (_closestLessonTime == DateTime.MaxValue)
            {
                returnString = "Free all day";
            }
            else
            {
                returnString = $"Free until {_closestLessonTime.ToString("HH:mm")}";
            }

            return returnString;
        }
    }
}
