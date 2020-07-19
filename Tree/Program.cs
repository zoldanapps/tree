﻿using System;
using Tree.Models;

namespace Tree
{
    class Program
    {      

        static void Main(string[] args)
        {            
            Settings settings = new Settings();

            try
            {
                // парсим переданые параметры                
                settings.Init(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (settings.ShowHelp)
            {
                ShowHelp();
                return;
            }
            NodeCreator nodeCreator = new NodeCreator(settings);
            TreeView treeView = new TreeView();            
            Node rootNode = nodeCreator.CreateTreeNode();
            treeView.PrintNodes(rootNode);            
        }

        private static void ShowHelp()
        {
            Console.WriteLine("Отображение дерева папок.");
            Console.WriteLine("Tree [Путь к папке] [Опции]");
            Console.WriteLine("Путь к папке - корневая папка откуда будет строится дерево. Если путь не задан, то берется текущая директория. ");
            Console.WriteLine("Опции:");
            Console.WriteLine("-s или --size Отображать размер файлов");
            Console.WriteLine("-h или --human-readable Отображать размер файлов в удобном виде");
            Console.WriteLine("-d или --depth Задает глубину просмотра директорий");
            Console.WriteLine("-do Задает напрвление сортировки. Если флаг указан, то сортировка идет в обратном порядке");
            Console.WriteLine("-os Сортировать файлы по размеру");
            Console.WriteLine("-oс Сортировать файлы и папки по дате создания");
            Console.WriteLine("-om Сортировать файлы и папки по дате модификации");
            Console.WriteLine("-? или --help Вывод справки о программе");
        }
       
    }
}
