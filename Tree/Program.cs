using System;
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
                // парсим переданные параметры
                
                settings.Init(args);

                //nodeCreator.IsHumanReadable = settings.GetOptionAsBool("h", "human-readable");
                //nodeCreator.IsShowSize = settings.GetOptionAsBool("s", "size");
                //nodeCreator.Depth = settings.GetOptionAsInt("d", "depth", -1);
                //nodeCreator.SortOrder = settings.GetOptionAsBool("do", "descending-order") == false ? SortOrder.Ascending : SortOrder.Descending;
                //if (settings.GetOptionAsBool("oc", "order-by-creation-date") == true)
                //    nodeCreator.OrderBy =  OrderBy.CreateDate;
                //if (settings.GetOptionAsBool("os", "order-by-size") == true)
                //    nodeCreator.OrderBy = OrderBy.Size;
                //if (settings.GetOptionAsBool("om", "order-by-modefy-date") == true)
                //    nodeCreator.OrderBy = OrderBy.ModifyDate;
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
