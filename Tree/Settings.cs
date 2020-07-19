using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tree.Models;

namespace Tree
{
    public class Settings
    {

        public int Depth { get; set; } = -1;

        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;

        public OrderBy OrderBy { get; set; } = OrderBy.Name;

        public bool ShowSize { get; set; }

        public bool HumanReadable { get; set; }

        public bool ShowHelp { get; set; }

        public string RootFolder { get; set; } = Environment.CurrentDirectory;


        public void Init(string[] args)
        {
            if (args.Length > 0)
            {
                int startIndex = 0;
                // первым параметром ожидаем путь к папке
                if (Directory.Exists(args[0]))
                {
                    RootFolder = args[0];
                    startIndex = 1;
                }

                foreach (string arg in args.Skip(startIndex))
                {
                    string[] splitData = arg.Split('=');

                    switch (splitData[0])
                    {
                        case "-h":
                        case "--human-readable":
                            HumanReadable = true;
                            break;
                        case "-s":
                        case "--size":
                            ShowSize = true;
                            break;
                        case "-d":
                        case "--depth":
                            try
                            {
                                Depth = int.Parse(splitData[1]);
                            }
                            catch
                            {
                                throw new ArgumentException("Неверное значение параметра");
                            }
                            break;
                        case "-?":
                        case "--help":
                            ShowHelp = true;
                            break;
                        case "-do":
                            SortOrder = SortOrder.Descending;
                            break;
                        case "-oc":
                            OrderBy = OrderBy.CreateDate;
                            break;
                        case "-os":
                            OrderBy = OrderBy.Size;
                            break;
                        case "-om":
                            OrderBy = OrderBy.ModifyDate;
                            break;
                        default:
                            throw new ArgumentException($"Неправильный параметр {splitData[0]}");
                    }
                }
            }
        }       
       
    }
}
