using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChecker.Models
{
    class Player
    {
        public string Name { get; set; }
        public int SeatNumber { get; set; }
        public int StartChips { get; set; }
        public bool Button { get; set; }
        public bool BoolSmallBlind { get; set; }
        public int IntSmallBlind { get; set; }
        public bool BoolBigBlind { get; set; }
        public int IntBigBlind { get; set; }
        public int IntAnte { get; set; }
        public int Posts { get; set; }
        public int Raises { get; set; }
        public int Calls { get; set; }
        public int Collected { get; set; }
    }
}
