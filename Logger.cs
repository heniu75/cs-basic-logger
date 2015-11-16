using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicLogger
{
    public enum LogLevel
    {
        Debug = 0,
        Warning,
        Error,
        Critical
    }
    public static class Logger
    {

        private static string _logFolderName;
        /// <summary>
        /// The folder name in the AppData/Local/ to put the logs in.
        /// </summary>
        public static string LogFolderName
        {
            get
            {
                return _logFolderName ?? System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            }
            set
            {
                _logFolderName = value;
            }
        }

        private static string _logLocation;

        /// <summary>
        /// The location where the logs are stored on the computer.
        /// By default, it's stored in the C:\Users\%currentuser%\AppData\Local\%application folder%
        /// </summary>
        public static string LogLocation
        {
            get
            {
                return _logLocation ?? System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData), LogFolderName, "logs");
            }
            set
            {
                _logLocation = value;
            }
        }

        private static int _logExpirationTime = 30;

        /// <summary>
        /// The time (in days) before a log is deleted from the log folder.
        /// </summary>
        public static int LogExpirationTime
        {
            get
            {
                return _logExpirationTime;
            }
            set
            {
                _logExpirationTime = value;
            }
        }

        /// <summary>
        /// The filename of the log file.
        /// By default, it's the current date.
        /// </summary>
        public static string LogFileName
        {
            get
            {
                return String.Format("{0:MM.dd.yy}.txt", DateTime.Today);
            }
        }

        private static void CleanLogDirectory()
        {
            if (!System.IO.Directory.Exists(LogLocation))
            {
                return;
            }

           string[] filesInLogDirectory = System.IO.Directory.GetFiles(LogLocation);
           foreach (string file in filesInLogDirectory)
           {
               string fileTitle = System.IO.Path.GetFileNameWithoutExtension(System.IO.Path.Combine(LogLocation, file));
               DateTime fileDate = DateTime.Parse(fileTitle);
               DateTime ExpiredDateThreshold = DateTime.Today.Subtract(new TimeSpan(LogExpirationTime, 0, 0, 0));
               if (fileDate < ExpiredDateThreshold)
               {
                   try
                   {
                       System.IO.File.Delete(System.IO.Path.Combine(LogLocation, file));
                   }
                   catch (System.IO.IOException exception)
                   {
                       System.Diagnostics.Debug.WriteLine(exception.Message);
                   }
                   catch (Exception exception)
                   {
                       System.Diagnostics.Debug.WriteLine(exception.Message);
                   }
               }
               // System.Diagnostics.Debug.WriteLine((DateTime.Parse(fileTitle) < (DateTime.Today.Subtract(new TimeSpan(30, 0, 0, 0)))).ToString());
           }
        }

        /// <summary>
        /// Logs the submitted string into the log file.  This function will append or prepend any necessary info.
        /// </summary>
        /// <param name="line">The line to be submitted to the log file and printed into the diagnostic console.</param>
        public static void Log(string line, LogLevel logLevel)
        {
            // Start by cleaning the log folder, because this is a good time to do it.
            CleanLogDirectory();

            // prepend current time to beginning of line
            string currentTime = String.Format("{0:HH:mm:ss}", DateTime.Now) + ":  ";

            line = String.Format("{0, -10}:  {1} {2}", logLevel, currentTime, line);
            string fullFilePath = System.IO.Path.Combine(LogLocation, LogFileName);
            System.Diagnostics.Debug.WriteLine(line);

            bool directoryExists = System.IO.Directory.Exists(LogLocation);
            bool fileExists = System.IO.File.Exists(fullFilePath);

            if (!directoryExists)
            {
                try
                {
                    System.IO.Directory.CreateDirectory(LogLocation);
                }
                catch (System.IO.IOException exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception.Message);
                    return;
                }
            }

            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(@fullFilePath, fileExists))
            {
                file.WriteLine(line);
            }
        }
    }
}
