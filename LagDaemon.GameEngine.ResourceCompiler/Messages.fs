module Messages


type IOResult<'T, 'F> =
    | IOSuccess of 'T
    | IOFailure of 'F


type MessageType<'T> = MessageType of 'T

let msg x = MessageType x

type ErrorMessages =
    | FileNotFound                  of MessageType<string>
    | ConnectionTimedOut            of MessageType<string>
    | WrongPasswordOrLoginId        of MessageType<string>
    | UnauthorizedAccessException   of MessageType<string>
    | ArgumentNullException         of MessageType<string>
    | PathTooLongException          of MessageType<string>
    | ArgumentException             of MessageType<string>
    | DirectoryNotFoundException    of MessageType<string>
    | NotSupportedException         of MessageType<string>
    | IOException                   of MessageType<string>

let getMessage message = 
    match message with
    | FileNotFound (MessageType msg) -> sprintf "File Not Found '%s'" msg
    | ConnectionTimedOut  (MessageType msg) -> sprintf "Connection Timed Out '%s'" msg
    | WrongPasswordOrLoginId  (MessageType msg) -> sprintf "Wrong Password or Login ID '%s'" msg
    | UnauthorizedAccessException (MessageType msg) -> sprintf "Unauthorized Access Exception  '%s'" msg
    | ArgumentNullException (MessageType msg) -> sprintf "Argument Null Exception '%s'" msg
    | PathTooLongException (MessageType msg) -> sprintf "Path Too Long Exception '%s'" msg
    | ArgumentException (MessageType msg) -> sprintf "Argument Exception '%s'" msg
    | DirectoryNotFoundException (MessageType msg) -> sprintf "Directory Not Found Exception '%s'" msg
    | NotSupportedException (MessageType msg) -> sprintf "Not Supported Exception '%s'" msg
    | IOException (MessageType msg) -> sprintf "IO Exception '%s'" msg



type IOFailureData = {
    Operation: string;
    File: string;
    Exception: ErrorMessages;
}

let createFailureData op file ex =
    {
        Operation = op;
        File = file;
        Exception = ex;
    }

let getIOErrorMessage m = 
    match m with
    | IOSuccess _ -> ""
    | IOFailure { Operation=op; File=f; Exception=ex } -> sprintf "Exception: %s\nMessage: %s\nFilepath: %s\n" (getMessage ex) op f 

