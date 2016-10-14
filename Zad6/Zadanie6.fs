module Zadanie6

open System
open Microsoft.SolverFoundation.Services

type TLiczba = {Liczba: int;}

let strModel = "Model[
    Parameters[Sets, Cyfry],
    Parameters[Integers, wartosci[Cyfry]],

    Decisions[Integers[0, 1], wybrane[Cyfry, Cyfry]],

    Constraints[
        Foreach[{p, Cyfry}, Sum[{c, Cyfry}, wybrane[p, c] == 1]],
        Foreach[{p, Cyfry}, Sum[{c, Cyfry}, wartosci[c]*wybrane[p, c]] == Sum[{c, Cyfry}, wybrane[c, p]]]
    ]
]"

let wartosci = [| for i in 0..9 -> {Liczba=i}|]

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let parametry = context.CurrentModel.Parameters
    for p in parametry do
        match p.Name with
        | "wartosci"      -> p.SetBinding (wartosci, "Liczba", [| "Liczba" |])
        | x               -> raise(System.ArgumentException("Nieobsługiwany parametr: " + x))
    let rozw = context.Solve()
    rozw.GetReport().WriteTo(Console.Out)

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem  // Znalezione rozwiązanie: 6210001000
    Console.ReadLine() |> ignore
    0
