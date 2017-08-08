using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TestLibrary
{
    public class DllManager
    {
        public const int DLL_VERSION = 99;

        string url = @"http://esteh.mobi/sync/sync.json";
        private const string DLL_NAME = "TestLibrary.dll";
        private const string TEMP_DLL_NAME = "Temp_" + DLL_NAME;

        public bool checkIfNewDllExist()
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param["test"] = "test";
            string responseJson = POSTRequest(param, url);

            if (responseJson != "")
            {
                Dictionary<string, Object> jsonDict = JsonManager.Deserialize(responseJson);
                int newVersion = int.Parse((string)jsonDict["version"]);

                LogUtil.Log(" Current dll version: " + DLL_VERSION);
                LogUtil.Log(" Server dll version: " + newVersion);

                if ( DLL_VERSION < newVersion)
                {                  
                    string urlForDownloadingNewDll = (string)jsonDict["url"];
                    bool isDownloaded = DownloadNewDll(urlForDownloadingNewDll).Result;

                    if (isDownloaded)
                    {
                        string hashFromServer = (string)jsonDict["checksum"];
                        bool isFileHashOK = checkHash(hashFromServer);
                        if (isFileHashOK)
                        {
                            LogUtil.Log(" Hashes are equal");
                            return true;
                        }
                        else
                        {
                            LogUtil.Log(" Different hash");
                            //izbrisi fajl
                            return false;
                        }
                    }
                }
            }
            return false;
        }

        private bool checkHash(string hashFromServer)
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            string newDllPath = Path.Combine(currentPath, TEMP_DLL_NAME);
            string hash = HashChecker.GetHashFromFile(newDllPath, HashChecker.SHA256);

            return hash.Equals(hashFromServer)? true : false;
        }

        private async Task<bool> DownloadNewDll(string url)
        {
            bool isDownloaded = false;
            
            try
            {
                using (var clientHandler = new HttpClientHandler())
                {
                    //clientHandler.ServerCredential = GetCredentials();
                    using (var httpClient = new HttpClient(clientHandler))
                    {
                        using (var response  = httpClient.GetAsync(url).Result)
                        {
                            if (response.IsSuccessStatusCode)
                            {
                                using (Stream streamToReadFrom = response.Content.ReadAsStreamAsync().Result)
                                {
                                    string currentPath = AppDomain.CurrentDomain.BaseDirectory;


                                    string fileToWriteTo = Path.Combine(currentPath, TEMP_DLL_NAME);
                                    using (Stream streamToWriteTo = File.Open(fileToWriteTo, FileMode.Create))    //create overwrite-uje ako postoji fajl pod tim imenom
                                    {
                                        //streamToReadFrom.CopyToAsync(streamToWriteTo).RunSynchronously();
                                        await streamToReadFrom.CopyToAsync(streamToWriteTo);
                                        isDownloaded = true;
                                        LogUtil.Log(" New dll downloaded");
                                    }
                                }
                            }
                        }

                        //byte[] data = httpClient.GetByteArrayAsync(url).Result;                    
                    }
                }
            }
            catch (Exception ex)
            {
                isDownloaded = false;
            }
            return isDownloaded;
        }

        private string POSTRequest(Dictionary<string, string> parameters, string url)
        {
            string returnValue = "";

            var formattedData = new FormUrlEncodedContent(parameters);

            using (var clientHandler = new HttpClientHandler())
            {
                //clientHandler.ServerCredential = GetCredentials();
                using (var httpClient = new HttpClient(clientHandler))
                {
                    //var response = httpClient.PostAsync(url, formattedData).Result;
                    var response = httpClient.GetAsync(url).Result;

                    if (response.IsSuccessStatusCode) //200 OK
                    {
                        var content = response.Content;
                        returnValue = content.ReadAsStringAsync().Result;
                    }
                }
            }
            return returnValue;
        }


        private bool deleteDll()
        {
            bool isDeleted = false;
            //string currentPath = Directory.GetCurrentDirectory();      proveri da li ovako ili ispod
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;  //esync sqlite nacin 
            string filePathToDelete = Path.Combine(currentPath, TEMP_DLL_NAME);

            if (File.Exists(filePathToDelete))
            {
                try
                {
                    File.Delete(filePathToDelete);
                    isDeleted = true;
                }
                catch (Exception)
                {
                    //log
                    isDeleted = false;
                }
            }
            return isDeleted;
        }
    }
}
