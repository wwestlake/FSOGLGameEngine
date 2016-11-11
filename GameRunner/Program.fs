// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System
open FileResources
open DAL


[<EntryPoint>]
let main argv = 
    printfn "LagDaemon Game Engine 0.0.1"
    printfn "Starting game engine"

    do serverAdminLog "GameEngine" "Starting Server"

    do Console.ReadKey() |> ignore

    0 // return an integer exit code
