open System.IO
open System

type Observation = { Label:string; Pixels:int[] }
type Distance = int[] -> int[] -> int

let toObservation (csvData:string) =
    let row = csvData.Split(',')
    let label = row.[0]
    let pixels = row.[1..] |> Array.map Convert.ToInt32
    { Label=label; Pixels=pixels }

let reader path =
    let lines = File.ReadAllLines path
    lines.[1..]
    |> Array.map toObservation

let trainingSamplePath = __SOURCE_DIRECTORY__ + @"\Data\trainingsample.csv"
let trainingData = reader trainingSamplePath

let manhattanDistane pixels1 pixels2 = 
    Array.sumBy (fun (x,y) -> abs x-y) (Array.zip pixels1 pixels2)

let train (observations: Observation[]) (distance: Distance) = 
    let classify (pixels:int[]) = 
        observations
        |> Array.minBy (fun x -> distance x.Pixels pixels)
        |> fun x -> x.Label
    classify

let classifier = train trainingData manhattanDistane

let validationSamplePath = __SOURCE_DIRECTORY__ + @"\Data\validationsample.csv"
let validationData = reader validationSamplePath
printfn "%d" validationData.Length

let evaluate validationData classifier =
    validationData
    |> Array.averageBy (fun x -> if classifier x.Pixels = x.Label then 1. else 0.)
    |> printfn "Correct: %.3f"

printfn "Manhattan"
evaluate validationData classifier