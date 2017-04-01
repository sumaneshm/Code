using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MarsDataStructures.Tree;
using System.Diagnostics;
using PortfolioHierarchy.Portfolio;
using System.Runtime.Serialization.Formatters.Binary;

namespace PortfolioHierarchy
{
    class Program
    {
        static int i;
        static Process thisProc;
        static void Main(string[] args)
        {
            //Utilities.DataManager dm = new Utilities.DataManager();
            //PortfolioNode root = dm.GetPortfolioHierarchy(6, 20);

            //DisplayTaskSubtree(root, 0);

            //Console.WriteLine("NodeCount : " + dm.NodeCount + "\n\n\n\n\n");


            List<PortfolioNode> port = new List<PortfolioNode>();

            try
            {
                for (i = 0; i < int.MaxValue; i++)
                {
                    port.Add(new PortfolioNode(i));

                    if (i % 5000 == 0)
                    {
                        DisplayStats(port);
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                Console.WriteLine("Game over ");
                DisplayStats(port);
            }
            Console.ReadLine();
        }

        private static void DisplayStats(List<PortfolioNode> port)
        {
            thisProc = Process.GetCurrentProcess();
            //Console.WriteLine("ProcessName:" + thisProc.ProcessName);
            Console.WriteLine("\n------------Portfolio count : " + i + "---------------------");
            Console.WriteLine("    virtual memory: {0}MB", thisProc.VirtualMemorySize64 / 1024.0 / 1024.0);
            Console.WriteLine("    private memory: {0}MB", thisProc.PrivateMemorySize64 / 1024.0 / 1024.0);
            Console.WriteLine("    physical memory: {0}MB", thisProc.WorkingSet64 / 1024.0 / 1024.0);
        }

        private static void DisplayTaskSubtree(PortfolioNode Task, int Level)
        {
            string indent = string.Empty.PadLeft(Level * 3);
            Console.WriteLine(indent + Task.PortfolioId);

            Level++;
            foreach (PortfolioNode ChildTask in Task.Children)
            {
                DisplayTaskSubtree(ChildTask, Level);
            }
        }
    }
}
