using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChecker.Models
{
    internal class PlayersList : IEnumerable
    {
        public PlayersList(List<Player>list)
        {
            playerList = list;
        }
        public List<Player> playerList = new List<Player>();
        public IEnumerator GetEnumerator()
        {
            return playerList.GetEnumerator();
        }
    }

    internal class PlayerEnumerator : IEnumerator
    {
        List<Player> playerList;
        int position = -1;
        public PlayerEnumerator(List<Player> list)
        {
            playerList = list;
        }
        public object Current
        {
            get
            {
                if (position == -1 || position >= playerList.Count)
                {
                    throw new InvalidOperationException();
                }
                return playerList[position];
            }
        }
        object IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext() 
        {
            if (position < playerList.Count - 1)
            {
                position++;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void Reset() 
        {
            position = -1;
        }
        public void Dispose() { }

        public void MoveButton()
        {
            foreach (var player in playerList)
            {
                if (player.Button == true)
                {
                    player.Button = false;
                    MoveNext();
                    player.Button = true;
                }
            }
        }
    }
}
