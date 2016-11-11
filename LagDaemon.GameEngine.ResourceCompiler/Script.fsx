// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r @"C:\Users\wwestlake\Documents\Visual Studio 2015\Projects\FSOGLGameEngine\LagDaemon.GameEngine.ResourceCompiler\bin\Debug\FParsecCS.dll"
#r @"C:\Users\wwestlake\Documents\Visual Studio 2015\Projects\FSOGLGameEngine\LagDaemon.GameEngine.ResourceCompiler\bin\Debug\FParsec.dll"
#r @"C:\Users\wwestlake\Documents\Visual Studio 2015\Projects\FSOGLGameEngine\LagDaemon.GameEngine.ResourceCompiler\bin\Debug\FParsec-Pipes.dll"


#load "FileIO.fs"


open LagDaemon.GameEngine.ResourceCompiler.FileIO

// Define your library scripting code here

let (|Fizz|Buzz|FizzBuzz|Num|) n =
    match n % 3, n % 5 with
    | 0,0 -> FizzBuzz
    | 0,_ -> Fizz
    | _,0 -> Buzz
    | n   -> Num n

for i in 1..100 do
    match i with
    | FizzBuzz   -> printfn "FizzBuzz"
    | Fizz       -> printfn "Fizz"
    | Buzz       -> printfn "Buzz"
    | Num n      -> printfn "%s" (string i)

let (|Fizz|_|) value =
    if value % 3 = 0 then Some () else None

let (|Buzz|_|) value =
    if value % 5 = 0 then Some () else None

for i in 1..100 do
    match i with
    | Fizz & Buzz  -> printfn "FizzBuzz"
    | Fizz         -> printfn "Fizz"
    | Buzz         -> printfn "Buzz"
    | n            -> printfn "%s" (string n)


////////////////////////////////////////////////
// parser
//////////////////////////////////////////////
open FParsec

type Result<'s, 'f> =
    | Success of 's
    | Failure of 'f

type Parser<'a> = Parser of (char list -> Result<'a * char list, string>)

let runParser parser inputChars =
    let (Parser parserFunc) = parser 
    parserFunc inputChars


let expectChar expectedChar =
    let innerParser inputChars =
        match inputChars with
        | c :: remainingChars ->
            if c = expectedChar then Success (c, remainingChars)
            else Failure (sprintf "Expected '%c', got '%c'" expectedChar c)
        | [] ->
            Failure (sprintf "Expected '%c', reached end of input" expectedChar)

    Parser innerParser

let stringToCharList str =
    List.ofSeq str

let orParse parser1 parser2 =
    let innerParser inputChars =
        match runParser parser1 inputChars with
        | Success result -> Success result
        | Failure _ -> runParser parser2 inputChars
    Parser innerParser

let (<|>) = orParse

let choice parserList =
    List.reduce orParse parserList

let anyCharOf validChars =
    validChars
    |> List.map expectChar
    |> choice


let andParse parser1 parser2 =
    let innerParser inputChars =
        match runParser parser1 inputChars with
        | Failure msg -> Failure msg
        | Success (c1, remaining1) ->
            match runParser parser2 remaining1 with
            | Failure msg -> Failure msg
            | Success (c2, remaining2) ->
                Success ((c1,c2), remaining2) 
    Parser innerParser

let ( .>>. ) = andParse

let mapParser mapFunc parser =
    let innerParser inputChars =
        match runParser parser inputChars with
        | Failure msg -> Failure msg
        | Success (result, remaining) ->
            Success (mapFunc result, remaining)

    Parser innerParser

let applyParser funcAsParser paramAsParser =
    (funcAsParser .>>. paramAsParser)
    |> mapParser (fun (f,x) -> f x)


let ( <*> ) = applyParser


let returnAsParser result =
    let innerParser inputChars =
        Success (result, inputChars)

    Parser innerParser

let liftToParser2 funcToLift paramAsParser1 paramAsParser2 =
    returnAsParser funcToLift <*> paramAsParser1 <*> paramAsParser2

let rec sequenceParsers parserList =
    let cons head rest = head :: rest
    let consAsParser = liftToParser2 cons
    match parserList with
    | [] -> returnAsParser []
    | parser :: remainingParsers -> 
        consAsParser parser (sequenceParsers remainingParsers)
    
let charListAsString chars =
    System.String(List.toArray chars)


let expectString expectedString =
    expectedString
    |> stringToCharList
    |> List.map expectChar
    |> sequenceParsers
    |> mapParser charListAsString



stringToCharList "take"
|> runParser (expectString "lake" <|> expectString "take")
|> printfn "%A"

let divide ifzero ifsuccess top bottom =
    if (bottom = 0) 
    then ifzero ["Divide by zero error"]
    else ifsuccess (top/bottom)

let ifzero = (fun x -> Failure x)
let ifsuccess = (fun x -> Success x)

let optionDivide = divide ifzero ifsuccess

let good = optionDivide 6 3
let bad = optionDivide 6 0

let add42 x = x + 42
1 |> add42

Some 1 |> Option.map add42


type TraceBuilder() =
    member this.Bind(m, f) = 
        match m with 
        | None -> 
            printfn "Binding with None. Exiting."
        | Some a -> 
            printfn "Binding with Some(%A). Continuing" a
        Option.bind f m

    member this.Return(x) = 
        printfn "Return an unwrapped %A as an option" x
        Some x

    member this.Zero() = 
        printfn "Zero"
        None

    member this.Combine (a,b) = 
        printfn "Returning early with %A. Ignoring second part: %A" a b 
        a

    member this.Delay(funcToDelay) = 
        let delayed = fun () ->
            printfn "%A - Starting Delayed Fn." funcToDelay
            let delayedResult = funcToDelay()
            printfn "%A - Finished Delayed Fn. Result is %A" funcToDelay delayedResult
            delayedResult  // return the result 

        printfn "%A - Delaying using %A" funcToDelay delayed
        delayed // return the new function

    member this.Run(funcToRun) = 
        printfn "%A - Run Start." funcToRun
        let runResult = funcToRun()
        printfn "%A - Run End. Result is %A" funcToRun runResult
        runResult // return the result of running the delayed function

// make an instance of the workflow                
let trace = new TraceBuilder()


trace { 
    printfn "Part 1: about to return 1"
    return 1
    printfn "Part 2: after return has happened"
    } |> printfn "Result for Part1 without Part2: %A"  


