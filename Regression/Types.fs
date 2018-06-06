module Types
    open Microsoft.ML.Runtime.Api;

    type BikeSharing() =
        [<Column(ordinal = "0"); DefaultValue>]
        val mutable Instant: string

        [<Column(ordinal = "1"); DefaultValue>]
        val mutable Dteday: string
        [<Column(ordinal = "2"); DefaultValue>]
        val mutable Season: float32
        [<Column(ordinal = "3"); DefaultValue>]
        val mutable Yr: float32
        [<Column(ordinal = "4"); DefaultValue>]
        val mutable Mnth: float32
        [<Column(ordinal = "5"); DefaultValue>]
        val mutable Holiday: string
        [<Column(ordinal = "6"); DefaultValue>]
        val mutable Weekday: float32
        [<Column(ordinal = "7"); DefaultValue>]
        val mutable Workingday: string
        [<Column(ordinal = "8"); DefaultValue>]
        val mutable Weathersit: string
        [<Column(ordinal = "9"); DefaultValue>]
        val mutable Temp: float32
        [<Column(ordinal = "10"); DefaultValue>]
        val mutable Atemp: float32
        [<Column(ordinal = "11"); DefaultValue>]
        val mutable Hum: float32
        [<Column(ordinal = "12"); DefaultValue>]
        val mutable Windspeed: float32
        [<Column(ordinal = "13"); DefaultValue>]
        val mutable Casual: float32
        [<Column(ordinal = "14"); DefaultValue>]
        val mutable Registered: float32

        [<Column(ordinal = "15"); DefaultValue>]
        val mutable Cnt: float32

    type BikeSharePrediction() = 
        [<ColumnName "Score"; DefaultValue>]
        val mutable Cnt: float32