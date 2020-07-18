using System;
using Tree.Models;

namespace Tree
{
    class Program
    {      

        static void Main(string[] args)
        {
            NodeCreator nodeCreator = new NodeCreator();
            TreeView treeView = new TreeView();
            
            try
            {
                // парсим переданные параметры
                OptionsParser optionsParser = new OptionsParser();
                optionsParser.Parse(args);
                nodeCreator.IsHumanReadable = optionsParser.GetOptionAsBool("h", "human-readable");
                nodeCreator.IsShowSize = optionsParser.GetOptionAsBool("s", "size");
                nodeCreator.Depth = optionsParser.GetOptionAsInt("d", "depth", -1);
                nodeCreator.SortOrder = optionsParser.GetOptionAsBool("do", "descending-order") == false ? SortOrder.Ascending : SortOrder.Descending;
                if (optionsParser.GetOptionAsBool("oc", "order-by-creation-date") == true)
                    nodeCreator.OrderBy =  OrderBy.CreateDate;
                if (optionsParser.GetOptionAsBool("os", "order-by-size") == true)
                    nodeCreator.OrderBy = OrderBy.Size;
                if (optionsParser.GetOptionAsBool("om", "order-by-modefy-date") == true)
                    nodeCreator.OrderBy = OrderBy.ModifyDate;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            string rootDirectory = Environment.CurrentDirectory;
            Node rootNode = nodeCreator.CreateTreeNode(rootDirectory);
            treeView.PrintNodes(rootNode);            
        }


        

        


        

       

       
    }
}
