namespace LagDaemon.GameEngine.ResourceCompiler.JSON

module JSONParser =

    open FParsec
    open LagDaemon.GameEngine.ResourceCompiler.FileIO

    /// UserState is used to force the type of certain parsers where
    /// a value restriction compiler error occurs
    type UserState = unit 
    type Parser<'t> = Parser<'t, UserState>
    
    /// redifines pstring as str
    let str s = pstring s
    
    /// white space parser
    let ws = spaces
    
    /// This DU represents the various Json values
    type Json = 
        | JString of string
        | JNumber of float
        | JBool   of bool
        | JNull
        | JList   of Json list
        | JObject of Map<string, Json>
    
    /// parser forwading
    let jvalue, jvalueRef = createParserForwardedToRef<Json, unit>()
    
    /// parser returns a JNull for the keyword "null"
    let jnull = stringReturn "null" JNull
    
    /// parses bool literals true and false
    let jbool =    (stringReturn "true" (JBool true))
                          <|> (stringReturn "false" (JBool false))
    
    /// parses numeric literals
    let jnumber = pfloat |>> JNumber
    
    /// parses string literals with escape sequences
    let stringLiteral =
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
    
    let jstring =
            stringLiteral |>> (fun x -> JString x)
    
    /// helper parser for constructing lists
    let listBetweenStrings sOpen sClose pElement f =
        between (str sOpen) (str sClose)
                (ws >>. sepBy (pElement .>> ws) (str "," >>. ws) |>> f)
    
    
    /// parser for jlist items
    let jlist = listBetweenStrings "[" "]" jvalue JList
    
    /// parser for key value pairs
    let keyValue = stringLiteral .>>. (ws >>. str ":" >>. ws >>. jvalue)
    
    /// parser for JObjects
    let jobject = listBetweenStrings "{" "}" keyValue (Map.ofList >> JObject)
    
    /// parser for Json values
    do jvalueRef := choice [jobject
                            jlist
                            jstring
                            jnumber
                            jbool
                            jnull]
    
    let rec interpret (data: Json) =
        match data with
        | JString value      -> printfn "%A" value
        | JNumber value      -> printfn "%A" value
        | JBool   value      -> printfn "%A" value
        | JNull              -> printfn "%A" "NULL"
        | JList []           -> ()
        | JList (hd :: tail) -> printfn "%A" hd ; interpret hd
        | JObject map         -> printfn "object: " ; for n in map  do 
                                                          printf "%s: " n.Key
                                                          interpret n.Value

    
    let json = ws >>. jvalue .>> ws .>> eof
    
    let loadJsonFile filepath = 
        match filepath |> readTextFileAsString |> run json with
        | Success (result, _, _) -> interpret result 
        | Failure (msg, _, _) -> failwith msg




