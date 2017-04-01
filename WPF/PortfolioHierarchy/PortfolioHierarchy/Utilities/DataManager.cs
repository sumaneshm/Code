using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PortfolioHierarchy.Portfolio;

namespace PortfolioHierarchy.Utilities
{
    class DataManager
    {
        int childCount = 5;
        int maxLevel = 2;
        int runningNodeId = 1;

        public int NodeCount
        {
            get { return runningNodeId; }
            set { runningNodeId = value; }
        }

        public PortfolioNode GetPortfolioHierarchy(int childCount=5,int maxLevel=10)
        {
            this.childCount = childCount;
            this.maxLevel = maxLevel;

            PortfolioNode node = new PortfolioNode(0);
            for (int i = 1; i <= childCount; i++)
            {
                node.Children.Add(new PortfolioNode(runningNodeId++));
            }
            PopulateNode(node, 1);
            return node;

        }

        public void PopulateNode(PortfolioNode node, int currentLevel)
        {
            if (currentLevel >= maxLevel)
                return;
            foreach (PortfolioNode child in node.Children)
            {
                for (int i = 0; i < childCount; i++)
                {
                    child.Children.Add(new PortfolioNode(runningNodeId++));
                }
                PopulateNode(child, ++currentLevel);
            }

        }

    }
}
