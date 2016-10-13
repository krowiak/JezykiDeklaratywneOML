module Zadanie2

open System
open Microsoft.SolverFoundation.Services

type TKoszt = {Wodki: string; Koszt: float}
type TCzas = {Wodki: string; Czas: float}
type TCenaSprz = {Wodki: string; CenaSprz: float}
type TCzasMaks = {CzasMaks: float}
type TKapital = {Kapital: float}

let strModel = "Model[
    Parameters[Sets, Wodki],
    Parameters[Reals, koszt[Wodki], czas[Wodki], cenaSprz[Wodki]],
    Parameters[Reals, czasMaks[]],
    Parameters[Reals, kapital[]],

    Decisions[Integers[0, Infinity], butelki[Wodki]],

    Constraints[
        Sum[{i, Wodki}, butelki[i]*czas[i]] <= czasMaks[],
        Sum[{i, Wodki}, butelki[i]*koszt[i]] <= kapital[]
    ],

    Goals[
        Maximize[Zysk -> Sum[{i, Wodki}, (cenaSprz[i] - koszt[i])*butelki[i]]]
    ]
]"

let sliwowica = "Sliwowica"
let zytnia = "Zytnia"

let daneKoszt = [|
    {Wodki=sliwowica; Koszt=3.0};
    {Wodki=zytnia; Koszt=2.0}
|]
let daneCzas = [|
    {Wodki=sliwowica; Czas=3.0};
    {Wodki=zytnia; Czas=4.0}
|]
let daneCenaSprz = [|
    {Wodki=sliwowica; CenaSprz=6.0};
    {Wodki=zytnia; CenaSprz=5.40}
|]
let daneCzasMaks = [|{CzasMaks=5000.0}|]
let daneKapital = [|{Kapital=4000.0}|]

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let parametry = context.CurrentModel.Parameters
    for p in parametry do
        match p.Name with
        | "czasMaks" -> p.SetBinding (daneCzasMaks, "CzasMaks")
        | "kapital"  -> p.SetBinding (daneKapital, "Kapital")
        | "koszt"    -> p.SetBinding (daneKoszt, "Koszt", [| "Wodki" |])
        | "cenaSprz" -> p.SetBinding (daneCenaSprz, "CenaSprz", [| "Wodki" |])
        | "czas"     -> p.SetBinding (daneCzas, "Czas", [| "Wodki" |])
        | x          -> raise(System.ArgumentException("Nieobsługiwany parametr: " + x))
    let rozw = context.Solve()
    rozw.GetReport().WriteTo(Console.Out)

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem
    System.Console.ReadLine() |> ignore
    0
