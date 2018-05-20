[<EntryPoint>]
let main argv = 
    let model = Training.trainData "data/train_data.txt"
    let metrics = Training.evaluateData model "data/evaluate_data.txt"
    let customAccuracy = Training.customEvaluator model "data/evaluate_data.txt"
    let falsePositive = Training.getDisambiguation model "data/evaluate_data.txt" "ham"
    let falseNegative = Training.getDisambiguation model "data/evaluate_data.txt" "spam"

    printfn "Micro accuracy is : %f" metrics.AccuracyMicro
    printfn "Macro accuracy is : %f" metrics.AccuracyMacro
    printfn "Custom accuracy is : %f" customAccuracy
    printfn "False positive average is: %f" falsePositive
    printfn "False negative average is: %f" falseNegative
    0