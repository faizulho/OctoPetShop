using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OctopusSamples.ProductService.Models;
using OctopusSamples.ProductService.Repositories;

namespace OctopusSamples.ProductService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IList<ProductDetail> _productCache = new List<ProductDetail>();

        public ProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ProductDetail>>> GetAllAsync()
        {  
            return await _productRepository.GetAll();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ProductDetail>> GetByIdAsync(int id)
        {
            ProductDetail product = null;

            var cachedProduct = _productCache.FirstOrDefault(x => x.Id == id);

            if (cachedProduct != null) 
            {
                product = cachedProduct;
            }
            else 
            {   
                product = await _productRepository.GetById(id);
            }

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
    }
}