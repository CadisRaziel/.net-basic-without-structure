public static class ProductRepository
{
    public static List<Product> Products { get; set; }

    public static void AddProduct(Product product)
    {
        if (Products == null)
        {
            Products = new List<Product>();
        }
        else
        {
            Products.Add(product);
        }
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