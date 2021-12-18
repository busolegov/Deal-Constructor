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
        }

        string buttonSeat;
        string smallBlindName;
        string bigBlindName;

        public List<Player> HistoryDealPlayerList { get; set; }
        public List<Player> NewDealPlayerList { get; set; }

        public List<string> newDeal = new List<string>();
        public void StackConstructor(HistoryPattern pattern) 
        {
            foreach (var player in pattern.playersInGame)
            {
                NewDealPlayerList.Add(new Player
                {
                    StartChips = NewStackConstructor(),
                    SeatNumber = player.SeatNumber,
                    Name = player.Name
                });
            }
            MoveButton(pattern);
            MoveSmallBlind(pattern);
            MoveBigBlind(pattern);
        }



        public int NewStackConstructor(HistoryPattern pattern, Player player) 
        {
            int newStack;
            if (player.Collected > 0)
            {
                newStack = player.StartChips + pattern.ActionSum - player.Raises - player.Posts - player.Calls - player.IntBigBlind - player.IntSmallBlind - pattern.Ante;
                return newStack;
            }
            else
            {
                newStack = player.StartChips;
                return newStack;
            }
        }

        public void MoveButton(HistoryPattern pattern) 
        {
            foreach (var player in pattern.playersInGame)
            {
                if (player.Button == true)
                {
                    int position = pattern.playersInGame.IndexOf(player);
                    if (position >= NewDealPlayerList.Count-1)
                    {
                        NewDealPlayerList[0].Button = true;
                        break;
                    }
                    else
                    {
                        NewDealPlayerList[position++].Button = true;
                        break;
                    }
                }
            }
        }

        public void MoveSmallBlind(HistoryPattern pattern)
        {
            foreach (var player in pattern.playersInGame)
            {
                if (player.BoolSmallBlind == true)
                {
                    int position = pattern.playersInGame.IndexOf(player);
                    player.BoolSmallBlind = false;
                    if (position >= NewDealPlayerList.Count-1)
                    {
                        NewDealPlayerList[0].BoolSmallBlind = true;
                        break;
                    }
                    else
                    {
                        NewDealPlayerList[position++].BoolSmallBlind = true;
                        break;
                    }
                }
            }
        }

        public void MoveBigBlind(HistoryPattern pattern)
        {
            foreach (var player in pattern.playersInGame)
            {
                if (player.BoolBigBlind == true)
                {
                    int position = pattern.playersInGame.IndexOf(player);
                    player.BoolBigBlind = false;
                    if (position >= NewDealPlayerList.Count-1)
                    {
                        NewDealPlayerList[0].BoolBigBlind = true;
                        break;
                    }
                    else
                    {
                        NewDealPlayerList[position++].BoolBigBlind = true;
                        break;
                    }
                }
            }
        }

        public string NewDealConstructor(HistoryPattern pattern)
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
            newData += pattern.handHistoryList[0];
            newData += pattern.handHistoryList[1];
            newData += pattern.handHistoryList[2];
            newData += pattern.handHistoryList[3];
            newData += String.Format("Seat {0} is the button\r", buttonSeat);
            newData += pattern.handHistoryList[5];
            foreach (var player in NewDealPlayerList)
            {
                newData += String.Format("Seat {0}: {1} ({2})\r", player.SeatNumber.ToString(), player.Name.ToString(), player.StartChips.ToString());
            }
            foreach (var player in NewDealPlayerList)
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
