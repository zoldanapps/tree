using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tree;
using Tree.Models;

namespace TreeTest
{
    [TestClass]
    public class TestUnit1
    {
        private const string RootFolderName = "RootFolder";

        private string RootFolderPath => Path.Combine(Environment.CurrentDirectory, RootFolderName);

        private Settings _settings = new Settings();

        /// <summary>
        /// Тест проверки парсинга передаваемых параметров
        /// </summary>
        [TestMethod]
        public void SettingsParserTest()
        {
           
           string[] args = { RootFolderPath, "--depth=3", "-s", "--human-readable"};

            _settings.Init(args);

            Assert.IsTrue(_settings.ShowSize);
            Assert.IsTrue(_settings.HumanReadable);
            Assert.IsTrue(_settings.Depth == 3);
            Assert.IsTrue(_settings.RootFolder == RootFolderPath);
        }

        /// <summary>
        /// Тест сортировки по имени в прямом направлении
        /// </summary>
        [TestMethod]
        public void SortByNameAscendingTest()
        {
            string[] args = { RootFolderPath };
            _settings.Init(args);

            NodeCreator nodeCreator = new NodeCreator(_settings);
            Node node = nodeCreator.CreateTreeNode();
            Assert.IsTrue(node.Children[0].Name == "abc.txt");
            Assert.IsTrue(node.Children[1].Name == "bac.txt");
            Assert.IsTrue(node.Children[2].Name == "A");
            Assert.IsTrue(node.Children[3].Name == "B");
            Assert.IsTrue(node.Children[4].Name == "C");
            Assert.IsTrue(node.Children[5].Name == "V1");
        }


        /// <summary>
        /// Тест сортировки по имени в обратном направлении
        /// </summary>
        [TestMethod]
        public void SortByNameDescendingTest()
        {
            string[] args = { RootFolderPath, "-do" };
            _settings.Init(args);

            NodeCreator nodeCreator = new NodeCreator(_settings);
            Node node = nodeCreator.CreateTreeNode();
            Assert.IsTrue(node.Children[0].Name == "bac.txt");
            Assert.IsTrue(node.Children[1].Name == "abc.txt");
            Assert.IsTrue(node.Children[2].Name == "V1");
            Assert.IsTrue(node.Children[3].Name == "C");
            Assert.IsTrue(node.Children[4].Name == "B");
            Assert.IsTrue(node.Children[5].Name == "A");
        }

        /// <summary>
        /// Тест сортировки по дате создания в прямом направление
        /// </summary>
        [TestMethod]
        public void SortByCreationDateAscendingTest()
        {
            string[] args = { RootFolderPath, "-oc" };
            _settings.Init(args);

            NodeCreator nodeCreator = new NodeCreator(_settings);
            Node node = nodeCreator.CreateTreeNode();
            Assert.IsTrue(node.Children[0].Name == "abc.txt");
            Assert.IsTrue(node.Children[1].Name == "bac.txt");
            Assert.IsTrue(node.Children[2].Name == "A");
            Assert.IsTrue(node.Children[3].Name == "B");
            Assert.IsTrue(node.Children[4].Name == "C");
            Assert.IsTrue(node.Children[5].Name == "V1");
        }

        /// <summary>
        /// Тест сортировки по дате создания в обратном направление (последний элемент - первый созданный)
        /// </summary>
        [TestMethod]
        public void SortByCreationDateDescendingTest()
        {
            string[] args = { RootFolderPath, "-oc", "-do" };
            _settings.Init(args);

            NodeCreator nodeCreator = new NodeCreator(_settings);
            Node node = nodeCreator.CreateTreeNode();
            Assert.IsTrue(node.Children[0].Name == "bac.txt");
            Assert.IsTrue(node.Children[1].Name == "abc.txt");
            Assert.IsTrue(node.Children[2].Name == "V1");
            Assert.IsTrue(node.Children[3].Name == "C");
            Assert.IsTrue(node.Children[4].Name == "B");
            Assert.IsTrue(node.Children[5].Name == "A");
        }

        /// <summary>
        /// Тест сортировки по размеру в прямом направление (первый элемент - самый маленький)
        /// </summary>
        [TestMethod]
        public void SortBySizeAscendingTest()
        {
            string[] args = { RootFolderPath, "-os",};
            _settings.Init(args);

            NodeCreator nodeCreator = new NodeCreator(_settings);
            Node node = nodeCreator.CreateTreeNode();
            Assert.IsTrue(node.Children[0].Name == "abc.txt");
            Assert.IsTrue(node.Children[1].Name == "bac.txt");            
        }

        /// <summary>
        /// Тест сортировки по размеру в обратном направление (первый элемент - самый большой)
        /// </summary>
        [TestMethod]
        public void SortBySizeDescendingTest()
        {
            string[] args = { RootFolderPath, "-os", "-do" };
            _settings.Init(args);

            NodeCreator nodeCreator = new NodeCreator(_settings);
            Node node = nodeCreator.CreateTreeNode();
            Assert.IsTrue(node.Children[0].Name == "bac.txt");
            Assert.IsTrue(node.Children[1].Name == "abc.txt");
        }

        /// <summary>
        /// Тест на глубину вложения (уровень 2 (до V2))
        /// </summary>
        [TestMethod]
        public void Depth2Test()
        {
            string[] args = { RootFolderPath, "--depth=2" };
            _settings.Init(args);

            NodeCreator nodeCreator = new NodeCreator(_settings);
            Node node = nodeCreator.CreateTreeNode();
            Assert.IsTrue(node.Children[5].Children.Count == 1); // V2
            Assert.IsTrue(node.Children[5].Children[0].Children.Count == 0);
        }

        /// <summary>
        /// Тест на глубину вложения (уровень 3 (до V3))
        /// </summary>
        [TestMethod]
        public void Depth3Test()
        {
            string[] args = { RootFolderPath, "--depth=3" };
            _settings.Init(args);

            NodeCreator nodeCreator = new NodeCreator(_settings);
            Node node = nodeCreator.CreateTreeNode();
            Assert.IsTrue(node.Children[5].Children.Count == 1); // V2
            Assert.IsTrue(node.Children[5].Children[0].Children.Count == 1); //V3
        }

        /// <summary>
        /// Тест на глубину вложения (уровень 1)
        /// </summary>
        [TestMethod]
        public void Depth1Test()
        {
            string[] args = { RootFolderPath, "--depth=1" };
            _settings.Init(args);

            NodeCreator nodeCreator = new NodeCreator(_settings);
            Node node = nodeCreator.CreateTreeNode();
            Assert.IsTrue(node.Children[5].Children.Count == 0);             
        }
    }
}
