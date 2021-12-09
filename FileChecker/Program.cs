using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileChecker
{
    class Program
    {
        public static void ScanFolder(object obj)
        {
            Console.WriteLine("Запущено сканирование: " + DateTime.Now);
        }

        static void Main(string[] args)
        {
            const string CURRENTPATH = @"C:\Users\6yc\Desktop\currentfolder";
            const string NEWPATH = @"C:\Users\6yc\Desktop\newfolder";

            try
            {
                #region testregion
                //FileService fileSerive = new FileService(CURRENTPATH, NEWPATH);
                //string[] fileList = fileSerive.GetFilesArray(CURRENTPATH);

                //List<FileData> fileScanned = fileSerive.GetFilesList(fileList);
                //foreach (var item in fileSerive.fileList)
                //{
                //    Console.WriteLine(item.PathName);
                //    Console.WriteLine(item.ChangedDate);
                //    Console.WriteLine(item.Size);
                //    Console.WriteLine("****************************");
                //}

                //TimerCallback timerCall = new TimerCallback(ScanFolder);
                //Timer timer = new Timer(timerCall, null, 0, 5000);
                #endregion
                while (true)
                {
                    FileService fileService = new FileService(CURRENTPATH, NEWPATH);

                    string[] firstTempArray = fileService.GetFilesArray(CURRENTPATH);
                    
                    fileService.processedFileList = fileService.GetFilesList(firstTempArray);
                    
                    if (fileService.processedFileList.Count == 0)
                    {
                        Console.WriteLine("В папке нет файлов. 5 секунд ожидания перед следующей проверкой...");
                        Thread.Sleep(5000);
                    }
                    else
                    {
                        foreach (var file in fileService.processedFileList)
                        {
                            fileService.ReadFileDataAsync(file.PathName, $"{NEWPATH}/temp_{file.Name}");

                            Console.WriteLine($"Просканирован файл {file.PathName}");
                        }
                    }
                    Console.WriteLine("Пауза 5 сек перед следующим сканированием...");
                    Console.WriteLine();
                    Thread.Sleep(5000);
                }
                Console.ReadLine();

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Возникла ошибка: { ex.Message}" );
            }
        }
    }
}
