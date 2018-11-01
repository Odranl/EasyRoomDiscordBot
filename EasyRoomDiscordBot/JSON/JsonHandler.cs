using EasyRoomDiscordBot.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyRoomDiscordBot.JSON
{
    public class JsonHandler
    {
        Dictionary<string, Room> Rooms = new Dictionary<string, Room>();

        public JsonHandler(DateTime date)
        {
            JObject array = JObject.Parse(GetHTTPPOST(date));

            IEnumerable<JToken> roomsJson = from p in array["area_rooms"]["E0503"] select p;

            string regex = "[A-B]{1}[0-9]{3}"; //E.g. A103, B107...

            foreach (dynamic p in roomsJson)
            {
                string roomName = p.Name.ToString();
                if (Regex.IsMatch(roomName.Split('/')[1], regex))
                {
                    Room room = new Room(roomName);
                    Rooms.TryAdd(roomName, room);
                }
            }

            var lessons = from p in array["events"] select p;

            foreach (JToken lesson in lessons)
            {
                string code = lesson["CodiceAula"].ToString();
                if (Rooms.TryGetValue(code, out var room))
                    room.Lessons.Add(new Lesson(
                    date.ToString("dd/MM/yyyy") + " " + lesson["from"].ToString(),
                    date.ToString("dd/MM/yyyy") + " " + lesson["to"].ToString(),
                    code));
            }
        }

        public string GetTable(DateTime date)
        {
            string table = "";
            foreach (var room in Rooms)
            {
                if (room.Value.IsFree(date))
                {
                    table += $":white_check_mark: ``{room.Value.Code.Split('/')[1]}:  {room.Value.FreeUntil(date, out var closestLesson)}``" + "\n";
                }
                else
                {
                    table += ":x: ``" + room.Value.Code.Split('/')[1] + ":  Busy``" + "\n";
                }
            }
            return table;
        }

        public string GetHTTPPOST()
        {
            HttpClient client = new HttpClient();

            var values = new Dictionary<string, string>
            {
               { "form-type", "rooms" },
               { "sede", "E0503" },
               { "date", DateTime.Now.ToString("dd-MM-yyyy")},
               { "_lang", "world" }
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("https://easyroom.unitn.it/Orario/rooms_call.php", content);

            return response.Result.Content.ReadAsStringAsync().Result;
        }
        public string GetHTTPPOST(DateTime date)
        {
            HttpClient client = new HttpClient();

            var values = new Dictionary<string, string>
            {
               { "form-type", "rooms" },
               { "sede", "E0503" },
               { "date", date.ToString("dd-MM-yyyy")},
               { "_lang", "world" }
            };

            var content = new FormUrlEncodedContent(values);

            return (client.PostAsync("https://easyroom.unitn.it/Orario/rooms_call.php", content)).Result.Content.ReadAsStringAsync().Result;
        }
    }
}
