
// val euler : f:('a -> float) -> delta:float -> list:'a list -> float
let euler f delta list =
    let rec innerEuler f delta list accum =
        match list with
        | [] -> accum
        | head :: rest -> innerEuler f delta rest (accum + ((f head) * delta))
    innerEuler f delta list 0.0    

// val fn : x:float -> float
let fn x = 1.0 / (x ** 2.0)

// val it : float = 0.9995001672
euler fn 0.001 [1.0..0.001..1000.0]

