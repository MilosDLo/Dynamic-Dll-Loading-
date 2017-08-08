using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary
{
    public static class LogUtil
    {
        static string eventSource = "MySource";
        static string eventLogString = "MyNewLog";

        public static void Log(string msg)
        {  
            if (!System.Diagnostics.EventLog.SourceExists(eventSource))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSource, eventLogString);
            }

            Console.WriteLine(msg);
            EventLog.WriteEntry(eventSource, msg);

        }


    }
}
