using FileHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LagDaemon.GameEngine.DAL
{
    /// <summary>
    /// A Data Context for access to the database files.  
    /// </summary>
    public class DataContext
    {
        private string _basePath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        public DataContext(string path)
        {
            _basePath = path;
        }

        private void writeLogEntry()
        {
            string logPath = Path.Combine(@"C:\Users\wwestlake\AppData\Roaming\LagDaemon\GameEngine\logs", "ServerLog.txt");

            var engine = new FileHelperEngine<ServerLogEntry>();

            var log = new List<ServerLogEntry>();

            log.Add(new ServerLogEntry()
            {
                Date = DateTime.Now,
                PlayerID = "LagDaemon",
                EntryType = LogEntryType.Information,
                EntrySeverity = LogEntrySeverity.Low,
                Entry = "This is a test log entry"
            });

            engine.AppendToFile(logPath, log);
        }


    }
}
