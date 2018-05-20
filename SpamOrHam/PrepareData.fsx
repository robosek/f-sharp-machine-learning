open System.IO

let dataPath = __SOURCE_DIRECTORY__ + @"/data/data.txt"
let data = File.ReadAllLines(dataPath)

let countTokensWithLabel label =
    data 
    |> Seq.map ((fun sentence -> sentence.Split '\t') >> (fun array -> array.[0]))
    |> Seq.filter(fun sentenceLabel -> sentenceLabel = label)
    |> Seq.length

let searchLabel = "ham"
let result = countTokensWithLabel searchLabel
printfn "There are %d %s labels" result searchLabel

let maxHam = 850
let maxSpam = 150
let prepareLabelData maxLabelNumber label = 

    data
    |> Seq.filter(fun sentenceLabel -> sentenceLabel.StartsWith label)
    |> Seq.chunkBySize maxLabelNumber
    |> Seq.head

let prepareTestData = 
    prepareLabelData maxHam "ham"
    |> Seq.append (prepareLabelData maxSpam "spam")
    |> Array.ofSeq

let prepareEvaluateData testData = 
    data |> Seq.except testData
    |> Array.ofSeq

let saveFile fileName data = 
    File.WriteAllLines(fileName, data)

let testData = prepareTestData
let evaluateData = prepareEvaluateData testData

let evaluateDataPath = __SOURCE_DIRECTORY__ + @"/data/evaluate_data.txt"
let trainDataPath = __SOURCE_DIRECTORY__ + @"/data/train_data.txt"

saveFile evaluateDataPath testData
saveFile trainDataPath evaluateData