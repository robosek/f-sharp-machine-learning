(* #r @"bin/Debug/net461/win-x64/Microsoft.ML.dll"
#r @"bin/Debug/net461/win-x64/Microsoft.ML.Api.dll"
#r @"bin/Debug/net461/win-x64/Microsoft.ML.Core.dll"
#r @"bin/Debug/net461/win-x64/Microsoft.ML.Data.dll"
#r @"bin/Debug/net461/win-x64/Microsoft.ML.CpuMath.dll"
#r @"bin/Debug/net461/win-x64/netstandard.dll"
#r @"bin/Debug/net461/win-x64/CpuMathNative.dll"
 *)

module Training 
    open Microsoft.ML
    open Microsoft.ML.Runtime.Api;
    open Microsoft.ML.Trainers;
    open Microsoft.ML.Transforms;
    open Microsoft.ML.Runtime.Data

    type SpamOrHam() =
        [<Column(ordinal = "0", name="Label"); DefaultValue>]
        val mutable Label: string 
        [<Column(ordinal = "1"); DefaultValue>]
        val mutable Content: string

    type CategoryPrediction() = 
        [<ColumnName "PredictedLabel"; DefaultValue>]
        val mutable PredictedLabel: string

    let trainAndPredictData dataFileName = 
        let learningPipeline = new LearningPipeline()
        let textLoader = new TextLoader<SpamOrHam>(dataFileName, false, "tab")
        let dictionizer = new Dictionarizer("Label")
        let textFeaturizer = new TextFeaturizer("Features", "Content")
        let predictLabelConverter = new PredictedLabelColumnOriginalValueConverter()
        predictLabelConverter.PredictedLabelColumn <- "PredictedLabel"
        let classifier = new FastTreeBinaryClassifier()
        classifier.NumLeaves <- 5
        classifier.NumTrees <- 5
        classifier.MinDocumentsInLeafs <- 2

        learningPipeline.Add(textLoader) |> ignore
        learningPipeline.Add dictionizer
        learningPipeline.Add(textFeaturizer) |> ignore
        learningPipeline.Add(classifier) |> ignore
        learningPipeline.Add(predictLabelConverter) |> ignore

        let model = learningPipeline.Train<SpamOrHam, CategoryPrediction>()        
        let test = new SpamOrHam()
        test.Content <- "REMINDER FROM O2: To get 2.50 pounds free call credit and details of great offers pls reply 2 this text with your valid name, house no and postcode"
        model.Predict(test)