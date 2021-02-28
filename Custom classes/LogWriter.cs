using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace LiftSimulator.Custom_classes
{
    public class LogWriter
    {
        private readonly object locker = new object();
        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        string fileName = "ElevatorLog.log";
        public void Log(string message)
        {
            _readWriteLock.EnterWriteLock();
            using (FileStream fs = new FileStream(fileName, FileMode.Append))
            using (TextWriter writer = new StreamWriter(fs))
            {
                writer.Write(DateTime.Now.ToString("yyyy-MM-dd @ HH:mm:ss") + ": ");
                writer.WriteLine(message);
            }
            _readWriteLock.ExitWriteLock();

            //lock (locker)
            //{


            //    //write message to log file
            //    using (FileStream fs = new FileStream(fileName, FileMode.Append))
            //    using (TextWriter writer = new StreamWriter(fs))
            //    {
            //        writer.Write(DateTime.Now.ToString("yyyy-MM-dd @ HH:mm:ss") + ": ");
            //        writer.WriteLine(message);
            //    }
            //}
        }
    }
}