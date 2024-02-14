using Microsoft.AspNetCore.Mvc;

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
    ProductRepository.AddProduct(product);
    return Results.Created($"/saveProduct/{product.Code}", product.Code);
});
app.MapGet("/getproductRepository/{code}", ([FromRoute] string code) =>
{
    var product = ProductRepository.GetBy(code);
    if (product != null)
    {
        return Results.Ok(product);
    }
    else
    {
        return Results.NotFound();
    }
});
app.MapPut("/editProduct", (Product product) =>
{
    var productSave = ProductRepository.GetBy(product.Code);
    productSave.Name = product.Name;
    return Results.Ok();
});
app.MapDelete("/deleteProduct/{code}", ([FromRoute] string code) =>
{
    var productSave = ProductRepository.GetBy(code);
    ProductRepository.Remove(productSave);
    return Results.Ok();
});

app.MapGet("/getproduct", ([FromQuery] string dateStart, [FromQuery] string dateEnd) =>
{
    return dateStart + " - " + dateEnd;
});
app.MapGet("/getproduct/{code}", ([FromRoute] string code) =>
{
    return code;
});

app.MapGet("/getproductheader", (HttpRequest request) =>
{
    return request.Headers["product-code"].ToString();
});

app.Run();



///NOTE:
/// 1. Não podemos criar endpoints de metodo de acesso semelhante(get - put - delete - post) com o mesmo nome, exemplo Get("\") e Get("\")
///new -> quando não tipo eu to criando um objeto anonimo
///Podemos adicionar items ao nosso header
///[FromQuery] -> para pegar parametros da url (tudo depois da ? ma url é parametros "api.app.com/getproduct?datastart={date}&dateend={date}")
///[FromRoute] -> para pegar parametros da rota "api.app.com/getproduct/1"
///HttpRequest -> è responsavel por receber as solicitações do usuario no endpoint (no postman no header a gente coloca a chave product-code e o valor dela, e quando o usaurio digitar essa url vai retornar o valor), nao e comum mais posso usar para mandar o token