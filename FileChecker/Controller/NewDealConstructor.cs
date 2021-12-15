using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChecker.Models
{
    internal class NewDealConstructor
    {
        string buttonSeat;
        string smallBlindName;
        string bigBlindName;

        public List<Player> playerList = new List<Player>();
        public List<string> newDeal = new List<string>();
        public void StackConstructor(HistoryPattern pattern) 
        {
            foreach (var player in pattern.playersInGame)
            {
                int newStack = player.StartChips - player.Posts;
                playerList.Add(new Player
                {
                    StartChips = newStack,
                    SeatNumber = player.SeatNumber,
                    Name = player.Name
                });
            }
            MoveButton(pattern);
            MoveSmallBlind(pattern);
            MoveBigBlind(pattern);
        }

        public void MoveButton(HistoryPattern pattern) 
        {
            foreach (var player in pattern.playersInGame)
            {
                if (player.Button == true)
                {
                    int position = pattern.playersInGame.IndexOf(player);
                    if (position >= playerList.Count-1)
                    {
                        playerList[0].Button = true;
                        break;
                    }
                    else
                    {
                        playerList[position++].Button = true;
                        break;
                    }
                }
            }
        }

        public void MoveSmallBlind(HistoryPattern pattern)
        {
            foreach (var player in pattern.playersInGame)
            {
                if (player.SmallBlind == true)
                {
                    int position = pattern.playersInGame.IndexOf(player);
                    player.SmallBlind = false;
                    if (position >= playerList.Count-1)
                    {
                        playerList[0].SmallBlind = true;
                        break;
                    }
                    else
                    {
                        playerList[position++].SmallBlind = true;
                        break;
                    }
                }
            }
        }

        public void MoveBigBlind(HistoryPattern pattern)
        {
            foreach (var player in pattern.playersInGame)
            {
                if (player.BigBlind == true)
                {
                    int position = pattern.playersInGame.IndexOf(player);
                    player.BigBlind = false;
                    if (position >= playerList.Count-1)
                    {
                        playerList[0].BigBlind = true;
                        break;
                    }
                    else
                    {
                        playerList[position++].BigBlind = true;
                        break;
                    }
                }
            }
        }

        public string NewDeal(HistoryPattern pattern)
        {
            foreach (var player in playerList)
            {
                if (player.Button == true)                
                {
                    buttonSeat = player.SeatNumber.ToString();
                }
                if (player.SmallBlind == true)
                {
                    smallBlindName = player.Name;
                }
                if (player.BigBlind == true)
                {
                    bigBlindName = player.Name;
                }
            }

            string newData = "";
            newData += pattern.handHistoryList[0];
            newData += pattern.handHistoryList[1];
            newData += pattern.handHistoryList[2];
            newData += pattern.handHistoryList[3];
            newData += String.Format("Seat {0} is the button\r", buttonSeat);
            newData += pattern.handHistoryList[5];
            foreach (var player in playerList)
            {
                newData += String.Format("Seat {0}: {1} ({2})\r", player.SeatNumber.ToString(), player.Name.ToString(), player.StartChips.ToString());
            }
            foreach (var player in playerList)
            {
                newData += String.Format("{0} posts ante [{1}]\r", player.Name, pattern.Ante.ToString());
            }
            newData += String.Format("{0} posts small blind [75]\r", smallBlindName);
            newData += String.Format("{0} posts big blind [150]\r", bigBlindName);
            newData += "** Dealing down cards **\r";
            newData += "Dealt to busolegov [ Ah, As ]";
            return newData;
        }


    }
}
