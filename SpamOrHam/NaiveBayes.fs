module Training 
    open Microsoft.ML
    open Microsoft.ML.Runtime.Api;
    open Microsoft.ML.Trainers
    open Microsoft.ML.Transforms
    open Microsoft.ML.Models
    open System.IO

    type SpamOrHam() =
        [<Column(ordinal = "0", name="Label"); DefaultValue>]
        val mutable Label: string 
        [<Column(ordinal = "1"); DefaultValue>]
        val mutable Content: string

    type CategoryPrediction() = 
        [<ColumnName "PredictedLabel"; DefaultValue>]
        val mutable PredictedLabel: string

    let trainData dataFileName = 
        let learningPipeline = new LearningPipeline()
        let textLoader = new TextLoader<SpamOrHam>(dataFileName, false, "tab")
        let dictionizer = new Dictionarizer("Label")
        let textFeaturizer = new TextFeaturizer("Features", "Content")
        let predictLabelConverter = new PredictedLabelColumnOriginalValueConverter()
        predictLabelConverter.PredictedLabelColumn <- "PredictedLabel"
        let classifier = new NaiveBayesClassifier()

        learningPipeline.Add textLoader
        learningPipeline.Add dictionizer
        learningPipeline.Add textFeaturizer
        learningPipeline.Add classifier
        learningPipeline.Add predictLabelConverter

        learningPipeline.Train<SpamOrHam, CategoryPrediction>()
    
    let evaluateData model testDataPath =
        let textLoader = new TextLoader<SpamOrHam>(testDataPath, false, "tab")
        let evaluator = new ClassificationEvaluator()
        evaluator.Evaluate(model, textLoader)
    
    let private mapToSpamOrHam(data: string[]) =
        let spamOrHam = new SpamOrHam()
        spamOrHam.Label <- data.[0]
        spamOrHam.Content <- data.[1]
        spamOrHam
        
    let customEvaluator (model: PredictionModel<SpamOrHam, CategoryPrediction>) testDataPath =
         let testData = File.ReadAllLines testDataPath
                           |> Seq.map ((fun sentence -> sentence.Split '\t') >> mapToSpamOrHam)
         let predictions = model.Predict testData

         testData
         |> Seq.zip predictions
         |> Seq.map(fun (prediction, spamOrHam) -> prediction.PredictedLabel = spamOrHam.Label)
         |> Seq.filter(fun validPrediction -> validPrediction = true)
         |> Seq.length
         |> fun number -> (float32(number)/1000.0f)