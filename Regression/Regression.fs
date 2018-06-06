module Regression 
    open Microsoft.ML
    open Microsoft.ML.Trainers
    open Microsoft.ML.Transforms
    open Microsoft.ML.Models
    open System
    open Microsoft.ML.Data
    open Types
    let private preparePipeline dataPath = 
        let pipeline = new LearningPipeline()
        let text = new TextLoader(dataPath)
        let textLoader = text.CreateFrom<BikeSharing>(true, ',')
        let tuple = ValueTuple.Create("Cnt", "Label")
        let columnCopier = new ColumnCopier(tuple)
        let vectorizer = new CategoricalOneHotVectorizer("Instant","Yr","Holiday","Workingday","Weathersit")
        let concatenator = new ColumnConcatenator("Features","Instant","Season"
                                                  ,"Yr","Mnth","Holiday","Weekday",
                                                  "Workingday","Weathersit","Temp",
                                                  "Atemp","Hum","Windspeed")
        let regressor = new FastTreeRegressor()

        pipeline.Add textLoader
        pipeline.Add columnCopier
        pipeline.Add vectorizer
        pipeline.Add concatenator
        pipeline.Add regressor
        pipeline

    let trainData dataPath =
        let pipeline = preparePipeline dataPath
        pipeline.Train<BikeSharing, BikeSharePrediction>()

    let evaluateData model dataPath =
        let text = new TextLoader(dataPath)
        let textLoader = text.CreateFrom<BikeSharing>(true, ',')
        let evaluator = new RegressionEvaluator()
        evaluator.Evaluate(model, textLoader)

    let crossValidateData dataPath = 
        let pipeline = preparePipeline dataPath
        let crossValidator = new CrossValidator()
        crossValidator.Kind <- MacroUtilsTrainerKinds.SignatureRegressorTrainer
        let validator = crossValidator.CrossValidate<BikeSharing, BikeSharePrediction>(pipeline)
        let metrics = validator.RegressionMetrics
        metrics
 