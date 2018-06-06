[<EntryPoint>]
let main argv =  
    let metrics = Regression.crossValidateData "data/day.csv"

    printfn "....Cross validation...."
    metrics|> Seq.iter(fun metric -> printfn "RMS: %O" metric.Rms
                                     printfn "RSquared: %O" metric.RSquared 
                                     printfn ".............................")

    0
