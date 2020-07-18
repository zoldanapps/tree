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
           string[] args = { "--depth=3", "-s", "--human-readable",};

            OptionsParser optionsParser = new OptionsParser();            
            optionsParser.Parse(args);

            Assert.IsTrue(optionsParser.GetOptionAsBool("s", "size"));
            Assert.IsTrue(optionsParser.GetOptionAsBool("h", "human-readable"));
            Assert.IsTrue(optionsParser.GetOptionAsInt("d", "depth") == 3);

        }
    }
}
