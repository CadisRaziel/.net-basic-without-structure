public static class ProductRepository
{
    public static List<Product> Products { get; set; } = Products = new List<Product>(); //inicializando a lista quando inicia o projeto

    public static void InitProducts(IConfiguration configuration)
    {
    //Pegando a lista de products que esta no arquivo appsettings.json(onde criamos uma lista com produtos para inicializar assim que o programa iniciar)
        var products = configuration.GetSection("Products").Get<List<Product>>();
        //Get<List<Product>>() -> converter para o tipo correto que é List<Product>

        Products = products;
    }

    public static void AddProduct(Product product)
    {
           Products.Add(product);        
    }
    public static Product GetBy(string code)
    {
        return Products.FirstOrDefault(x => x.Code == code);
    }

    public static void Remove(Product product) {
        Products.Remove(product);
    
    }
}

///Note:
///Metodos static -> uma classe estática permanece namemória durante toda a execução do programa, e seus membros podem ser acessados sem a necessidade de criar instâncias da classe. 