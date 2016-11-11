module FileResources

open System
open System.IO
open Messages

type Directories =
    | BaseDirectory
    | AssetDirectory
    | LibraryDirectory
    | PluginDirectory
    | DataDirectory
    | DocDirectory
    | LogDirectory

type FileNames =
    | ClientLog
    | ServerLog


let relativeBaseDir = @"LagDaemon\GameEngine\"

let appDataFolder = Environment.SpecialFolder.ApplicationData

let baseDirectory = Path.Combine(Environment.GetFolderPath(appDataFolder), 
                        relativeBaseDir)

let createPath relPath = Path.Combine(baseDirectory, relPath)

let directories = Map.empty.
                     Add(BaseDirectory, baseDirectory).
                     Add(AssetDirectory, createPath "assets" ).
                     Add(LibraryDirectory, createPath "lib").
                     Add(PluginDirectory, createPath "plugins").
                     Add(DataDirectory, createPath "data").
                     Add(DocDirectory, createPath "docs").
                     Add(LogDirectory, createPath "logs")


let checkOrCreateDir dir =
    let failData = createFailureData "checkOrCreateDir" dir
    let fail = failData >> IOFailure
    let failmsg = msg "Exception thrown while attempting to create a directory"
    match Directory.Exists(dir) with
    | true -> IOSuccess "Directory Exists"
    | false -> 
            try 
                do Directory.CreateDirectory(dir) |> ignore
                IOSuccess "Directory Created"
            with
                | :? System.UnauthorizedAccessException     -> failmsg |> UnauthorizedAccessException |> fail
                | :? System.ArgumentNullException           -> failmsg |> ArgumentNullException  |> fail
                | :? System.IO.PathTooLongException         -> failmsg |> PathTooLongException |> fail
                | :? System.ArgumentException               -> failmsg |> ArgumentException |> fail
                | :? System.IO.DirectoryNotFoundException   -> failmsg |> DirectoryNotFoundException |> fail
                | :? System.NotSupportedException           -> failmsg |> NotSupportedException |> fail
                | :? System.IO.IOException                  -> failmsg |> IOException |> fail


let checkOrCreateAllDirectories () = 
    Map.toList directories |> List.collect (fun (k,v) -> [checkOrCreateDir v])
    
let getDirectory key = Map.find key directories

// test routine
//do checkOrCreateAllDirectories () |> List.iter (fun x -> printfn "%A" x)

let filenames = Map.empty.
                    Add(ServerLog, Path.Combine( (getDirectory LogDirectory), "ServerLog.txt")).
                    Add(ClientLog, Path.Combine( (getDirectory LogDirectory), "ClientLog.txt"))


let getFilename key = Map.find key filenames

