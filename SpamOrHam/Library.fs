open System
open Training

[<EntryPoint>]
let main argv = 
    let predict = Training.trainAndPredictData "data/data.txt"
    let result = predict.PredictedLabel
    printfn "Predicted label for test sentence is: %s" result 
    0