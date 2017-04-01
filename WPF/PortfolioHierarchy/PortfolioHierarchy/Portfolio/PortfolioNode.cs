using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarsDataStructures.Tree;

namespace PortfolioHierarchy.Portfolio
{
    public class PortfolioNode : TreeNode<PortfolioNode>
    {
        private int _portfolioId;

        public int PortfolioId
        {
            get { return _portfolioId; }
            set { _portfolioId = value; }
        }

        private bool _Complete;
        public bool Complete
        {
            get { return _Complete; }
            set
            {
                // if this task is complete, then all child tasks must also be complete
                if (value)
                {
                    foreach (PortfolioNode task in Children)
                    {
                        task.Complete = true;
                    }
                }

                _Complete = value;
            }
        }

        double[] var = new double[15];
        double[,] pnl = new double[15, 520];

        public PortfolioNode(int PortfolioId)
        {
            this.PortfolioId = PortfolioId;
            Complete = false;
        }
    }
}
