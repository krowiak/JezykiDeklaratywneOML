module Zadanie4

open System
open Microsoft.SolverFoundation.Services

type TKrawedz = {W1: int; W2: int; CzyKrawedz: int;}
type TInt = {Int: int}


let liczbaWierzcholkow = [|{Int=6}|]
let krawedzie = [|
    {W1=0; W2=1; CzyKrawedz=1};
    {W1=0; W2=2; CzyKrawedz=0};
    {W1=0; W2=3; CzyKrawedz=0};
    {W1=0; W2=4; CzyKrawedz=1};
    {W1=0; W2=5; CzyKrawedz=0};
    
    {W1=1; W2=2; CzyKrawedz=1};
    {W1=1; W2=3; CzyKrawedz=0};
    {W1=1; W2=4; CzyKrawedz=1};
    {W1=1; W2=5; CzyKrawedz=0};
    
    {W1=2; W2=3; CzyKrawedz=1};
    {W1=2; W2=4; CzyKrawedz=0};
    {W1=2; W2=5; CzyKrawedz=0};
    
    {W1=3; W2=4; CzyKrawedz=1};
    {W1=3; W2=5; CzyKrawedz=1};

    {W1=4; W2=5; CzyKrawedz=0};
|]


let strModel = "Model[
    Parameters[Sets, Wierzcholki],
    Parameters[Integers[0, 1], krawedz[Wierzcholki, Wierzcholki]],
    Parameters[Integers[0, +Infinity], liczbaWierzcholkow[]],

    Decisions[Integers[0, 1], Foreach[{w, liczbaWierzcholkow[]}, wybrany[w]]],

    Constraints[
        Foreach[{w1, liczbaWierzcholkow[] - 1}, {w2, w1 + 1, liczbaWierzcholkow[]}, wybrany[w1] + wybrany[w2] <= 1 + krawedz[w1, w2]]
    ],

    Goals[
        Maximize[RozmiarKliki -> Sum[{w, liczbaWierzcholkow[]}, wybrany[w]]]
    ]
]"

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let parametry = context.CurrentModel.Parameters
    for p in parametry do
        match p.Name with
        | "krawedz" -> p.SetBinding (krawedzie, "CzyKrawedz", [|"W1"; "W2"|])
        | "liczbaWierzcholkow" -> p.SetBinding (liczbaWierzcholkow, "Int")
        | x -> raise(System.ArgumentException("Nieobsługiwany parametr: " + x))
    let rozw = context.Solve()
    rozw.GetReport().WriteTo(Console.Out)

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem  // Dla przykładowych danych rozmiar 3, wierzchołki 0, 1, 4.
    Console.ReadLine() |> ignore
    0
