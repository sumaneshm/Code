using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeLibrary
{
    public class NodeTreeGenerator
    {
        public static Node GenerateTree()
        {
            Node node1 = new Node("Node 1", 101, 1001, NodeType.Artificial);
            Node node2 = new Node("Node 2", 201, 2001, NodeType.Official);
            Node node3 = new Node("Node 3", 301, 3001, NodeType.Generic);
            Node node4 = new Node("Node 4", 401, 4001, NodeType.Artificial);
            Node node5 = new Node("Node 5", 501, 5001, NodeType.Generic);
            Node node6 = new Node("Node 6", 601, 6001, NodeType.Official);
            Node node7 = new Node("Node 7", 701, 7001, NodeType.Artificial);

            node1.Children.Add(node2);
            node2.Children.Add(node3);
            node2.Children.Add(node4);
            node1.Children.Add(node5);
            node5.Children.Add(node6);
            node5.Children.Add(node7);
            for (int i = 8; i <= 100; i++)
            {
                Node node = new Node("Node " + i, i * 100 + 1, i * 1000 + 1, NodeType.Artificial);
                node5.Children.Add(node);
            }

            return node1;
        }
    }
}
