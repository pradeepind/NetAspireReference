namespace Catalog.Endpoints
{
    public static class ProductEndPoints
    {
        public static void MapProductEndPoints(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/products");

            // GET all
            group.MapGet("/", async (ProductService productService) =>
            {
                var products = await productService.GetProductsAsync();
                return Results.Ok(products);
            })
                .WithName("GetAllProducts")
                .Produces<List<Product>>(StatusCodes.Status200OK);

            // GET by ID
            group.MapGet("/{id}", async (int id, ProductService productService) =>
            {
                var product = await productService.GetProductByIdAsync(id);
                return product is not null ? Results.Ok(product) : Results.NotFound();
            })
                .WithName("GetProductById")
                .Produces<Product>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound);

            // POST (Create)
            group.MapPost("/", async (Product product, ProductService productService) =>
            {
                await productService.CreateProductAsync(product);
                return Results.Created($"/products/{product.Id}", product);
            })
                .WithName("CreateProduct")
                .Produces<Product>(StatusCodes.Status201Created);

            // PUT (Update)
            group.MapPut("/{id}", async (int id, Product inputProduct, ProductService productService) =>
            {
                var product = await productService.GetProductByIdAsync(id);
                if (product is null)
                {
                    return Results.NotFound();
                }
                await productService.UpdateProductAsync(product, inputProduct);
                return Results.NoContent();
            })
                .WithName("UpdateProduct")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);

            // DELETE
            group.MapDelete("/{id}", async (int id, ProductService productService) =>
            {
                var product = await productService.GetProductByIdAsync(id);
                if (product is null)
                {
                    return Results.NotFound();
                }
                await productService.DeleteProductAsync(product);
                return Results.NoContent();
            })
                .WithName("DeleteProduct")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status404NotFound);
        }
    }
}
