using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//configurando o serviço do db
builder.Services.AddSqlServer<ApplicationDbContext>(builder.Configuration["Database:SqlServer"]);

var app = builder.Build();

//app.Configuration -> ele sabe que é o configuration que definimos no repoitory e no appsettings
//Aqui vamos inicializar (antes da api executar, ele seta a lista de produtos)
var configuration = app.Configuration;
ProductRepository.InitProducts(configuration);

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


//!========== CRUD ==========
app.MapPost("/saveProduct", (RecordProductRequestDTO recordProductRequestDTO, ApplicationDbContext  context) =>
{
    var category = context.Category.Where(c => c.Id == recordProductRequestDTO.CategoryId).First();
    var product = new Product{
        Code = recordProductRequestDTO.Code,
        Name = recordProductRequestDTO.Name,
        Description = recordProductRequestDTO.Description,
        Category = category
    };

    if(recordProductRequestDTO.Tags != null) {
        product.Tags = new List<Tag>();
        foreach (var item in recordProductRequestDTO.Tags)
        {
            product.Tags.Add(new Tag { Name = item });
        }
    }

    context.Products.Add(product);  
    context.SaveChanges(); //Salva no banco
    return Results.Created($"/saveProduct/{product.Id}", product.Id);
});
app.MapGet("/getproductRepository/{id}", ([FromRoute] int id, ApplicationDbContext  context) =>
{

    ///NOTE
    ///Include -> para incluir a categoria e as tags do produto (incluir os relacionamentos que o product tem)
    ///Se nao colocar ele, ele só vai retornar os 'Products' indepentende se la dentro de 'Product' tem relacionamento de tabela ou nao

    var product = context.Products    
    .Include(t => t.Tags)
    .Where(p => p.Id == id).First();
    if (product != null)
    {
        return Results.Ok(product);
    }
    else
    {
        return Results.NotFound();
    }
});
app.MapPut("/editProduct/{id}", ([FromRoute] int id, RecordProductRequestDTO recordProductRequestDTO, ApplicationDbContext  context) =>
{
    var product = context.Products  
    .Include(t => t.Tags)
    .Where(p => p.Id == id).First();

    var category = context.Category.Where(c => c.Id == recordProductRequestDTO.CategoryId).First();

    product.Code = recordProductRequestDTO.Code;
    product.Name = recordProductRequestDTO.Name;
    product.Description = recordProductRequestDTO.Description; 
    product.Category = category; 

    //Ao remover a lista de tags em seguida eu ja crio uma nova atualizando
    //Se eu nao remover a lista antiga e atualizar ela o que vai acontecer ?
    //Vai acontecer que ela vai manter os dados antigos dessa lista e cria dados novos logo abaixo dos antigos (tudo isso no banco de dados)
    if(recordProductRequestDTO.Tags != null) {
        product.Tags = new List<Tag>(); //Ou seja se o usuario atualizar a lista de tags é necessario ter esse código para remover os antigos e incluir os novos (para nao sujar o banco)
        //Agora se for um sistema que não tem problema, podemos deixar salvo as tags antigas eu só comento esse código acima.

        foreach (var item in recordProductRequestDTO.Tags)
        {
            product.Tags.Add(new Tag { Name = item });
        }
    }
   
   context.SaveChanges();
    return Results.Ok();
});
app.MapDelete("/deleteProduct/{id}", ([FromRoute] int id, ApplicationDbContext  context) =>
{
    var product = context.Products.Where(p => p.Id == id).First();
    context.Products.Remove(product);
    context.SaveChanges();
    return Results.Ok();
});
//!========== CRUD ==========


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

if(app.Environment.IsStaging()){ //-> Defino que o endpoint só vai funcionar em modo stage
app.MapGet("/configuration/databaseStage", (IConfiguration configuration) => {
    //retorna o nome/port do servidor que esta no arquivo appsettings.json
    return Results.Ok($"{configuration["database:connection"]}/{configuration["database:port"]}");
});
}

if(app.Environment.IsDevelopment()){ //-> Defino que o endpoint só vai funcionar em modo Prod
app.MapGet("/configuration/databaseProd", (IConfiguration configuration) => {
    //retorna o nome/port do servidor que esta no arquivo appsettings.json
    return Results.Ok($"{configuration["database:connection"]}/{configuration["database:port"]}");
});
}


app.Run();



///NOTE:
/// 1. Não podemos criar endpoints de metodo de acesso semelhante(get - put - delete - post) com o mesmo nome, exemplo Get("\") e Get("\")
///new -> quando não tipo eu to criando um objeto anonimo
///Podemos adicionar items ao nosso header
///[FromQuery] -> para pegar parametros da url (tudo depois da ? ma url é parametros "api.app.com/getproduct?datastart={date}&dateend={date}")
///[FromRoute] -> para pegar parametros da rota "api.app.com/getproduct/1"
///HttpRequest -> è responsavel por receber as solicitações do usuario no endpoint (no postman no header a gente coloca a chave product-code e o valor dela, e quando o usaurio digitar essa url vai retornar o valor), nao e comum mais posso usar para mandar o token