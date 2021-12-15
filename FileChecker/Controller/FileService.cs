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
                    ChangedDate = GetFileDate(array[i]),
                    Size = GetFileSize(array[i]),
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

        /// <summary>
        /// Асинхронный метод чтения и записи файла.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="buffer"></param>

        public async void ReadFileDataAsync (string currentPath, string newPath) 
        {
            try
            {
                using (StreamReader fileDataReader = new StreamReader(currentPath, Encoding.UTF8))
                {
                    string data = fileDataReader.ReadToEnd();
                    
                    HistoryPattern tournamentHistory = new HistoryPattern(data);
                    tournamentHistory.GetLastGameBlock();
                    tournamentHistory.GetNumberOfPlayers();

                    for (int i = 6; i < 6 + tournamentHistory.playerCount; i++)
                    {
                        tournamentHistory.GetDealPlayersInfo(tournamentHistory.handHistoryList[i]);
                    }

                    for (int i = 7 + tournamentHistory.playerCount; i < tournamentHistory.handHistoryList.Count; i++)
                    {
                        foreach (var player in tournamentHistory.playersInGame)
                        {
                            tournamentHistory.ScanWithName(player, tournamentHistory.handHistoryList[i]);
                        }
                    }

                    tournamentHistory.GetAnte();
                    tournamentHistory.GetSmallBlind();
                    tournamentHistory.GetBigBlind();

                    NewDealConstructor newDeal = new NewDealConstructor();
                    newDeal.StackConstructor(tournamentHistory);

                    //foreach (var player in tournamentHistory.playersInGame)
                    //{
                    //    Console.WriteLine(player.Name);
                    //    Console.WriteLine(player.Button);
                    //    Console.WriteLine(player.Raises);
                    //    Console.WriteLine(player.Posts);
                    //    Console.WriteLine(player.Collected);
                    //    Console.WriteLine(player.Calls);
                    //}
                    //data = string.Join("", GetLastGameBlock(data).ToArray());
                    using (StreamWriter fileDataWriter = new StreamWriter(newPath, false, Encoding.UTF8))
                    {
                        await fileDataWriter.WriteAsync(newDeal.NewDeal(tournamentHistory));
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
