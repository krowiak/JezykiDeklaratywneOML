module Zadanie7

open System
open System.Linq
open Microsoft.SolverFoundation.Services

type TKrawedz = {W1: int; W2: int; CzyKrawedz: int;}
type TInt = {Int: int}

let liczbaWierzcholkow = [|{Int=6}|]
let krawedzie = [|
    {W1=0; W2=1; CzyKrawedz=1};
    {W1=0; W2=2; CzyKrawedz=1};
    {W1=0; W2=3; CzyKrawedz=0};
    {W1=0; W2=4; CzyKrawedz=1};
    {W1=0; W2=5; CzyKrawedz=1};
    
    {W1=1; W2=2; CzyKrawedz=1};
    {W1=1; W2=3; CzyKrawedz=1};
    {W1=1; W2=4; CzyKrawedz=0};
    {W1=1; W2=5; CzyKrawedz=1};
    
    {W1=2; W2=3; CzyKrawedz=1};
    {W1=2; W2=4; CzyKrawedz=1};
    {W1=2; W2=5; CzyKrawedz=0};
    
    {W1=3; W2=4; CzyKrawedz=1};
    {W1=3; W2=5; CzyKrawedz=1};

    {W1=4; W2=5; CzyKrawedz=1};
|]

let strModel = "Model[
    Parameters[Sets, Wierzcholki],
    Parameters[Integers[0, 1], krawedz[Wierzcholki, Wierzcholki]],
    Parameters[Integers[1, +Infinity], lWierz[]],

    Decisions[Integers[0, 1], Foreach[{wierzcholek, lWierz[]}, {kolor, lWierz[]}, kolory[wierzcholek, kolor]]],

    Constraints[
        Foreach[{wierzcholek, lWierz[]}, Sum[{kolor, lWierz[]}, kolory[wierzcholek, kolor]] == 1],
        Foreach[
            {w1, lWierz[] - 1}, {w2, w1 + 1, lWierz[]}, {kolor, lWierz[]},
            (kolory[w1, kolor] + kolory[w2, kolor]) * krawedz[w1, w2] <= 1
        ]
    ],

    Goals[
        Minimize[SumaWartosciEtykietKolorow -> Sum[{wierzcholek, lWierz[]}, {kolor, lWierz[]}, kolor*kolory[wierzcholek, kolor]]]
    ]
]"

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let parametry = context.CurrentModel.Parameters
    for p in parametry do
        match p.Name with
        | "krawedz" -> p.SetBinding (krawedzie, "CzyKrawedz", [|"W1"; "W2"|])
        | "lWierz" -> p.SetBinding (liczbaWierzcholkow, "Int")
        | x -> raise(System.ArgumentException("Nieobsługiwany parametr: " + x))
    let rozw = context.Solve()

    // Rozwiązanie ustawia odpowiednio kolory na grafie, ale zwraca tylko sumę "wartości" kolorów
    // Zawsze będzie ona sumą n pierwszych liczb naturalnych > 0, n = liczba chromatyczna
    let sumaZWyniku = rozw.Goals.First().ToInt32()
    let mutable sumaKolorow = 0
    let mutable liczbaKolorow = 0
    while (liczbaKolorow < liczbaWierzcholkow.[0].Int - 1) && (sumaKolorow < sumaZWyniku) do
        liczbaKolorow <- liczbaKolorow + 1
        sumaKolorow <- sumaKolorow + liczbaKolorow

    rozw.GetReport().WriteTo(Console.Out)
    printfn "Liczba chromatyczna grafu to %i." liczbaKolorow

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem  // Dla przykładowych danych liczba chromatyczna to 3
    Console.ReadLine() |> ignore
    0

