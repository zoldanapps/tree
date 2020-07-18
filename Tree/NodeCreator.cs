using System;
using System.IO;
using System.Linq;
using Tree.Models;

namespace Tree
{
    public class NodeCreator
    {

        private const int GB = 1024 * 1024 * 1024;
        private const int MB = 1024 * 1024;
        private const int KB = 1024;

        private int _rootDepth;

        public int Depth { get; set; }

        public SortOrder SortOrder { get; set; } = SortOrder.Ascending;

        public OrderBy OrderBy { get; set; } = OrderBy.Name;

        public bool IsShowSize { get; set; }

        public bool IsHumanReadable { get; set; }

        public Node CreateTreeNode (string rootDirectory)
        {
            _rootDepth = GetDepth(rootDirectory);
            Node node = new Node();
            CreateNode(rootDirectory, node);
            return node;
        }

        private  void CreateNode(string path, Node node)
        {
            int depth = GetDepth(path) - _rootDepth;

            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            node.Name = directoryInfo.Name;

            var files = SortFileList(directoryInfo.GetFiles());

            foreach (FileInfo file in files)
            {
                Node fileNode = new Node();
                if (IsShowSize)
                {
                    fileNode.Name = IsHumanReadable ? $"{file.Name} {GetReadableLength(file.Length)}" : $"{file.Name} ({file.Length} B)";
                }
                else
                {
                    fileNode.Name = file.Name;
                }
                node.Children.Add(fileNode);
            }
            if (depth == Depth)
                return;

            var dirs = SortFolderList(directoryInfo.GetDirectories());
            foreach (DirectoryInfo dir in dirs)
            {
                Node childrenNode = new Node();
                node.Children.Add(childrenNode);
                CreateNode(dir.FullName, childrenNode);
            }
        }

        private int GetDepth(string path)
        {
            return path.Count(s => s.Equals('\\'));
        }
        private IOrderedEnumerable<FileInfo> SortFileList(FileInfo[] files)
        {
            if (OrderBy == OrderBy.Size)
                return SortOrder == SortOrder.Ascending ? files.OrderBy(f => f.Length) : files.OrderByDescending(f => f.Length);

            if (OrderBy == OrderBy.CreateDate)
                return SortOrder == SortOrder.Ascending ? files.OrderBy(f => f.CreationTime) : files.OrderByDescending(f => f.CreationTime);

            if (OrderBy == OrderBy.ModifyDate)
                return SortOrder == SortOrder.Ascending ? files.OrderBy(f => f.LastAccessTime) : files.OrderByDescending(f => f.LastAccessTime);
            return SortOrder == SortOrder.Ascending ? files.OrderBy(f => f.Name) : files.OrderByDescending(f => f.Name);

        }

        private IOrderedEnumerable<DirectoryInfo> SortFolderList(DirectoryInfo[] folders)
        {
            if (OrderBy == OrderBy.CreateDate)
                return SortOrder == SortOrder.Ascending ? folders.OrderBy(d => d.CreationTime) : folders.OrderByDescending(d => d.CreationTime);

            if (OrderBy == OrderBy.ModifyDate)
                return SortOrder == SortOrder.Ascending ? folders.OrderBy(d => d.LastAccessTime) : folders.OrderByDescending(d => d.LastAccessTime);
            return SortOrder == SortOrder.Ascending ? folders.OrderBy(f => f.Name) : folders.OrderByDescending(f => f.Name);
        }

        private string GetReadableLength(decimal fileLength)
        {
            decimal length = 0;
            string result = string.Empty;
            if (fileLength == 0)
                result = "empty";

            if (fileLength >= GB)
            {
                length = Math.Round((fileLength / GB), MidpointRounding.AwayFromZero);
                result = length.ToString() + " GB";
            }
            if (fileLength >= MB)
            {
                length = Math.Round((fileLength / MB), MidpointRounding.AwayFromZero);
                result = length.ToString() + " MB";
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
