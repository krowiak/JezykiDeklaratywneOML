module Zadanie11

open System
open Microsoft.SolverFoundation.Services

type TKomorka = {X: int; Y: int; Wartosc: int}

//let n = 4
//let wartosci = [|
//      7;  53; 183; 439;
//    627; 343; 773; 959;
//    447; 283; 463;  29;
//    217; 623;   3; 399;
//|]

let n = 15
let wartosci = [|
      7;  53; 183; 439; 863; 497; 383; 563;  79; 973; 287;  63; 343; 169; 583;
    627; 343; 773; 959; 943; 767; 473; 103; 699; 303; 957; 703; 583; 639; 913;
    447; 283; 463;  29;  23; 487; 463; 993; 119; 883; 327; 493; 423; 159; 743;
    217; 623;   3; 399; 853; 407; 103; 983;  89; 463; 290; 516; 212; 462; 350;
    960; 376; 682; 962; 300; 780; 486; 502; 912; 800; 250; 346; 172; 812; 350;
    870; 456; 192; 162; 593; 473; 915;  45; 989; 873; 823; 965; 425; 329; 803;
    973; 965; 905; 919; 133; 673; 665; 235; 509; 613; 673; 815; 165; 992; 326;
    322; 148; 972; 962; 286; 255; 941; 541; 265; 323; 925; 281; 601;  95; 973;
    445; 721;  11; 525; 473;  65; 511; 164; 138; 672;  18; 428; 154; 448; 848;
    414; 456; 310; 312; 798; 104; 566; 520; 302; 248; 694; 976; 430; 392; 198;
    184; 829; 373; 181; 631; 101; 969; 613; 840; 740; 778; 458; 284; 760; 390;
    821; 461; 843; 513;  17; 901; 711; 993; 293; 157; 274;  94; 192; 156; 574;
     34; 124;   4; 878; 450; 476; 712; 914; 838; 669; 875; 299; 823; 329; 699;
    815; 559; 813; 459; 522; 788; 168; 586; 966; 232; 308; 833; 251; 631; 107;
    813; 883; 451; 509; 615;  77; 281; 613; 459; 205; 380; 274; 302;  35; 805;
|]

let tworzMacierz n (wartosci:int[]) =
    let macierz = Array.zeroCreate (n*n)
    for y in 0..n-1 do
        for x in 0..n-1 do
            macierz.[y*n + x] <- {X=x; Y=y; Wartosc = wartosci.[y*n + x]}
    macierz

let strModel = "Model[
    Parameters[Sets, X, Y],
    Parameters[Integers[0, +Infinity], Macierz[X, Y]],

    Decisions[Integers[0, 1], Foreach[{x, X}, {y, Y}, Wybrane[x, y]]],

    Constraints[
        Foreach[{x, X}, Sum[{y, Y}, Wybrane[x, y]] <= 1],
        Foreach[{y, Y}, Sum[{x, X}, Wybrane[x, y]] <= 1]
    ],

    Goals[
        Maximize[Suma -> Sum[{x, X}, {y, Y}, Wybrane[x, y]*Macierz[x, y]]]
    ]
]"

let tworzWybranaWartosc (decyzja:obj[]) =
    let x = decyzja.[1] :?> float |> int
    let y = decyzja.[2] :?> float |> int
    let wartosc = wartosci.[y*n + x]
    (x, y, wartosc)

let rozwiazProblem =
    let context = SolverContext.GetContext()
    context.LoadModel(FileFormat.OML, new IO.StringReader(strModel))
    let parametry = context.CurrentModel.Parameters
    for p in parametry do
        match p.Name with
        | "Macierz" -> p.SetBinding (wartosci |> tworzMacierz n, "Wartosc", [|"X"; "Y"|])
        | x -> raise(System.ArgumentException("Nieobsługiwany parametr: " + x))
    let rozw = context.Solve()
    //rozw.GetReport().WriteTo(Console.Out)  // Zdecydowanie za dużo na raz
    
    let suma = Seq.head rozw.Goals 
    let wybrane = 
        rozw.Decisions 
        |> Seq.map (fun dec -> dec.GetValues()) 
        |> Seq.concat
        |> Seq.where (fun wart -> wart.[0] :?> float = 1.0)
        |> Seq.map tworzWybranaWartosc
    
    Seq.iter (printfn "%A") wybrane
    printfn "Suma docelowa wynosi: %i." <|  suma.ToInt32()
    

[<EntryPoint>]
let main (args : string[]) =
    rozwiazProblem  // Wynik: 13938
    Console.ReadLine() |> ignore
    0