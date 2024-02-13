var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/user", () => new
{
    Name = "Vitu",
    Age = 31

});
app.MapGet("/AddHeader", (HttpResponse response) => response.Headers.Add("X-My-Header", "adicionando coisas no header"));
app.MapGet("/AddHeaderAndReturnBody", (HttpResponse response) =>
{
    response.Headers.Add("X-My-Header", "adicionando coisas no header e retornando body");
    return new
    {
        Name = "Vitu",
        Age = 31
    };
});

app.MapPost("/saveProduct", (Product product) =>
{
    return product.Code + " - " + product.Name;
});

app.Run();

///NOTE:
/// 1. Não podemos criar endpoints de metodo de acesso semelhante(get - put - delete - post) com o mesmo nome, exemplo Get("\") e Get("\")
///new -> quando não tipo eu to criando um objeto anonimo
///Podemos adicionar items ao nosso header