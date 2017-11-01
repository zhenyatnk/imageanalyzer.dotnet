using System;
using System.IO;

namespace ui
{
    class ObserverTask : imageanalyzer.dotnet.core.interfaces.IObserverTask
    {
        public ObserverTask(string aFileName)
        {
            m_FileName = aFileName;
        }

        public void HandleComplete()
        {
            Console.WriteLine("Conplete:\t" + m_FileName);
        }
        public void HandleStart()
        {
            Console.WriteLine("Start:\t" + m_FileName);
        }

        public void HandleError(string aMessage, int aErrorCode)
        {
            Console.WriteLine("Error:\t" + aErrorCode + " '" + aMessage + "'");
        }
        private string m_FileName;
    }


    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                imageanalyzer.dotnet.core.CAnalyzer analyzer = new imageanalyzer.dotnet.core.CAnalyzer();
                foreach (string file in Directory.GetFiles(args[0]))
                {
                    analyzer.AddTask(file, new ObserverTask(file));
                }
            }
            Console.ReadKey(true);
        }
    }
}
