using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ProductService.Models;

namespace ProductService.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        public ProductServiceContext db = new ProductServiceContext();

        [Route]
        [HttpGet]
        public IQueryable<ProductDTO> GetProducts()
        {
            var products = from p in db.Products
                           select new ProductDTO()
                           {
                               Id = p.Id,
                               Name = p.Name,
                               Description = p.Description,
                               Price = p.Price,
                               DeliveryPrice = p.DeliveryPrice,
                           };
            return products;
        }

        [Route]
        [HttpGet]
        public async Task<IHttpActionResult> SearchProducts(string name)
        {
            var products = await db.Products
                                    .Where(p =>
                                        p.Name.ToLower().Contains(name.ToLower()))
                                    .Select(p =>
                                        new ProductDTO() {
                                            Id = p.Id,
                                            Name = p.Name,
                                            Description = p.Description,
                                            Price = p.Price,
                                            DeliveryPrice = p.DeliveryPrice,
                                        }).ToListAsync();

            return Ok(products);
        }

        [Route("{id}", Name="GetProductById")]
        [HttpGet]
        [ResponseType(typeof(ProductDTO))]
        public async Task<IHttpActionResult> GetProduct(Guid id)
        {
            var product = await db.Products.Select(p =>
                new ProductDTO()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    DeliveryPrice = p.DeliveryPrice
                }).SingleOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [Route]
        [HttpPost]
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> CreateProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Products.Add(product);
            await db.SaveChangesAsync();

            var dto = new ProductDTO()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice,
            };

            return CreatedAtRoute("GetProductById", new { id = product.Id }, dto);
        }

        [Route("{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateProduct(Guid id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        [Route("{id}")]
        [HttpDelete]
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> DeleteProduct(Guid id)
        {
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            await db.SaveChangesAsync();

            return Ok(product);
        }

        private bool ProductExists(Guid id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }

        [Route("{productId}/options")]
        [HttpGet]
        public IQueryable<ProductOptionDTO> GetProductOptions(Guid productId)
        {
            var productOptions = from p in db.ProductOptions
                                 where p.ProductId == productId
                                 select new ProductOptionDTO()
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     Description = p.Description
                                 };
            return productOptions;
        }

        [Route("{productId}/options/{id}", Name="GetProductOptionById")]
        [HttpGet]
        [ResponseType(typeof(ProductDTO))]
        public async Task<IHttpActionResult> GetProductOptions(Guid productId, Guid id)
        {
            var productOption = await db.ProductOptions
                .Where(p => p.ProductId == productId)
                .Select(p =>
                    new ProductOptionDTO()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                    }).SingleOrDefaultAsync(p => p.Id == id);

            if (productOption == null)
            {
                return NotFound();
            }

            return Ok(productOption);
        }

        [Route("{productId}/options")]
        [HttpPost]
        [ResponseType(typeof(Product))]
        public async Task<IHttpActionResult> CreateProductOption(Guid productId, ProductOption productOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (productId != productOption.ProductId)
            {
                return BadRequest();
            }

            db.ProductOptions.Add(productOption);
            await db.SaveChangesAsync();

            var dto = new ProductOptionDTO()
            {
                Id = productOption.Id,
                Name = productOption.Name,
                Description = productOption.Description
            };

            return CreatedAtRoute("GetProductOptionById", new { productId = productOption.ProductId, id = productOption.Id }, dto);
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> UpdateProductOption(Guid productId, Guid id, ProductOption productOption)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (productId != productOption.ProductId || id != productOption.Id)
            {
                return BadRequest();
            }

            db.Entry(productOption).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductOptionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(System.Net.HttpStatusCode.NoContent);
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        [ResponseType(typeof(ProductOption))]
        public async Task<IHttpActionResult> DeleteProductOption(Guid id)
        {
            ProductOption productOption = await db.ProductOptions.FindAsync(id); 
            if (productOption == null)
            {
                return NotFound();
            }

            db.ProductOptions.Remove(productOption);
            await db.SaveChangesAsync();

            return Ok(productOption);
        }

        private bool ProductOptionExists(Guid id)
        {
            return db.ProductOptions.Count(e => e.Id == id) > 0;
        }
    }
}
