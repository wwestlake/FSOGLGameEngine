namespace LagDaemon.GameEngine.ResourceCompiler

module FileIO =

    open System
    open System.IO


    /// Read the specified file at filepath and return it as a sequence of lines
    let readTextFile (filepath: string) =
        seq {
            use sr = new StreamReader(filepath)
            while not sr.EndOfStream do
                yield sr.ReadLine()
        }

    let readTextFileAsString filepath = 
         filepath |> readTextFile |> Seq.toList |> List.reduce (fun a b -> a + Environment.NewLine + b)

    
