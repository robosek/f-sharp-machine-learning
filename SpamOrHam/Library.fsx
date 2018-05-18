#r @"bin/x64/Debug/net461/Microsoft.ML.dll"
#r @"bin/x64/Debug/net461/Microsoft.ML.Api.dll"
#r @"bin/x64/Debug/net461/Microsoft.ML.Core.dll"
#r @"bin/x64/Debug/net461/Microsoft.ML.Data.dll"
#r @"bin/x64/Debug/net461/Microsoft.ML.CpuMath.dll"
#r @"bin/x64/Debug/net461/netstandard.dll"


open Microsoft.ML
open Microsoft.ML.Runtime.Api;
open Microsoft.ML.Trainers;
open Microsoft.ML.Transforms;

type SpamOrHam() = 
    [<Column(ordinal = "0", name = "Label"); DefaultValue>]
    val mutable Label: float32
    [<Column(ordinal = "1"); DefaultValue>]
    val mutable Content: string


type CategoryPrediction() = 
    [<ColumnName "PredictedLabel"; DefaultValue>]
    val mutable Label: float32


module Training = 

    let dataFileName = "data.txt"
    let filePath = __SOURCE_DIRECTORY__ + "\\data\\" + dataFileName

    let trainAndPredictData = 
        let learningPipeline = new LearningPipeline()
        let textLoader = new TextLoader<SpamOrHam>(filePath, false, "tab")
        let textFeaturizer = new TextFeaturizer("Features", "Content")
        let classifier = new NaiveBayesClassifier()

        learningPipeline.Add(textLoader) |> ignore
        learningPipeline.Add(textFeaturizer) |> ignore
        learningPipeline.Add(classifier) |> ignore

        let model = learningPipeline.Train<SpamOrHam, CategoryPrediction>()
        ""
        
        let test = new SpamOrHam()
        model.Predict(test)