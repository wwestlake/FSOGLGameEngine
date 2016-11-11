#r @"C:\Users\wwestlake\Documents\Visual Studio 2015\Projects\FSOGLGameEngine\LagDaemon.GameEngine.ResourceCompiler\bin\Debug\FParsecCS.dll"
#r @"C:\Users\wwestlake\Documents\Visual Studio 2015\Projects\FSOGLGameEngine\LagDaemon.GameEngine.ResourceCompiler\bin\Debug\FParsec.dll"
#r @"C:\Users\wwestlake\Documents\Visual Studio 2015\Projects\FSOGLGameEngine\LagDaemon.GameEngine.ResourceCompiler\bin\Debug\FParsec-Pipes.dll"


open FParsec

type UserState = unit // doesn't have to be unit, of course
type Parser<'t> = Parser<'t, UserState>


let test p str =
    match run p str with
    | Success(result, _, _)   -> printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

let str s = pstring s
//let floatBetweenBrackets : Parser<_> = str "[" >>. pfloat .>> str "]"


let betweenStrings s1 s2 p = str s1 >>. p .>> str s2

let floatBetweenBrackets: Parser<_> = pfloat |> between (str "[") (str "]")

test floatBetweenBrackets "[1.2]"
test floatBetweenBrackets "[]"
test floatBetweenBrackets "[1.0"

test (skipMany floatBetweenBrackets) "[1.0][2][3][4]"
test (skipMany1 floatBetweenBrackets <?> "float between brackets" ) "[1]"

let ws = spaces

let str_ws s = pstring s .>> ws
let float_ws : Parser<float> = pfloat .>> ws

let numberList = str_ws "[" >>. sepBy float_ws (str_ws ",") .>> str_ws "]"

test numberList @"[ 1.0, 
            2.0 ,     
                    3.0,         3.0]"


let identifier: Parser<_> =
    let isIdentifierFirstChar c = isLetter c || c = '_'
    let isIdentifierChar c = isLetter c || isDigit c || c = '_'

    many1Satisfy2L isIdentifierFirstChar isIdentifierChar "identifier"
        .>> ws // skips trailing whitespace

test identifier "sdfasd_1"

let stringLiteral: Parser<_> =
    let normalChar = satisfy (fun c -> c <> '\\' && c <> '"')
    let unescape c = match c with
                     | 'n' -> '\n'
                     | 'r' -> '\r'
                     | 't' -> '\t'
                     | c   -> c
    let escapedChar = pstring "\\" >>. (anyOf "\\nrt\"" |>> unescape)
    between (pstring "\"") (pstring "\"")
            (manyChars (normalChar <|> escapedChar))

test stringLiteral "\"abc\""

test stringLiteral "\"abc\\\"def\\\\ghi\""
test stringLiteral "\"abc\\def\""


let stringLiteral2: Parser<_>  =
    let normalCharSnippet = many1Satisfy (fun c -> c <> '\\' && c <> '"')
    let escapedChar = pstring "\\" >>. (anyOf "\\nrt\"" |>> function
                                                            | 'n' -> "\n"
                                                            | 'r' -> "\r"
                                                            | 't' -> "\t"
                                                            | c   -> string c)
    between (pstring "\"") (pstring "\"")
            (manyStrings (normalCharSnippet <|> escapedChar))

test stringLiteral2 "\"abc\\\"def\\\\ghi\""
test stringLiteral2 "\"abc\\def\""

let stringLiteral3: Parser<_> =
    let normalCharSnippet = manySatisfy (fun c -> c <> '\\' && c <> '"')
    let escapedChar = pstring "\\" >>. (anyOf "\\nrt\"" |>> function
                                                            | 'n' -> "\n"
                                                            | 'r' -> "\r"
                                                            | 't' -> "\t"
                                                            | c   -> string c)
    between (pstring "\"") (pstring "\"")
            (stringsSepBy normalCharSnippet escapedChar)


test stringLiteral3 "\"abc\\\"def\\\\ghi\""
test stringLiteral3 "\"abc\\def\""

let pipe7 p1 p2 p3 p4 p5 p6 p7 f =
    pipe4 p1 p2 p3 (tuple4 p4 p5 p6 p7)
          (fun x1 x2 x3 (x4, x5, x6, x7) -> f x1 x2 x3 x4 x5 x6 x7)


let boolean: Parser<_> =     (stringReturn "true"  true)
                         <|> (stringReturn "false" false)


test boolean "false"
test boolean "true"
test boolean "tru"

test ( ws >>. (str "a" <|> str "b") ) " b"







