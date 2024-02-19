public record RecordProductRequestDTO(string Code, string Name, string Description, int CategoryId, List<string> Tags);

///NOTE
///Para não precisarmos passar o model inteiro, podemos criar um DTO para receber apenas o que precisamos
///Exemplo preciso criar um endpoint só com 'Name' e 'Description' do produto, eu faria o DTO só com esses dois atributos
///Isso é bom para enxugar o retorno que vamos mandar para o front, para não haver confusão e erros e tambem para nosso código ficar fiel ao que necessita
///Resumindo temos nossa entidade modelo com todos itens que é necessario e temos nosso DTO que pegamos apenas o que queremos dessa entidade modelo
///è chamado de payload