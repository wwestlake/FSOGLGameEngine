using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileHelpers;

namespace LagDaemon.GameEngine.DAL
{
    public enum LogEntryType { Information, Warning, Error  }
    public enum LogEntrySeverity { High, Medium, Low }
     
    [DelimitedRecord("|")]
    public class ServerLogEntry
    {
        [FieldConverter(ConverterKind.Date, "dd/MM/yyyy:hh:mm:ss")]
        public DateTime Date;
        public string PlayerID;
        public LogEntryType EntryType;
        public LogEntrySeverity EntrySeverity;
        public string Entry;
    }
}
