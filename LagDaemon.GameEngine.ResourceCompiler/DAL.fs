module DAL


open System
open System.IO
open FileHelpers
open FileResources

let createEngine<'T when 'T: not struct> () = new FileHelperEngine<'T>()

type LogSeverity = 
    | Low = 1
    | Medium = 2
    | High = 3

type LogType =
    | Info = 1
    | Warning = 2
    | Error = 3
    | Exception = 4
    | Admin = 5

[<DelimitedRecord("|")>]
type LogEntry() =
    [<FieldConverter(ConverterKind.Date, "dd/MM/yyyy-hh:mm:ss")>]
    [<DefaultValue>]val mutable Date : DateTime
    [<DefaultValue>]val mutable PlayerID : string
    [<DefaultValue>]val mutable Severity : LogSeverity
    [<DefaultValue>]val mutable LogType : LogType
    [<DefaultValue>]val mutable Entry : string


let serverLogFilePath = getFilename ClientLog
let clientLogFilePath = getFilename ServerLog

let writeLog filepath logtype severity playerid entry = 
    let log = new LogEntry()
    log.Date <- DateTime.Now
    log.PlayerID <- playerid
    log.Severity <- severity
    log.LogType <- logtype
    log.Entry <- entry
    let engine = createEngine<LogEntry>()
    engine.AppendToFile(filepath, log)

let serverLog = writeLog serverLogFilePath
let clientLog = writeLog clientLogFilePath

let serverInfoLog = serverLog LogType.Info LogSeverity.Low
let serverWarnLog = serverLog LogType.Warning LogSeverity.Low
let serverErrorLog = serverLog LogType.Error LogSeverity.Medium
let serverExceptionLog = serverLog LogType.Exception LogSeverity.High
let serverAdminLog = serverLog LogType.Admin LogSeverity.Low

let clientInfoLog = clientLog LogType.Info LogSeverity.Low
let clientWarnLog = clientLog LogType.Warning LogSeverity.Low
let clientErrorLog = clientLog LogType.Error LogSeverity.Medium
let clientExceptionLog = clientLog LogType.Exception LogSeverity.High
let clientAdminLog = clientLog LogType.Admin LogSeverity.Low


(*
do serverInfoLog "LagDaemon" "This is another entry"
do serverWarnLog "LagDaemon" "This is another entry"
do serverErrorLog "LagDaemon" "This is another entry"
do serverExceptionLog "LagDaemon" "This is another entry"
do serverAdminLog "LagDaemon" "This is another entry"

do clientInfoLog "LagDaemon" "This is another entry"
do clientWarnLog "LagDaemon" "This is another entry"
do clientErrorLog "LagDaemon" "This is another entry"
do clientExceptionLog "LagDaemon" "This is another entry"
do clientAdminLog "LagDaemon" "This is another entry"
*)


