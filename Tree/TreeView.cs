using System;
using Tree.Models;

namespace Tree
{
    public class TreeView
    {        

        private const string Cross = " ├─";
        private const string Corner = " └─";
        private const string Vertical = " │ ";
        private const string Space = "   ";     


        public void PrintNodes(Node node)
        {
            PrintNode(node);
        }

        private void PrintNode(Node node, string indent = "")
        {
            Console.WriteLine(node.Name);
           
            var numberOfChildren = node.Children.Count;
            for (var i = 0; i < numberOfChildren; i++)
            {
                var child = node.Children[i];
                var isLast = (i == (numberOfChildren - 1));
                PrintChildNode(child, indent, isLast);
            }
        }

        private void PrintChildNode(Node node, string indent, bool isLast)
        {            
            Console.Write(indent);
         
            if (isLast)
            {
                Console.Write(Corner);
                indent += Space;
            }
            else
            {
                Console.Write(Cross);
                indent += Vertical;
            }

            PrintNode(node, indent);
        }       
    }
}
