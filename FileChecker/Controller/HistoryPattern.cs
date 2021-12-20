using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileChecker.Models
{
    internal class HistoryPattern
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

        public int ActionSum { get; set; }

        private string _tournamentHH;

        public string TournamentHH
        {
            get { return _tournamentHH; }
        }

        public List<string> handHistoryList = new List<string>();
        public List<string> header = new List<string>();
        public List<Player> playersInGame = new List<Player>();


        /// <summary>
        /// Последняя раздача из файла.
        /// </summary>
        public void GetLastGameBlock()
        {
            List<int> gameBlockIndex = new List<int>();
            string[] allLinesArray = TournamentHH.Split('\n');
            for (int i = 0; i < allLinesArray.Length - 1; i++)
            {                                                                                   
                if (allLinesArray[i].Contains("#Game No"))
                {
                    gameBlockIndex.Add(i);
                }
            }

            int count = (allLinesArray.Length - 3) - gameBlockIndex.Last();
            string[] lastGameBlockArray = new string[count];
            for (int i = gameBlockIndex.Last(), k = 0; i < (allLinesArray.Length - 3); i++, k++)
            {
                handHistoryList.Add(allLinesArray[i]);
                lastGameBlockArray[k] = allLinesArray[i];
            }
        }

        public void GetAnte() 
        {
            foreach (var line in handHistoryList)
            {
                if (line.Contains($"ante"))
                {
                    Ante = GetNumberInSquareBrackets(line);
                    return;
                }
            }
        }

        public void GetSmallBlindPosition()
        {
            foreach (var line in handHistoryList)
            {
                if (line.Contains($"small blind"))
                {
                    SmallBlind = GetNumberInSquareBrackets(line);
                    Regex regexName = new Regex(@"(\S*)");
                    Match matchName = regexName.Match(line);
                    string name = matchName.Value;
                    foreach (var player in playersInGame)
                    {
                        if (name == player.Name)
                        {
                            player.BoolSmallBlind = true;
                            return;
                        }
                    }
                }
            }
        }

        public void GetBigBlindPosition()
        {
            foreach (var line in handHistoryList)
            {
                if (line.Contains($"big blind"))
                {
                    BigBlind = GetNumberInSquareBrackets(line);
                    Regex regexName = new Regex(@"(\S*)");
                    Match matchName = regexName.Match(line);
                    string name = matchName.Value;
                    foreach (var player in playersInGame)
                    {
                        if (name == player.Name)
                        {
                            player.BoolBigBlind = true;
                            return;
                        }
                    }
                }
            }
        }
        
        public void GetButtonPostion()
        {
            char seat = handHistoryList[4][5];
            int buttonSeat = Int32.Parse(seat.ToString());
            foreach (var player in playersInGame)
            {
                if (buttonSeat == player.SeatNumber)
                {
                    player.Button = true;
                    return;
                }
            }
        }

        public void SetBlindsAndAnteToPlayer()
        {
            foreach (var player in playersInGame)
            {
                player.IntAnte = Ante;

                if (player.BoolBigBlind)
                {
                    player.IntBigBlind = BigBlind;
                }
                if (player.BoolSmallBlind)
                {
                    player.IntSmallBlind = SmallBlind;
                }
            }
        }


        /// <summary>
        /// Количество игроков.
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public void GetNumberOfPlayers()
        {
            char count = handHistoryList[5][26];
            playerCount = Int32.Parse(count.ToString());
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
            GetButtonPostion();
            SetBlindsAndAnteToPlayer();
        }

        /// <summary>
        /// Выборка ников.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="line"></param>
        public void ScanWithName(Player player, string line)
        {

            if (line.Contains($"{player.Name} posts ["))
            {
                player.Posts += GetNumberInSquareBrackets(line);
            }
            if (line.Contains($"{player.Name} raises"))
            {
                player.Raises += GetNumberInSquareBrackets(line);
            }
            if (line.Contains($"{player.Name} calls"))
            {
                player.Calls += GetNumberInSquareBrackets(line);
            }
            if (line.Contains($"{player.Name} collected"))
            {
                player.Collected += GetNumberInSquareBrackets(line);
            }
        }

        public void PlayersActionSum() 
        {
            foreach (var player in playersInGame)
            {
                ActionSum += player.Posts + player.Raises + player.Calls;
            }
            ActionSum += SmallBlind + BigBlind + Ante * playerCount;
        }


        /// <summary>
        /// Парсинг числа в квадратных скобках
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
        /// Парсинг числа в круглых скобках
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
