using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceDll
{
    public partial class Service1 : ServiceBase
    {

        static string eventSource = "MySource";
        static string eventLog = "MyNewLog";

        private const string DLL_NAME = "TestLibrary.dll";
        private const string DLL_ENTRY_CLASS_NAME = "EntryPointClass";
        private const string DLL_ENTRY_CLASS_FULL_NAME = "TestLibrary.EntryPointClass"; //namespace + class name
        private const string TEMP_DLL_NAME = "Temp_" + DLL_NAME;
        private const string OLD_DLL_NAME = "Old_" + DLL_NAME;

        public Service1()
        {
            InitializeComponent();

            if (!System.Diagnostics.EventLog.SourceExists("MySource"))
            {
                System.Diagnostics.EventLog.CreateEventSource("MySource", "MyNewLog");
            }
            eventLog1.Source = eventSource;
            eventLog1.Log = eventLog;
        }

        protected override void OnStart(string[] args)
        {
            eventLog1.WriteEntry("Service has been started");

            Thread helloThread = new Thread(() => StartServiceThread());
            helloThread.IsBackground = true;
            helloThread.Start();

            eventLog1.WriteEntry("Kraj onStart");
        }

        private void StartServiceThread()
        {
            while (true)
            {
                string currentPath = AppDomain.CurrentDomain.BaseDirectory;
                string filePath = Path.Combine(currentPath, DLL_NAME);

                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                AppDomain app = AppDomain.CreateDomain("ESTEH DOMAIN", null, setup);

                app.DoCallBack(new CrossAppDomainDelegate(LoaderCallback));


                AppDomain.Unload(app);
                GC.Collect(); // collects all unused memory
                GC.WaitForPendingFinalizers(); // wait until GC has finished its work
                GC.Collect();
                EventLog.WriteEntry(eventSource, "Old dll unloaded");

                renameDlls();
            }
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry("Service has been stopped");
        }

        private static void LoaderCallback()
        {
            EventLog.WriteEntry(eventSource, "---------------------------");
            EventLog.WriteEntry(eventSource, "LOADING NEW DLL....");

            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(currentPath, DLL_NAME);

            Loader loader = new Loader();
            Assembly assembly = loader.GetAssembly(filePath);


            if (assembly != null)
            {
                EventLog.WriteEntry(eventSource, "NEW DLL HAS BEEN LOADED");

                // umesto ovoga mogu da napravim interface IDllRunnable,koji bi bio u service i dll projektu ,i pozivam tipa:
                // IDllRunnable esyncEntry = assembly.CreateInstance("TestLibrary.EntryPointClass") as IDllRunnable;
                //if (esyncEntry == null)
                //    throw new Exception("broke");
                // esyncEntry.run();


                dynamic esyncEntry = assembly.CreateInstance(DLL_ENTRY_CLASS_FULL_NAME);
                esyncEntry.Run(null);
            }
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            return AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
        }


        private static void renameDlls()
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string newDll = Path.Combine(currentPath, TEMP_DLL_NAME);

            try
            {
                if (File.Exists(newDll))
                {
                    renameDll(DLL_NAME, OLD_DLL_NAME);
                    renameDll(TEMP_DLL_NAME, DLL_NAME);
                }
                else
                {
                    //log ne postoji sacuvan novi dll
                }
            }
            catch (Exception exc)
            {
                //log?
                throw exc;
            }
        }

        private static void renameDll(string oldName, string NewName)
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;

            string currentDll = Path.Combine(currentPath, oldName);
            string newDll = Path.Combine(currentPath, NewName);

            if (File.Exists(currentDll))
            {
                if (File.Exists(newDll))
                {
                    File.Delete(newDll);
                }
                File.Move(currentDll, newDll);
            }
        }
    }

    public class Loader : MarshalByRefObject
    {
        public Assembly GetAssembly(string assemblyPath)
        {
            try
            {
                return Assembly.LoadFile(assemblyPath);
            }
            catch (Exception)
            {
                return null;
                // throw new InvalidOperationException(ex);
            }
        }
    }
}
