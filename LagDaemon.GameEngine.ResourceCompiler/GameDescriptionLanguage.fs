module GameDescriptionLanguage

open FParsec

/// UserState is used to force the type of certain parsers where
/// a value restriction compiler error occurs (temporary, these types are removed once 
/// all of the functions are created and the type inference system locks in the types
type UserState = unit 
type Parser<'t> = Parser<'t, UserState>

/// redifines pstring as str
let str s = pstring s

/// white space parser
let ws = spaces


/// This DU represents the various Json values
type GDL = 
    | DString   of string
    | DNumber   of float
    | DBool     of bool
    | DNull
    | DVertex   of float * float * float
    | DUVCoord  of float * float
    | DList     of GDL list
    | DObject   of Map<string, GDL>

/// parser forwading
let dvalue, dvalueRef = createParserForwardedToRef<GDL, unit>()

/// parser returns a DNull for the keyword "null"
let dnull: Parser<_> = stringReturn "null" DNull

/// parses bool literals true and false
let dbool: Parser<_> =    (stringReturn "true" (DBool true))
                      <|> (stringReturn "false" (DBool false))

/// parses numeric literals
let dnumber: Parser<_> = pfloat |>> DNumber

/// parses string literals with escape sequences
let stringLiteral: Parser<_> =
    let escape = anyOf "\"\\/bfmrt"
                 |>> function
                    | 'b' -> "\b"
                    | 'f' -> "\u000C"
                    | 'n' -> "\n"
                    | 'r' -> "\r"
                    | 't' -> "\t"
                    | c   -> string c

    let unicodeEscape =
        let hex2int c = (int c &&& 15) + (int c >>> 6) * 9

        str "u" >>. pipe4 hex hex hex hex (fun h3 h2 h1 h0 ->
            (hex2int h3) * 4096 + (hex2int h2) * 256 + (hex2int h1) * 16 + hex2int h0
            |> char |> string
        )

    let escapedCharSnippet = str "\\" >>. (escape <|> unicodeEscape)
    let normalCharSnippet = manySatisfy (fun c -> c <> '"' && c <> '\\')

    between (str "\"") (str "\"")
            (stringsSepBy normalCharSnippet escapedCharSnippet)

let dstring =
        stringLiteral |>> (fun x -> DString x)

/// helper parser for constructing lists
let listBetweenStrings sOpen sClose pElement f =
    between (str sOpen) (str sClose)
            (ws >>. sepBy (pElement .>> ws) (str ";" >>. ws) |>> f)

/// parser for jlist items
let dlist = listBetweenStrings "[" "]" dvalue DList







