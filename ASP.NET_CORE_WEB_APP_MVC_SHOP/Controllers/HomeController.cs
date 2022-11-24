using ASP.NET_CORE_WEB_APP_MVC_SHOP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using System.Diagnostics;

namespace ASP.NET_CORE_WEB_APP_MVC_SHOP.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _dbContext;


        public HomeController(ILogger<HomeController> logger, AppDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> GetProducts()
        {
            return View(await _dbContext.Products.ToListAsync());
        }

        public  IActionResult Create()
        {

            return View(new Product());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                _dbContext.Add(product);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("GetProducts");
            }

            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "error, can not save changes");
            }

            return View(product);
        }

        public async Task<IActionResult> Edit(Guid Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var product = await _dbContext.Products.FirstOrDefaultAsync(m => m.ProductId == Id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var product = await _dbContext.Products.FirstOrDefaultAsync(s => s.ProductId == Id);


            if (await TryUpdateModelAsync<Product>(
                product, "", s => s.ProductName))
            {
                try
                {
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "error, can not save changes");
                }
            }

            return View(product);
        }
        public async Task<IActionResult> Delete(Guid id, bool? Savechangeserror = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _dbContext.Products.AsNoTracking().FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }

            if (Savechangeserror.GetValueOrDefault())
            {
                ViewData["DeleteError"] = "Delete failed";
            }

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _dbContext.Products.Remove(product);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Delete), new { id = id, Savechangeserror = true });
            }
        }


        public IActionResult Login()
        {
            return View();
        }
        public void LoginAction()
        {
            string login = Request.Query["login"].ToString();
            string passwrod = Request.Query["password"].ToString();

            var users = from f in _dbContext.Users
                          select f;
            if (!String.IsNullOrEmpty(login))
            {
                users = from f in users where f.Login.Equals(login) select f;
            }
            if (users.Any())
            {
                users = from f in users where f.Password.Equals(passwrod) select f;
                if (users.Any())
                {
                    Response.StatusCode = 200;
                    Response.WriteAsync("Success");
                    return;
                }
                else
                {
                    Response.StatusCode = 500;
                    Response.WriteAsync("Wrong password");
                    return;
                }
            }
   
            Response.StatusCode = 404;
            Response.WriteAsync("Failure");
            return;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}