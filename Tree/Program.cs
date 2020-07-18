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
        private static int _maxDepth = 3;
        private const int GB = 1024 * 1024 * 1024;
        private const int MB = 1024 * 1024;
        private const int KB = 1024;



        private static Node _node = new Node();


        static void Main(string[] args)
        {
            bool isHumanReadable = false;
            //if (args.Length > 0 )
            //{
            //    string arg = args.FirstOrDefault(a => a == "-h" || a == "--human-readable");
            //    if (arg != null)
            //        isHumanReadable = true;
            //    arg = args.FirstOrDefault(a => a == "-d" || a == "--depth");

            //    arg = args.FirstOrDefault(a => a == "-h" || a == "--human-readable");
            //    if (arg != null)
            //    {
            //        string[] option = arg.Split('=');
            //        if ()
            //    }

            //}

            string rootDirectory = @"d:\TMP\root";
            _rootDepth = GetDepth(rootDirectory);
            

            //_treeManager.SetRootFolder(rootDirectory);
            //_treeManager.ShowRootNode();            
            ShowTree(rootDirectory, _node, false, isHumanReadable);
            _treeManager.PrintNode(_node);
            Console.ReadKey();
        }


        private static void ShowTree(string path, Node node, bool isShowSize, bool isHumanReadable)
        {
            int depth = GetDepth(path) - _rootDepth;
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            node.Name = directoryInfo.Name;
            FileInfo[] fileInfo = directoryInfo.GetFiles();
            foreach (FileInfo file in fileInfo)
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
    
            //_treeManager.PrintNode(directoryInfo.Name, depth);
            if (depth == _maxDepth)
                return;      
            
            
            DirectoryInfo[] dirs = directoryInfo.GetDirectories();
            
           ////   //_treeLevel++;
            foreach (DirectoryInfo dir in dirs)
            {
                Node n = new Node();                
                node.Children.Add(n);           
                ShowTree(dir.FullName, n, isShowSize, isHumanReadable);
            }
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

        

        private static void ViewNode(string name)
        {
            Console.WriteLine($"+{name}");
        }

        private static void Shift()
        {
            Console.WriteLine($"  |");            
        }
    }
}
