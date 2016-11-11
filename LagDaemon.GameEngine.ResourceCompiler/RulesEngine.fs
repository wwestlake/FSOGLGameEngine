namespace LagDaemon.GameEngine.ResourceCompiler.RulesEngine

// Just use a monad
// What's a monad
// A monad is a monoid in the category of endofunctors
// ???
// What's the problem?
// I don't know what and endofunctor is :(
// That's easy.  A functor is a homomorphism between categories, and so an endofunctor is just
// a functor that maps to itself.



module RulesEngine =


    type Result<'T,'F> =
        | Success of 'T
        | Failure of 'F

    let bind func =
        fun input ->
            match input with
            | Success s -> func s
            | Failure f -> Failure f

    let (>>=) input func =
        bind func input

    let map func =
        bind (func >> Success)
        
    
    let tee deadEndFunc oneTrackInput =
        deadEndFunc oneTrackInput
        oneTrackInput
        
    let pass func a =
        map (tee func) a

