module Zadanie9

open System
open Microsoft.SolverFoundation.Services

type TM2 = {Blok: string; M2: int;}
type TM3 = {Blok: string; M3: int;}
type TKoszt = {Blok: string; Koszt: float;}
type TInt = {Int: int}

let MinM2 = [|{Int=350}|]
let MaxM3 = [|{Int=150}|]
let MaxBlokow = [|{Int=6}|]
let P15 = "15-piętrowy"
let P10 = "10-piętrowy"

let LiczbyM2 = [|
    {Blok=P15; M2=50};
    {Blok=P10; M2=100}
|]

let LiczbyM3 = [|
    {Blok=P15; M3=75};
    {Blok=P10; M3=0}
|]

let Koszta = [|
    {Blok=P15; Koszt=4000000.0};
    {Blok=P10; Koszt=3000000.0}
|]

let strModel = "Model[
    Parameters[Sets, Bloki],
    Parameters[Integers[0, +Infinity], m2[Bloki], m3[Bloki]],
    Parameters[Reals[0, +Infinity], koszt[Bloki]],
    Parameters[Integers[0, +Infinity], minM2[], maxM3[], max[]],

    Decisions[Integers[0, +Infinity], liczba[Bloki]],

    Constraints[
        Sum[{rodzaj, Bloki}, liczba[rodzaj]] <= max[],
        Sum[{rodzaj, Bloki}, liczba[rodzaj]*m2[rodzaj]] >= minM2[],
        Sum[{rodzaj, Bloki}, liczba[rodzaj]*m3[rodzaj]] <= maxM3[]
    ],

    Goals[
        Minimize[MinimalnyKoszt -> Sum[{rodzaj, Bloki}, koszt[rodzaj]*liczba[rodzaj]]]
    ]
]"

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let parametry = context.CurrentModel.Parameters
    for p in parametry do
        match p.Name with
        | "m2"      -> p.SetBinding (LiczbyM2, "M2", [|"Blok"|])
        | "m3"      -> p.SetBinding (LiczbyM3, "M3", [|"Blok"|])
        | "koszt"   -> p.SetBinding (Koszta, "Koszt", [|"Blok"|])
        | "minM2"   -> p.SetBinding (MinM2, "Int")
        | "maxM3"   -> p.SetBinding (MaxM3, "Int")
        | "max"     -> p.SetBinding (MaxBlokow, "Int")
        | x         -> raise(System.ArgumentException("Nieobsługiwany parametr: " + x))
    let rozw = context.Solve()
    rozw.GetReport().WriteTo(Console.Out)

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem  // Rozwiązanie: 4x blok 10-piętrowy
    Console.ReadLine() |> ignore
    0
