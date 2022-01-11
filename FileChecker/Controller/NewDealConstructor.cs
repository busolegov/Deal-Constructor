using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChecker.Models
{
    internal class NewDealConstructor
    {
        public NewDealConstructor(HistoryPattern pattern)
        {
            HistoryDealPlayerList = pattern.playersInGame;
            HistoryDeal = pattern;
            PlayerCount = pattern.playerCount;
        }

        public HistoryPattern HistoryDeal { get; set; }

        public int PlayerCount { get; set; }
        string buttonSeat;
        string smallBlindName;
        string bigBlindName;

        public List<Player> HistoryDealPlayerList { get; set; }
        public List<Player> NewDealPlayerList { get; set; }
        public List <string> StringNewDeal { get; set; }

        /// <summary>
        /// Формирование текущей раздачи.
        /// </summary>
        public void GetPlayersData() 
        {
            NewDealPlayerList = new List<Player>();
            foreach (var player in HistoryDealPlayerList)
            {
                NewDealPlayerList.Add(new Player
                {
                    StartChips = GetNewStack(player),
                    SeatNumber = player.SeatNumber,
                    Name = player.Name,
                    Button = player.Button,
                    BoolSmallBlind = player.BoolSmallBlind,
                    BoolBigBlind = player.BoolBigBlind
                });
            }

            for (int i = 0; i < NewDealPlayerList.Count; i++)
            {
                if (NewDealPlayerList[i].StartChips == 0)
                {
                    NewDealPlayerList.RemoveAt(i);
                    PlayerCount--;
                }
            }
            MoveButton();
            MoveSmallBlind();
            MoveBigBlind();
        }

        /// <summary>
        /// Количество фишек игрока в текущей раздаче.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public int GetNewStack(Player player) 
        {
            if (player.Collected > 0)
            {
                int newStack = player.StartChips + HistoryDeal.ActionSum - 
                           player.Raises - player.Posts - player.Calls -
                           player.IntBigBlind - player.IntSmallBlind - player.IntAnte;
                return newStack;
            }
            else
            {
                int newStack = player.StartChips - player.Raises - player.Posts -
                           player.Calls - player.IntBigBlind - player.IntSmallBlind - player.IntAnte;
                return newStack;
            }
        }

        /// <summary>
        /// Перемещение позиции баттона.
        /// </summary>
        public void MoveButton() 
        {
            foreach (var player in NewDealPlayerList)
            {
                if (player.Button == true)
                {
                    int position = NewDealPlayerList.IndexOf(player);
                    if (position >= NewDealPlayerList.Count-1)
                    {
                        player.Button = false;
                        NewDealPlayerList[0].Button = true;
                        break;
                    }
                    else
                    {
                        player.Button = false;
                        position++;
                        NewDealPlayerList[position].Button = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Перемещение позиции маленького блайнда.
        /// </summary>
        public void MoveSmallBlind()
        {
            foreach (var player in NewDealPlayerList)
            {
                if (player.BoolSmallBlind == true)
                {
                    int position = NewDealPlayerList.IndexOf(player);
                    if (position >= NewDealPlayerList.Count-1)
                    {
                        player.BoolSmallBlind = false;
                        NewDealPlayerList[0].BoolSmallBlind = true;
                        break;
                    }
                    else
                    {
                        player.BoolSmallBlind = false;
                        position++;
                        NewDealPlayerList[position].BoolSmallBlind = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Перемещение позиции большого блайнда.
        /// </summary>
        public void MoveBigBlind()
        {
            foreach (var player in NewDealPlayerList)
            {
                if (player.BoolBigBlind == true)
                {
                    int position = NewDealPlayerList.IndexOf(player);
                    if (position >= NewDealPlayerList.Count-1)
                    {
                        player.BoolBigBlind = false;
                        NewDealPlayerList[0].BoolBigBlind = true;
                        break;
                    }
                    else
                    {
                        player.BoolBigBlind = false;
                        position++;
                        NewDealPlayerList[position].BoolBigBlind = true;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Конструктор теста текущей раздачи.
        /// </summary>
        /// <returns></returns>
        public string GetNewDealText()
        {
            foreach (var player in NewDealPlayerList)
            {
                if (player.Button == true)                
                {
                    buttonSeat = player.SeatNumber.ToString();
                }
                if (player.BoolSmallBlind == true)
                {
                    smallBlindName = player.Name;
                }
                if (player.BoolBigBlind == true)
                {
                    bigBlindName = player.Name;
                }
            }
            string newData = "";
            newData += HistoryDeal.handHistoryList[0];
            newData += HistoryDeal.handHistoryList[1];
            newData += HistoryDeal.handHistoryList[2];
            newData += HistoryDeal.handHistoryList[3];
            newData += String.Format("Seat {0} is the button\r", buttonSeat);
            newData += String.Format("Total number of players : {0}\r", PlayerCount.ToString());
            foreach (var player in NewDealPlayerList)
            {
                newData += String.Format("Seat {0}: {1} ({2})\r", player.SeatNumber.ToString(), player.Name.ToString(), player.StartChips.ToString());
            }
            foreach (var player in NewDealPlayerList)
            {
                newData += String.Format("{0} posts ante [{1}]\r", player.Name, HistoryDeal.Ante.ToString());
            }
            newData += String.Format("{0} posts small blind [{1}]\r", smallBlindName, HistoryDeal.SmallBlind.ToString());
            newData += String.Format("{0} posts big blind [{1}]\r", bigBlindName, HistoryDeal.BigBlind.ToString());
            newData += "** Dealing down cards **\r";
            return newData;
        }


    }
}
