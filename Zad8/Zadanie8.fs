module Zadanie4

open System
open Microsoft.SolverFoundation.Services

type TKrawedz = {W1: string; W2: string; CzyKrawedz: int;}

//(x1 + x ¯2 + x3) · (x ¯1 + x3 + x ¯4) · (x1 + x ¯2 + x ¯3) · (x2 + x ¯3 + x4)
let strModel = "Model[
    Decisions[Integers[0, 1], Foreach[{i, 1, 4}, X[i]]],

    Constraints[
        X[1] + (1 - X[2]) + X[3] >= 1,
        (1 - X[1]) + X[3] + (1 - X[4]) >= 1,
        X[1] + (1 - X[2]) + (1 - X[3]) >= 1,
        X[2] + (1 - X[3]) + X[4] >= 1
    ]
]"

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let rozw = context.Solve()
    rozw.GetReport().WriteTo(Console.Out)

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem  // Znalezione rozwiazanie: x1 = x2 = x3 = x4 = 0
    Console.ReadLine() |> ignore
    0
