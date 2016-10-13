module Zadanie5

open System
open Microsoft.SolverFoundation.Services

//Dany jest zbiór Z = fw(i): w(n) = n2 - n + 1, i = 1, 2, . . . , 30g.
//Znajdź minimalną liczbę elementów zbioru Z, które sumują się do
//liczby 4285.

type TLiczba = {Liczba: int;}

let strModel = "Model[
    Parameters[Sets, Liczby],
    Parameters[Integers, doOsiagniecia[]],
    Parameters[Integers, wartosci[Liczby]],

    Decisions[Integers[0, 1], wybrane[Liczby]],

    Constraints[
        Sum[{i, Liczby}, wybrane[i]*(wartosci[i]*wartosci[i] - wartosci[i] + 1)] == doOsiagniecia[]
    ],

    Goals[
        Minimize[LiczbaWybranych -> Sum[{i, Liczby}, wybrane[i]]]
    ]
]"

let doOsiagniecia = [| {Liczba=4285} |]
let iMin = 1
let iMax = 30
let liczby = [| for i in iMin..iMax -> {Liczba=i}|]

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let parametry = context.CurrentModel.Parameters
    for p in parametry do
        match p.Name with
        | "doOsiagniecia" -> p.SetBinding (doOsiagniecia, "Liczba")
        | "wartosci"      -> p.SetBinding (liczby, "Liczba", [| "Liczba" |])
        | x               -> raise(System.ArgumentException("Nieobsługiwany parametr: " + x))
    let rozw = context.Solve()
    rozw.GetReport().WriteTo(Console.Out)

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem
    Console.ReadLine() |> ignore
    // Sprawdzenie
//    [30; 29; 28; 26; 24; 23; 12] |> List.map (fun x -> x*x - x + 1) |> List.sum |> Console.WriteLine
//    Console.ReadLine() |> ignore
    0
