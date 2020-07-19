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

        private readonly Settings _settings;
        private int _rootDepth;

       

        public NodeCreator(Settings settigs)
        {
            _settings = settigs;
        }

        public Node CreateTreeNode ()
        {
            _rootDepth = GetDepth(_settings.RootFolder);
            Node node = new Node();
            CreateNode(_settings.RootFolder, node);
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
                if (_settings.ShowSize)
                {
                    fileNode.Name = _settings.HumanReadable ? $"{file.Name} {GetReadableLength(file.Length)}" : $"{file.Name} ({file.Length} B)";
                }
                else
                {
                    fileNode.Name = file.Name;
                }
                node.Children.Add(fileNode);
            }
            if (depth == _settings.Depth)
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
            if (_settings.OrderBy == OrderBy.Size)
                return _settings.SortOrder == SortOrder.Ascending ? files.OrderBy(f => f.Length) : files.OrderByDescending(f => f.Length);

            if (_settings.OrderBy == OrderBy.CreateDate)
                return _settings.SortOrder == SortOrder.Ascending ? files.OrderBy(f => f.CreationTime) : files.OrderByDescending(f => f.CreationTime);

            if (_settings.OrderBy == OrderBy.ModifyDate)
                return _settings.SortOrder == SortOrder.Ascending ? files.OrderBy(f => f.LastAccessTime) : files.OrderByDescending(f => f.LastAccessTime);
            return _settings.SortOrder == SortOrder.Ascending ? files.OrderBy(f => f.Name) : files.OrderByDescending(f => f.Name);

        }

        private IOrderedEnumerable<DirectoryInfo> SortFolderList(DirectoryInfo[] folders)
        {
            if (_settings.OrderBy == OrderBy.CreateDate)
                return _settings.SortOrder == SortOrder.Ascending ? folders.OrderBy(d => d.CreationTime) : folders.OrderByDescending(d => d.CreationTime);

            if (_settings.OrderBy == OrderBy.ModifyDate)
                return _settings.SortOrder == SortOrder.Ascending ? folders.OrderBy(d => d.LastAccessTime) : folders.OrderByDescending(d => d.LastAccessTime);
            return _settings.SortOrder == SortOrder.Ascending ? folders.OrderBy(f => f.Name) : folders.OrderByDescending(f => f.Name);
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
