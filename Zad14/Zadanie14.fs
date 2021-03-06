﻿module Zadanie14

open System
open Microsoft.SolverFoundation.Services

let strModel = "Model[
    Decisions[Integers[500, Infinity], x, y],

    Constraints[
        x*x + x + 1 == 3 * y * y
    ],

    Goals[
        Minimize[x + y]
    ]
]"

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let rozw = context.Solve()
    rozw.GetReport().WriteTo(Console.Out)

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem  // Znalezione rozwiązanie: x = 4366, y = 2521
    Console.ReadLine() |> ignore
    0
