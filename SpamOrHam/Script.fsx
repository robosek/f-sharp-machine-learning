open System.IO

type DocType = 
    | Spam
    | Ham

let identify (example: DocType*string) = 
    let docType,content = example
    match docType with
    | Spam -> printfn "%s is spam" content
    | Ham -> printfn "%s is ham" content

let parseDocType (label: string) = 
    match label with
    | "spam" -> Spam
    | "ham" -> Ham
    | _ -> failwith "Unknown label"

let parseLine (line: string) = 
    let words = line.Split('\t')
    printfn "%s" words.[0]
    let label = words.[0] |>  parseDocType
    let content = words.[1]
    (label, content)

let dataFileName = "data.txt"
let filePath = __SOURCE_DIRECTORY__ + "/data/" + dataFileName

let readFile filePath = 
    File.ReadAllLines filePath
    |> Array.map parseLine
