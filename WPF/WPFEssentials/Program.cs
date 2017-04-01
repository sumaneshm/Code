using System;
using System.Windows;

public class Program
{
    [STAThread()]
    static void Main()
    {
        Application app = new Application();
        Window w = new Window();
        w.Title = "Hellow WPF essentials";
        app.Run(w);
    }
}
