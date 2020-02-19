namespace Tienda.Web.Controllers
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Data.Entities;
    using Helpers;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Tienda.Web.Models;

    public class ProductsController : Controller
    {
        private readonly IProductRepository productRepository;

        private readonly IUserHelper userHelper;

        public ProductsController(IProductRepository productRepository, IUserHelper userHelper)
        {
            this.productRepository = productRepository;
            this.userHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(this.productRepository.GetAll().OrderBy(x=>x.Name));
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel productView)
        {
            if (ModelState.IsValid)
            {
                //Variable path como un string vacío
                var path = string.Empty;

                if (productView.ImageFile != null && productView.ImageFile.Length > 0)
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), 
                        "wwwroot\\images\\Products",
                        productView.ImageFile.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await productView.ImageFile.CopyToAsync(stream);
                    }

                    path = $"~/images/Products/{productView.ImageFile.FileName}";
                }

                var product = this.ToProduct(productView, path);

                // TODO: Pending to change to: this.User.Identity.Name
                product.User = await this.userHelper.GetUserByEmailAsync("dbocastillo@gmail.com");
                await this.productRepository.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }

            return View(productView);
        }

        private Product ToProduct(ProductViewModel productView, string path)
        {
            return new Product
            {
                Id =  productView.Id,
                ImageUrl =  path,
                IsAvailabe = productView.IsAvailabe,
                LastPurchase = productView.LastPurchase,
                LastSale = productView.LastSale,
                Name =  productView.Name,
                Price =  productView.Price,
                Stock = productView.Stock,
                User =  productView.User
            };
        }
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            var productView = this.ToProductView(product);

            return View(productView);
        }

        private ProductViewModel ToProductView(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                ImageUrl = product.ImageUrl,
                IsAvailabe = product.IsAvailabe,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                User = product.User
            };

        }


        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel productView)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var path = productView.ImageUrl;

                    if (productView.ImageFile != null && productView.ImageFile.Length > 0)
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\Products", productView.ImageFile.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await productView.ImageFile.CopyToAsync(stream);
                        }

                        path = $"~/images/Products/{productView.ImageFile.FileName}";
                    }

                    var product = this.ToProduct(productView, path);

                    // TODO: Pending to change to: this.User.Identity.Name
                    product.User = await this.userHelper.GetUserByEmailAsync("dbocastillo@gmail.com");
                    await this.productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await this.productRepository.ExistAsync(productView.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(productView);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await this.productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await this.productRepository.GetByIdAsync(id);
            await this.productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }
    }
}
