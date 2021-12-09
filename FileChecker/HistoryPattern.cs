using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileChecker
{
    class HistoryPattern
    {
        public HistoryPattern(string data)
        {
            _tournamentHH = data;
        }
        public int SmallBlind { get; set; }
        public int BigBlind { get; set; }

        public int Ante { get; set; }

        /// <summary>
        /// Количество игроков
        /// </summary>
        public int playerCount;
        
        private string _tournamentHH;

        public string TournamentHH
        {
            get { return _tournamentHH; }
        }

        public List<string> handHistoryList = new List<string>();
        public List<string> header = new List<string>();
        public List<Player> playersInGame = new List<Player>();


        //public List<string> GetHeader() 
        //{
        //    for (int i = 0; i < 4; i++)
        //    {
        //        header.Add(handHistoryList[i]);
        //    }
        //    return header;
        //}

        /// <summary>
        /// Последняя раздача.
        /// </summary>
        public void GetLastGameBlock()
        {
            List<int> gameBlockCount = new List<int>();
            string[] allLinesArray = TournamentHH.Split('\n');
            for (int i = 0; i < allLinesArray.Length - 1; i++)
            {                                                                                   
                if (allLinesArray[i].Contains("#Game No"))
                {
                    gameBlockCount.Add(i);
                }
            }

            int count = (allLinesArray.Length - 3) - gameBlockCount.Last();
            string[] lastGameBlockArray = new string[count];
            for (int i = gameBlockCount.Last(), k = 0; i < (allLinesArray.Length - 3); i++, k++)
            {
                handHistoryList.Add(allLinesArray[i]);
                lastGameBlockArray[k] = allLinesArray[i];
            }
        }

        /// <summary>
        /// Количество игроков.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public void GetNumberOfPlayers()
        {
            try
            {
                char count = handHistoryList[5][26];
                playerCount = Int32.Parse(count.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ ex.Message}");
            }
        }

        public void GetDealPlayersInfo(string line) 
        {

            char number = line[5];
            int seatNumber = Int32.Parse(number.ToString());
            line = line.Substring(8);
            Regex regexName = new Regex(@"(\S*)");
            Match matchName = regexName.Match(line);
            string name = matchName.Value;
            int chips = GetNumberInRoundBrackets(line);

            playersInGame.Add(new Player
            {
                SeatNumber = seatNumber,
                Name = name,
                StartChips = chips
            });
        }

        public void ScanWithName(Player player, string line)
        {

            if (line.Contains($"{player.Name} posts"))
            {
                player.Posts =+ GetNumberInSquareBrackets(line);
            }
            if (line.Contains($"{player.Name} raises"))
            {
                player.Raises =+ GetNumberInSquareBrackets(line);
            }
            if (line.Contains($"{player.Name} calls"))
            {
                player.Calls =+ GetNumberInSquareBrackets(line);
            }
            if (line.Contains($"{player.Name} collected"))
            {
                player.Collected =+ GetNumberInSquareBrackets(line);
            }
        }

        /// <summary>
        /// Число в квадратных скобках
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public int GetNumberInSquareBrackets(string line) 
        {
            int chips;
            string[] tempChips = line.Split(new[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries);
            if (tempChips[1].Contains(","))
            {
                string[] ch = tempChips[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                chips = Int32.Parse(ch[0] + ch[1]);
            }
            else
            {
                chips = Int32.Parse(tempChips[1]);
            }
            return chips;
        }

        /// <summary>
        /// Число в круглых скобках
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public int GetNumberInRoundBrackets(string line)
        {
            int chips;
            string[] tempChips = line.Split(new[] { '(', ')' }, StringSplitOptions.RemoveEmptyEntries);
            if (tempChips[1].Contains(","))
            {
                string[] ch = tempChips[1].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                chips = Int32.Parse(ch[0] + ch[1]);
            }
            else
            {
                chips = Int32.Parse(tempChips[1]);
            }
            return chips;
        }








    }
}
