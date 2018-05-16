namespace NaiveBayes

module Classifier = 
    type Token = string

    type Tokenizer = string -> Token Set

    type TokenizedDoc = Token Set

    type DocsGroup = {
        Proportion: float
        TokenFrequencies: Map<Token, float>
    }

    let tokenScore (docsGroup: DocsGroup) (token: Token) =
        if docsGroup.TokenFrequencies.ContainsKey token then
            log docsGroup.TokenFrequencies.[token]
        else
            0.0
    
    let score (document: TokenizedDoc) (docsGroup: DocsGroup) =
        let scoreToken = tokenScore docsGroup
        log docsGroup.Proportion + (document |> Seq.sumBy scoreToken)
    
    let classify (groups: (_*DocsGroup)[]) (tokenizer: Tokenizer) (text: string) = 
        let tokenized = tokenizer text
        groups
        |> Array.maxBy(fun (_,group) -> score tokenized group)
        |> fst