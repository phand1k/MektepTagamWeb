using MektepTagamWeb.Data;
using MektepTagamWeb.Models;
using MektepTagamWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MektepTagamWeb.Controllers
{
    [Authorize(Roles = "Кассир")]
    public class CashBoxController : Controller
    {
        private readonly ApplicationDbContext context;
        private UserManager<AspNetUser> _userManager;
        public CashBoxController(ApplicationDbContext context, UserManager<AspNetUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }
        private async Task<AspNetUser> GetCurrentUserAsync() => await _userManager.GetUserAsync(HttpContext.User);
        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel
            {
                Transactions = await context.Transactions.Include(x => x.Dish).Include(x => x.CardCode.AspNetUser).Take(10).ToListAsync(),
            };

            return View(viewModel);
        }

        public async Task<IActionResult> EditDish(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var dish = await context.Dishes.Where(x=>x.OrganizationId == GetCurrentUserAsync().Result.OrganizationId).FirstOrDefaultAsync(x=>x.Id == id);
            if (dish == null)
            {
                return BadRequest();
            }
            return View(dish);
        }
        [HttpPost]
        public async Task<IActionResult> EditDish(Dish dish)
        {
            if (ModelState.IsValid)
            {
                context.Entry(dish).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return RedirectToAction("Dishes");
            }
            ModelState.AddModelError("", "");
            return View(dish);
        }
        public async Task<IActionResult> Dishes()
        {
            var list = await context.Dishes.
                Where(x=>x.IsDeleted == false && x.OrganizationId == GetCurrentUserAsync().Result.OrganizationId).ToListAsync();
            return View(list);
        }
        public async Task<IActionResult> CardCodes()
        {
            var list = await context.CardCodes.Include(x=>x.AspNetUser.Organization).
                Where(x=>x.IsDeleted == false && x.OrganizationId == GetCurrentUserAsync().Result.OrganizationId).ToListAsync();
            return View(list);
        }
        public IActionResult CreateCardCode()
        {
            ViewBag.AspNetUsers = new SelectList(context.AspNetUsers.
                Where(x => x.OrganizationId == GetCurrentUserAsync().Result.OrganizationId && x.IsDeleted == false), "Id", "FullName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCardCode(CardCode cardCode)
        {
            ViewBag.AspNetUsers = new SelectList(context.AspNetUsers.
               Where(x => x.OrganizationId == GetCurrentUserAsync().Result.OrganizationId && x.IsDeleted == false), "Id", "FullName");

            var codeExists = await context.CardCodes.Where(x=>x.IsDeleted == false && x.Code == cardCode.Code).FirstOrDefaultAsync();
            if (codeExists == null)
            {
                cardCode.OrganizationId = GetCurrentUserAsync().Result.OrganizationId;
                await context.CardCodes.AddAsync(cardCode);
                await context.SaveChangesAsync();
                return RedirectToAction("CardCodes");
            }
            return View(cardCode);
        }
        public IActionResult CreateDish()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateDish(Dish dish)
        {
            var dishExists = await context.Dishes.Where(x=>x.Name == dish.Name && x.IsDeleted == false).FirstOrDefaultAsync();
            if (dishExists == null)
            {
                dish.OrganizationId = GetCurrentUserAsync().Result.OrganizationId;
                await context.Dishes.AddAsync(dish);
                await context.SaveChangesAsync();
                return RedirectToAction("Dishes");
            }
            ModelState.AddModelError("Name", "Блюдо уже существует!");
            return View(dish);
        }

    }
}
