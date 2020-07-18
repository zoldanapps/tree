using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tree.Models;

namespace Tree
{
    class Program
    {
        private static TreeManager _treeManager = new TreeManager();
        private static int _rootDepth;
        private static int _maxDepth;
        private static SortOrder _sortOrder = SortOrder.Ascending;
        private static OrderBy _orderBy = OrderBy.Name;


        private const int GB = 1024 * 1024 * 1024;
        private const int MB = 1024 * 1024;
        private const int KB = 1024;

        static void Main(string[] args)
        {
            bool isHumanReadable;
            bool isShowSize;

            try
            {
                // парсим переданные параметры
                OptionsParser optionsParser = new OptionsParser();
                optionsParser.Parse(args);
                isHumanReadable = optionsParser.GetOptionAsBool("h", "human-readable");
                isShowSize = optionsParser.GetOptionAsBool("s", "size");
                _maxDepth = optionsParser.GetOptionAsInt("d", "depth", -1);
                _sortOrder = optionsParser.GetOptionAsBool("do", "descending-order") == false ? SortOrder.Ascending : SortOrder.Descending;
                if (optionsParser.GetOptionAsBool("oc", "order-by-creation-date") == true)                
                    _orderBy =  OrderBy.CreateDate;
                if (optionsParser.GetOptionAsBool("os", "order-by-size") == true)
                    _orderBy = OrderBy.Size;
                if (optionsParser.GetOptionAsBool("om", "order-by-modefy-date") == true)
                    _orderBy = OrderBy.ModifyDate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }


            string rootDirectory = @"d:\TMP\root";
            _rootDepth = GetDepth(rootDirectory);

            Node treeNode = CreateTreeNode(rootDirectory, isShowSize, isHumanReadable);            
            _treeManager.PrintNode(treeNode);
            Console.ReadKey();
        }


        private static Node CreateTreeNode(string path, bool isShowSize, bool isHumanReadable)
        {
            Node node = new Node();
            CreateNode(path, node, isShowSize, isHumanReadable);
            return node;
        }

        private static void CreateNode(string path, Node node, bool isShowSize, bool isHumanReadable)
        {
            int depth = GetDepth(path) - _rootDepth;

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            node.Name = directoryInfo.Name;

            var files = SortFileList(directoryInfo.GetFiles());            
            
            foreach (FileInfo file in files)
            {
                Node fileNode = new Node();
                if (isShowSize)
                {
                    fileNode.Name = isHumanReadable ? $"{file.Name} {GetReadableLength(file.Length)}" : $"{file.Name} ({file.Length} B)";
                }
                else
                {
                    fileNode.Name = file.Name;
                }
                node.Children.Add(fileNode);
            }            
            if (depth == _maxDepth)
                return;

            var dirs = SortFolderList(directoryInfo.GetDirectories());            
            foreach (DirectoryInfo dir in dirs)
            {
                Node childrenNode = new Node();
                node.Children.Add(childrenNode);
                CreateNode(dir.FullName, childrenNode, isShowSize, isHumanReadable);
            }
        }


        private static IOrderedEnumerable<FileInfo> SortFileList(FileInfo[] files)
        {
            if ( _orderBy == OrderBy.Size)            
                return _sortOrder == SortOrder.Ascending ? files.OrderBy(f => f.Length) : files.OrderByDescending(f => f.Length);
            
            if (_orderBy == OrderBy.CreateDate)
                return _sortOrder == SortOrder.Ascending ? files.OrderBy(f => f.CreationTime) : files.OrderByDescending(f => f.CreationTime);

            if (_orderBy == OrderBy.ModifyDate)
                return _sortOrder == SortOrder.Ascending ? files.OrderBy(f => f.LastAccessTime) : files.OrderByDescending(f => f.LastAccessTime);
            return _sortOrder == SortOrder.Ascending ? files.OrderBy(f => f.Name) : files.OrderByDescending(f => f.Name);

        }       

        private static IOrderedEnumerable<DirectoryInfo> SortFolderList(DirectoryInfo[] folders)
        {
            if (_orderBy == OrderBy.CreateDate)
                return _sortOrder == SortOrder.Ascending ? folders.OrderBy(d => d.CreationTime) : folders.OrderByDescending(d => d.CreationTime);

            if (_orderBy == OrderBy.ModifyDate)
                return _sortOrder == SortOrder.Ascending ? folders.OrderBy(d => d.LastAccessTime) : folders.OrderByDescending(d => d.LastAccessTime);
            return _sortOrder == SortOrder.Ascending ? folders.OrderBy(f => f.Name) : folders.OrderByDescending(f => f.Name);
        }       

        private static int GetDepth(string path)
        {
            return path.Count(s => s.Equals('\\'));
        }



        private static string GetReadableLength(decimal fileLength)
        {
            decimal length = 0;
            string result = string.Empty;
            if (fileLength == 0)
                result =  "empty";

            if (fileLength >= GB )
            {
                length = Math.Round((fileLength / GB), MidpointRounding.AwayFromZero);
                result =  length.ToString() + " GB";
            }
            if (fileLength >= MB)
            {
                length = Math.Round((fileLength / MB), MidpointRounding.AwayFromZero);
                result =  length.ToString() + " MB" ;
            }

            if (fileLength >= KB)
            {
                length = Math.Round((fileLength / KB), MidpointRounding.ToEven);
                result = length.ToString() + " KB";
            }
            if (fileLength > 0 && fileLength < KB)
                result = fileLength.ToString() + " B";

            return $"({result})";
        }     
    }
}
