using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FileChecker.Models;
using FileChecker.Controller;

namespace FileChecker
{
    class Program
    {
        static void Main(string[] args)
        {
            const string CURRENTPATH = @"C:\Users\6yc\Desktop\currentfolder";
            const string NEWPATH = @"C:\Users\6yc\Desktop\newfolder";

            try
            {
                while (true)
                {
                    Console.WriteLine("Запущено сканирование: " + DateTime.Now);

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
                            fileService.ReadFileDataAsync(file.PathName, $"{NEWPATH}/new_{file.Name}");

                            Console.WriteLine($"Просканирован файл {file.PathName}");
                        }
                    }
                    Console.WriteLine("Пауза 5 сек перед следующим сканированием...");
                    Console.WriteLine();
                    Thread.Sleep(5000);
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Возникла ошибка: { ex.Message}" );
            }
        }
    }
}
