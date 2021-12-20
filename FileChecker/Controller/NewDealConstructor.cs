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
        }

        public HistoryPattern HistoryDeal { get; set; }
        string buttonSeat;
        string smallBlindName;
        string bigBlindName;

        public List<Player> HistoryDealPlayerList { get; set; }
        public List<Player> NewDealPlayerList { get; set; }
        public List <string> StringNewDeal { get; set; }
        public void PlayersConstructor() 
        {
            NewDealPlayerList = new List<Player>();
            foreach (var player in HistoryDealPlayerList)
            {
                NewDealPlayerList.Add(new Player
                {
                    StartChips = NewStackConstructor(player),
                    SeatNumber = player.SeatNumber,
                    Name = player.Name
                });
            }
            MoveButton();
            MoveSmallBlind();
            MoveBigBlind();
        }


        public int NewStackConstructor(Player player) 
        {
            int newStack;
            if (player.Collected > 0)
            {
                newStack = player.StartChips + HistoryDeal.ActionSum - 
                           player.Raises - player.Posts - player.Calls -
                           player.IntBigBlind - player.IntSmallBlind - player.IntAnte;
                return newStack;
            }
            else
            {
                newStack = player.StartChips - player.Raises - player.Posts -
                           player.Calls - player.IntBigBlind - player.IntSmallBlind - player.IntAnte;
                return newStack;
            } 
        }

        public void MoveButton() 
        {
            foreach (var player in HistoryDealPlayerList)
            {
                if (player.Button == true)
                {
                    int position = HistoryDealPlayerList.IndexOf(player);
                    if (position >= NewDealPlayerList.Count-1)
                    {
                        NewDealPlayerList[0].Button = true;
                        break;
                    }
                    else
                    {
                        position++;
                        NewDealPlayerList[position].Button = true;
                        break;
                    }
                }
            }
        }

        public void MoveSmallBlind()
        {
            foreach (var player in HistoryDealPlayerList)
            {
                if (player.BoolSmallBlind == true)
                {
                    int position = HistoryDealPlayerList.IndexOf(player);
                    player.BoolSmallBlind = false;
                    if (position >= NewDealPlayerList.Count-1)
                    {
                        NewDealPlayerList[0].BoolSmallBlind = true;
                        break;
                    }
                    else
                    {
                        position++;
                        NewDealPlayerList[position].BoolSmallBlind = true;
                        break;
                    }
                }
            }
        }

        public void MoveBigBlind()
        {
            foreach (var player in HistoryDealPlayerList)
            {
                if (player.BoolBigBlind == true)
                {
                    int position = HistoryDealPlayerList.IndexOf(player);
                    player.BoolBigBlind = false;
                    if (position >= NewDealPlayerList.Count-1)
                    {
                        NewDealPlayerList[0].BoolBigBlind = true;
                        break;
                    }
                    else
                    {
                        position++;
                        NewDealPlayerList[position].BoolBigBlind = true;
                        break;
                    }
                }
            }
        }

        public string NewDealTextConstructor()
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
            newData += HistoryDeal.handHistoryList[5];
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
            newData += "Dealt to busolegov [ Ah, As ]";
            return newData;
        }


    }
}
