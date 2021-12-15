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
        public bool SmallBlind { get; set; }
        public bool BigBlind { get; set; }
        public int Posts { get; set; }
        public int Raises { get; set; }
        public int Calls { get; set; }
        public int Collected { get; set; }
    }
}
