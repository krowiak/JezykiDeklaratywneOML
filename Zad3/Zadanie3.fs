module Zadanie4

open System
open Microsoft.SolverFoundation.Services

type TKrawedz = {W1: string; W2: string; CzyKrawedz: int;}

//  1 2 3 4 5
//a + + - - -
//b - - + + +
//c + - + - +
//d - + - + -
//e - - - - +
//f + - + - -
let krawedzie = [|
    {W1="1"; W2="a"; CzyKrawedz=1};
    {W1="1"; W2="b"; CzyKrawedz=0};
    {W1="1"; W2="c"; CzyKrawedz=1};
    {W1="1"; W2="d"; CzyKrawedz=0};
    {W1="1"; W2="e"; CzyKrawedz=0};
    {W1="1"; W2="f"; CzyKrawedz=1};
    
    {W1="2"; W2="a"; CzyKrawedz=1};
    {W1="2"; W2="b"; CzyKrawedz=0};
    {W1="2"; W2="c"; CzyKrawedz=0};
    {W1="2"; W2="d"; CzyKrawedz=1};
    {W1="2"; W2="e"; CzyKrawedz=0};
    {W1="2"; W2="f"; CzyKrawedz=0};
    
    {W1="3"; W2="a"; CzyKrawedz=0};
    {W1="3"; W2="b"; CzyKrawedz=1};
    {W1="3"; W2="c"; CzyKrawedz=1};
    {W1="3"; W2="d"; CzyKrawedz=0};
    {W1="3"; W2="e"; CzyKrawedz=0};
    {W1="3"; W2="f"; CzyKrawedz=1};
    
    {W1="4"; W2="a"; CzyKrawedz=0};
    {W1="4"; W2="b"; CzyKrawedz=1};
    {W1="4"; W2="c"; CzyKrawedz=0};
    {W1="4"; W2="d"; CzyKrawedz=1};
    {W1="4"; W2="e"; CzyKrawedz=0};
    {W1="4"; W2="f"; CzyKrawedz=0};
    
    {W1="5"; W2="a"; CzyKrawedz=0};
    {W1="5"; W2="b"; CzyKrawedz=1};
    {W1="5"; W2="c"; CzyKrawedz=1};
    {W1="5"; W2="d"; CzyKrawedz=0};
    {W1="5"; W2="e"; CzyKrawedz=1};
    {W1="5"; W2="f"; CzyKrawedz=0};
|]

let strModel = "Model[
    Parameters[Sets, X, Y],
    Parameters[Integers[0, 1], jestKrawedz[X, Y]],

    Decisions[Integers[0, 1], Foreach[{x, X}, {y, Y}, wybranoKrawedz[x, y]]],

    Constraints[
        Foreach[{x, X}, Sum[{y, Y}, wybranoKrawedz[x, y]] <=1],
        Foreach[{y, Y}, Sum[{x, X}, wybranoKrawedz[x, y]] <=1],
        Foreach[{x, X}, {y, Y}, wybranoKrawedz[x, y] -: jestKrawedz[x, y]]
    ],

    Goals[
        Maximize[LiczbaKrawedzi -> Sum[{x, X}, {y, Y}, wybranoKrawedz[x, y]]]
    ]
]"

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let parametry = context.CurrentModel.Parameters
    for p in parametry do
        match p.Name with
        | "jestKrawedz" -> p.SetBinding (krawedzie, "CzyKrawedz", [|"W1"; "W2"|])
        | x -> raise(System.ArgumentException("Nieobsługiwany parametr: " + x))
    let rozw = context.Solve()
    rozw.GetReport().WriteTo(Console.Out)

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem  // Znalezione rozwiazanie dla przykładowych danych: (1; f),(2; d),(3; c),(4; b),(5; e)
    Console.ReadLine() |> ignore
    0
