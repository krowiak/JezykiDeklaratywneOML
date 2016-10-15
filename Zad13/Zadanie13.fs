module Zadanie13

open System
open Microsoft.SolverFoundation.Services

type TInt = {Int: int}

let liczbaWierzcholkow = [|{Int=6}|]
let liczby = [|
    {Int=1}; {Int=2}; {Int=3};
    {Int=6}; {Int=7}; {Int=5}
|]

let strModel = "Model[
    Parameters[Sets, Liczby],
    Parameters[Integers[0, +Infinity], wartLiczb[Liczby]],

    Decisions[Integers[0, 1], Foreach[{liczba, Liczby}, Zbior1[liczba]]],
    Decisions[Integers[0, 1], Foreach[{liczba, Liczby}, Zbior2[liczba]]],

    Constraints[
        Foreach[{liczba, Liczby}, Zbior1[liczba] + Zbior2[liczba] == 1],
        Sum[{liczba, Liczby}, Zbior1[liczba]*wartLiczb[liczba]] == Sum[{liczba, Liczby}, Zbior2[liczba]*wartLiczb[liczba]]
    ]
]"

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let parametry = context.CurrentModel.Parameters
    for p in parametry do
        match p.Name with
        | "wartLiczb" -> p.SetBinding (liczby, "Int", [|"Int"|])
        | x -> raise(System.ArgumentException("Nieobsługiwany parametr: " + x))
    let rozw = context.Solve()
    rozw.GetReport().WriteTo(Console.Out)
    if rozw.Quality = SolverQuality.Infeasible
    then printfn "Rozwiązanie niemożliwe."
    else printfn "Rozwiązanie istnieje."

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem
    Console.ReadLine() |> ignore
    0

