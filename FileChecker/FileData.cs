using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileChecker
{
    class FileData
    {
        public string PathName { get; set; }
        public string Name { get; set; }
        public DateTime ChangedDate { get; set; }

        public byte Size { get; set; }

        //public string Data { get; set; }

        //public bool isChanged = false;

    }
}
