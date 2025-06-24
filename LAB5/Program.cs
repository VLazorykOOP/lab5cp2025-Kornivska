using System;
using System.Windows.Forms;
using ArchimedeanTree;  // Простір імен, де знаходиться Form2

namespace LAB5
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form2());
        }
    }
}
