using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tree;

namespace TreeTest
{
    [TestClass]
    public class OptionsParserTest
    {        
        [TestMethod]
        public void ParserTest()
        {
            string rootFolder = "C:/Windows/Temp";
           string[] args = {rootFolder , "--depth=3", "-s", "--human-readable"};

            Settings optionsParser = new Settings();            
            optionsParser.Init(args);

            Assert.IsTrue(optionsParser.ShowSize);
            Assert.IsTrue(optionsParser.HumanReadable);
            Assert.IsTrue(optionsParser.Depth == 3);
            Assert.IsTrue(optionsParser.RootFolder == rootFolder);

        }
    }
}
