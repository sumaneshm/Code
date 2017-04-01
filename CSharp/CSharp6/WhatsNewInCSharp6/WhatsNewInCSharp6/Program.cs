using System;

// C# 6.0 => New feature 1 : including static in here will allow us to include static class 
using static System.Console;

namespace WhatsNewInCSharp6
{
    public class Student
    {
        public Guid CustomerId { get; set; } = Guid.NewGuid();
    }
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("This is a test");
        }
    }
}
