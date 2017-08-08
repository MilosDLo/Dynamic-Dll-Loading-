using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestLibrary
{
    [Serializable]
    public class EntryPointClass 
    {
        EventLog eventLogInstance; //izbrisi,vise ne treba jer koristim LogUtil


        public EntryPointClass(EventLog eventLog)
        {
            eventLogInstance = eventLog;
        }
        public EntryPointClass()
        {
            eventLogInstance = null;
        }


        public int Run(EventLog eventLog)
        {
            eventLogInstance = eventLog;

            ConcurrentDictionary<string, bool> runningThreads = new ConcurrentDictionary<string, bool>();

            DllManager dllManager = new DllManager();

            Thread helloThread = new Thread(() => CheckForNewDll(runningThreads));
            helloThread.IsBackground = true;
            helloThread.Start();


            Thread exportThread = new Thread(() => DoingSomething("export 11", runningThreads));
            exportThread.IsBackground = true;
            exportThread.Start();


            Thread importThread = new Thread(() => DoingSomething("import 11", runningThreads));
            importThread.IsBackground = true;
            importThread.Start();


            while (true)
            {
                Thread.Sleep(5000);

                bool areAllThreadsStopped = runningThreads.All(x => x.Value == false);

                if (areAllThreadsStopped)
                {
                    LogUtil.Log("***** ALL THREADS HAVE BEEN STOPPED *****");
                    return 1;
                }
            }
        }

        private void CheckForNewDll(ConcurrentDictionary<string, bool> runningThreads)
        {
            runningThreads["newDllThread"] = true;

            while (true)
            {

                Thread.Sleep(6000);

                //Thread helloThread = new Thread(() => Desc());
                //helloThread.IsBackground = true;
                //helloThread.Start();

                bool newDll = DllCheck();
                if (newDll)
                {
                    runningThreads["newDllThread"] = false;
                    return;
                }

            }
        }

        private bool DllCheck()
        {
            DllManager dllManager = new DllManager();
            //bool newDll = false;

            LogUtil.Log("***** Checking for new dll.. *****");

            bool exist = dllManager.checkIfNewDllExist();
            if (exist)
            {
                LogUtil.Log("******** NEW DLL *********");
                //NewDllExist = true;
                return true;
            }
            return false;
        }

        private void DoingSomething(string name, ConcurrentDictionary<string, bool> runningThreads)
        {
            runningThreads[name] = true;

            while (true)
            {
                LogUtil.Log(name.ToUpper() + ": doing something...");

                //nesto radi
                Thread.Sleep(1000);

                LogUtil.Log("----" + name.ToUpper() + "--  check if there is new dll");

                if (runningThreads.ContainsKey("newDllThread"))
                {
                    if (runningThreads["newDllThread"] == false)
                    {
                        LogUtil.Log("----" + name.ToUpper() + "--  new dll exists,STOPPING THREAD..");
                        runningThreads[name] = false;
                        return;
                    } 
                }
            }
        }
        public void Desc()
        {
            Console.WriteLine("22222");
            Thread.Sleep(300); //0.3s
        }




    }
}
