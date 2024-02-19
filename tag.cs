public class Tag {
    public int Id { get; set; }
    public string Name { get; set; }

    //Estamos criando o ProductId que é uma PK para não ser NULLA !! por isso criamos esse campo
    public int ProductId { get; set; }
}