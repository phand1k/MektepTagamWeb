using MektepTagamWeb.Data;
using MektepTagamWeb.Models;
using MektepTagamWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace MektepTagamWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                if (User.IsInRole("Администратор"))
                {
                    var user = User.Identity.Name;
                    return View();
                }
                else if (User.IsInRole("Кассир"))
                {
                    return RedirectToAction("Index", "CashBox");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddToBalance()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddToBalance(TransactionCreateModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await context.AspNetUsers
                            .FirstOrDefaultAsync(u => u.IndividualNumber == model.IndividualNumber);

            if (user == null)
            {
                ModelState.AddModelError("IndividualNumber", "Пользователь с таким ИИН не найден");
                return View(model);
            }
            var cardCodeId = await context.CardCodes.
                Where(x=>x.AspNetUserId == user.Id).Select(x=>x.Id).FirstOrDefaultAsync();

            Guid? dishIdForStandart = await context.Dishes.
                Where(x=>x.Name == "Default dish").Select(x=>x.Id).FirstOrDefaultAsync();

            var transaction = new Transaction
            {
                Amount = model.Amount,
                CardCodeId = cardCodeId,
                DateOfCreatedTransaction = DateTime.Now,
                OrganizationId = user.OrganizationId,
                DishId = dishIdForStandart
            };

            context.Transactions.Add(transaction);
            await context.SaveChangesAsync();

            return RedirectToAction("Index"); // Переадресация на нужную страницу после создания
        }
        public IActionResult RegisterOrganization()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RegisterOrganization(Organization organization)
        {
            var organizationExists = await context.Organizations
                .Where(x => x.Number == organization.Number)
                .FirstOrDefaultAsync();

            if (organizationExists != null)
            {
                ModelState.AddModelError("IndividualNumber", "Organization exists!");
                return View(organization);
            }

            await context.Organizations.AddAsync(organization);
            await context.SaveChangesAsync();
            AspNetUser defaultUser = new()
            {
                Email = Guid.NewGuid().ToString(),
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = Guid.NewGuid().ToString(),
                PhoneNumber = Guid.NewGuid().ToString(),
                FirstName = "Default",
                LastName = "User",
                PasswordHash = Guid.NewGuid().ToString(),
                NormalizedEmail = Guid.NewGuid().ToString(),
            };

            defaultUser.OrganizationId = organization.Id;

            await context.AspNetUsers.AddAsync(defaultUser);
            Dish dish = new Dish();
            dish.Description = "Стандартное блюдо, которое создается автоматически";
            dish.Name = "Default dish";
            dish.Price = 1.0;
            dish.OrganizationId = organization.Id;
            await context.Dishes.AddAsync(dish);

            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}