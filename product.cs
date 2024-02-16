public class Product {
    public int Id { get; set; } //Ele ja entende que esse vai ser a primary Key só de colocar 'Id'
    public string? Code { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }

    //Aqui eu criei o CategoryId pois ele pega o Category debaixo e concatena com o Id de Category, com isso eu digo que o CategoryId que ele cria na db nao pode ser NULO !!!
    //è muito importante fazer isso, mais lembre de deixar os nomes e os tipos iguais Category e CategoryId int
    public int CategoryId { get; set; } 
    public Category? Category { get; set; } //O entityFramework é inteligente o suficiente para entender que essa propriedade é uma chave estrangeira e vai realizar o relacionamento OneToOne


    //Pelo que deu pra entender quando colcoamos como uma "LIST" a gente ta falando que é oneToMany !!
    public List<Tag> Tags { get; set; } //Produto pode ter varias tags
}