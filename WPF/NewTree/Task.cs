// This collection of non-binary tree data structures created by Dan Vanderboom.
// Critical Development blog: http://dvanderboom.wordpress.com
// Original Tree<T> blog article: http://dvanderboom.wordpress.com/2008/03/15/treet-implementing-a-non-binary-tree-in-c/

using System;
using System.Collections.Generic;
using System.Text;

namespace TreeDataStructure
{
    public class Task : ComplexTreeNode<Task>
    {
        public string Name;

        private bool _Complete;
        public bool Complete
        {
            get { return _Complete; }
            set
            {
                // if this task is complete, then all child tasks must also be complete
                if (value)
                {
                    foreach (Task task in Children)
                    {
                        task.Complete = true;
                    }
                }

                _Complete = value;
            }
        }

        public Task(string Name)
        {
            this.Name = Name;
            Complete = false;
        }
    }
}