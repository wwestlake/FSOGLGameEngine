#r @"C:\Users\wwestlake\Documents\Visual Studio 2015\Projects\FSOGLGameEngine\LagDaemon.GameEngine.ResourceCompiler\bin\Debug\FParsecCS.dll"
#r @"C:\Users\wwestlake\Documents\Visual Studio 2015\Projects\FSOGLGameEngine\LagDaemon.GameEngine.ResourceCompiler\bin\Debug\FParsec.dll"
#r @"C:\Users\wwestlake\Documents\Visual Studio 2015\Projects\FSOGLGameEngine\LagDaemon.GameEngine.ResourceCompiler\bin\Debug\FParsec-Pipes.dll"

#load "JSONParser.fs"

open FParsec
open LagDaemon.GameEngine.ResourceCompiler.JSON



let example = "{
    \"glossary\": {
        \"title\": \"example glossary\",
		\"GlossDiv\": {
            \"title\": \"S\",
			\"GlossList\": {
                \"GlossEntry\": {
                    \"ID\": \"SGML\",
					\"SortAs\": \"SGML\",
					\"GlossTerm\": \"Standard Generalized Markup Language\",
					\"Acronym\": \"SGML\",
					\"Abbrev\": \"ISO 8879:1986\",
					\"GlossDef\": {
                        \"para\": \"A meta-markup language, used to create markup languages such as DocBook.\",
						\"GlossSeeAlso\": [\"GML\", \"XML\", 5, true, false]
                    },
					\"GlossSee\": \"markup\"
                }
            }
        }
    }
}       "

run json example


