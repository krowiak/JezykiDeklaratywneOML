module Zadanie14

open System
open Microsoft.SolverFoundation.Services

let strModel = "Model[
    Decisions[Integers[-Infinity, 500], x, y],

    Constraints[
        x*x + x + 1 == 3 * y * y
    ],

    Goals[
        Minimize[x + y]
    ]
]"
//
//Zad. 14.15 Dla całkowitych x, y > 500 znajdź najmniejszą sumę x + y,
//tak aby było spełnione równanie x2 + x + 1 = 3y2.

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let rozw = context.Solve()
    rozw.GetReport().WriteTo(Console.Out)

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem  // Znalezione rozwiązanie: x = -4367, y = -2521
    Console.ReadLine() |> ignore
    0
