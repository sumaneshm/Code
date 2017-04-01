using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TreeLibrary
{
     [Serializable]
    public class Node
    {
        readonly List<Node> _children = new List<Node>();

        public IList<Node> Children
        {
            get { return _children; }
        }

        public string Name { get; set; }

        public int GFRMNodeId { get; set; }

        public int ChorusNodeId { get; set; }

        public NodeType NType { get; set; }

        public Node()
        {
            _children = new List<Node>();
        }

        public Node(string Name, int GFRMNodeId, int ChorusNodeId, NodeType NType): this()
        {
            this.Name = Name;
            this.GFRMNodeId = GFRMNodeId;
            this.ChorusNodeId = ChorusNodeId;
            this.NType = NType;
        }

        public BaseNodeData Data { get; set; }
    }
}
