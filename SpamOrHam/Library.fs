[<EntryPoint>]
let main argv = 
    let model = Training.trainData "data/train_data.txt"
    let metrics = Training.evaluateData model "data/evaluate_data.txt"
    let customAccuracy = Training.customEvaluator model "data/evaluate_data.txt"
    
    printfn "Micro accuracy is : %f" metrics.AccuracyMicro
    printfn "Macro accuracy is : %f" metrics.AccuracyMacro
    printfn "Custom accuracy is : %f" customAccuracy
    0