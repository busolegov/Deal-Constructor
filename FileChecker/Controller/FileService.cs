using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using FileChecker.Models;

namespace FileChecker.Controller
{
    class FileService
    {
        public FileService(string checkPath, string newPath)
        {
            currentFolderPath = checkPath;
            newFolderPath = newPath;
        }

        public readonly string currentFolderPath;
        public readonly string newFolderPath;
        
        public string [] filesArray;
        
        public List<FileData> processedFileList = new List<FileData>();
        public List<FileData> tempFileList = new List<FileData>();
        public List<FileData> lastGameBlock = new List<FileData>();



        /// <summary>
        /// Метод получения списка файлов в папке
        /// </summary>
        /// <param name="checkPath"></param>
        /// <returns></returns>
        public string [] GetFilesArray(string checkPath) 
        {
            filesArray = Directory.GetFiles(checkPath);
            return filesArray;
        }

        /// <summary>
        /// Обрезка имени файла.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string NameCutter(string path) 
        {
            path = path.Substring(path.Length-92);
            return path;
        }

        /// <summary>
        /// Метод определния времени последнего изменения файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DateTime GetFileDate(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            return fileInfo.LastWriteTime;
        }

        /// <summary>
        /// Метод определения размера файла
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public byte GetFileSize(string path)
        {
            FileInfo fileInfo = new FileInfo(path);
            return (byte)fileInfo.Length;
        }

        public List<FileData> GetFilesList(string[] array) 
        {
            for (int i = 0; i < array.Length; i++)
            {
                processedFileList.Add(new FileData
                { 
                    PathName = array[i],
                    Name = NameCutter(array[i])
                });
            }
            return processedFileList;
        }


        /// <summary>
        /// Метод сравнения размеров файлов - bool
        /// </summary>
        /// <param name="path"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public bool CompareFileDate(DateTime tempDate, DateTime date) 
        {
            if (tempDate > date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GetStructure(HistoryPattern pattern) 
        {
            pattern.GetLastGameBlock();
            pattern.GetNumberOfPlayers();
            pattern.PlayersActionSum();
            for (int i = 6; i < 6 + pattern.playerCount; i++)
            {
                pattern.GetDealPlayersInfo(pattern.handHistoryList[i]);
            }

            for (int i = 7 + pattern.playerCount; i < pattern.handHistoryList.Count; i++)
            {
                foreach (var player in pattern.playersInGame)
                {
                    pattern.ScanWithName(player, pattern.handHistoryList[i]);
                }
            }

            pattern.GetAnte();
            pattern.GetSmallBlindPosition();
            pattern.GetBigBlindPosition();
            pattern.GetButtonPostion();
            pattern.SetBlindsAndAnteToPlayer();
            pattern.PlayersActionSum();
        }


        /// <summary>
        /// Асинхронный метод чтения и записи файла.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="buffer"></param>
        public async void ReadWriteFileDataAsync (string currentPath, string newPath) 
        {
            try
            {
                using (StreamReader fileDataReader = new StreamReader(currentPath, Encoding.UTF8))
                {
                    string data = fileDataReader.ReadToEnd();
                    
                    HistoryPattern tournamentHistory = new HistoryPattern(data);

                    GetStructure(tournamentHistory);

                    NewDealConstructor newDeal = new NewDealConstructor(tournamentHistory);
                    newDeal.PlayersConstructor();

                    using (StreamWriter fileDataWriter = new StreamWriter(newPath, false, Encoding.UTF8))
                    {
                        await fileDataWriter.WriteAsync(newDeal.NewDealTextConstructor());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Возникла ошибка при чтении или записи файла: {ex.Message}");
            }
        }
    }
}
