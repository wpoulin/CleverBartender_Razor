using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CleverBartender_v2.Models;
using System.Text;

namespace CleverBartender_v2.Controllers
{
    public class DrinksController : Controller
    {
        private readonly DrinkContext _context;

        public DrinksController(DrinkContext context)
        {
            _context = context;
        }

        // GET: Drinks
        public async Task<IActionResult> Index()
        {
            var mobile = _context.MobileNodes.ToArray();

            List<SelectListItem> items = new List<SelectListItem>();

            foreach (MobileNode device in mobile)
            {
                items.Add(new SelectListItem { Text = device.Name, Value = device.IpAddress });
            }

            ViewBag.DeviceType = items;

            ViewData["test"] = GlobalVariables.socketStarted;

            return View(await _context.Drinks.ToListAsync());
        }

        // GET: Drinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drinks.FirstOrDefaultAsync(m => m.Id == id);
            var recipe = await _context.Recipes.Where(t =>t.DrinkId == id).ToListAsync();
            var recipeIdList = recipe.Select(c => c.IngredientId).ToList();

            List<OrderDrink> OrderList = new List<OrderDrink>();
            int iteration = 0;

            foreach (int ids in recipeIdList)
            {
                var orderDrink = new OrderDrink();
                var ingredient = _context.Ingredients.Where(e => e.Id == ids);
                orderDrink.Name = ingredient.First().Name;
                orderDrink.PumpNumber = ingredient.First().PumpNumber;

                var quantity = recipe.Skip(iteration).Take(1);
                orderDrink.Quantity = quantity.First().Quantity;

                OrderList.Add(orderDrink);
                iteration++;
            }

            ViewData["recipe"] = OrderList;

            if (drink == null)
            {
                return NotFound();
            }

            return View(drink);
        }

        // GET: Drinks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Drink drink)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drink);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(drink);
        }

        // GET: Drinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drinks.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }
            return View(drink);
        }

        // POST: Drinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Drink drink)
        {
            if (id != drink.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drink);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrinkExists(drink.Id))
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
            return View(drink);
        }

        // GET: Drinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drinks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drink == null)
            {
                return NotFound();
            }

            return View(drink);
        }

        // POST: Drinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var drink = await _context.Drinks.FindAsync(id);
            _context.Drinks.Remove(drink);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }






        // GET: Drinks/Order/5
        public async Task<IActionResult> Order(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var drink = await _context.Drinks.FirstOrDefaultAsync(m => m.Id == id);
            var recipe = await _context.Recipes.Where(t => t.DrinkId == id).ToListAsync();
            var recipeIdList = recipe.Select(c => c.IngredientId).ToList();

            List<OrderDrink> OrderList = new List<OrderDrink>();
            int iteration = 0;

            foreach (int ids in recipeIdList)
            {
                var orderDrink = new OrderDrink();
                var ingredient = _context.Ingredients.Where(e => e.Id == ids);
                orderDrink.Name = ingredient.First().Name;
                orderDrink.PumpNumber = ingredient.First().PumpNumber;

                var quantity = recipe.Skip(iteration).Take(1);
                orderDrink.Quantity = quantity.First().Quantity;

                OrderList.Add(orderDrink);
                iteration++;
            }

            if (drink == null)
            {
                return NotFound();
            }

            //SocketBuildMessage(OrderList, 1);
            //SocketSendData();

            return RedirectToAction("Index", "Drinks");
        }







        private bool DrinkExists(int id)
        {
            return _context.Drinks.Any(e => e.Id == id);
        }

        private void SocketBuildMessage(List<OrderDrink> orderList, int noeudFixe)
        {
            int noeud = noeudFixe;

            int[] qty = new int[4];


            foreach (var ingredient in orderList)
            {
                qty[ingredient.PumpNumber-1] = ingredient.Quantity;
            }

            SocketSendData(noeudFixe,qty[0], qty[1], qty[2], qty[3]);
        }

        private void SocketSendData(int Noeud, int qty1, int qty2, int qty3, int qty4)
        {
            //Byte[] response2 = Encoding.UTF8.GetBytes("test");
            Byte[] response2 = new byte[3];
            //int qty1 = 6, qty2 = 0, qty3 = 0, qty4 = 0;

            response2[0] = (Byte) (0x01); // Choix du mbed
            response2[0] = (Byte)(0xFF & Noeud); // Choix du mbed
            response2[1] = (Byte) ((0xF0 & (qty1 << 4)) | (0x0F & qty2)); // 4 bits pour qty1 et 4 bits pour qty2
            response2[2] = (Byte) ((0xF0 & (qty3 << 4)) | (0x0F & qty4)); // 4 bits pour qty3 et 4 bits pour qty4
            

            //Byte[] test = new Byte[] { bytesM[0], bytesM[1], bytesM[2], bytesM[3], bytesM[4], bytesM[5] };
            Byte[] test = new Byte[] { 129, (byte)(128 + response2.Length), 0, 0, 0, 0 };
            Byte[] test2 = new Byte[test.Length + response2.Length];
            System.Buffer.BlockCopy(test, 0, test2, 0, test.Length);
            System.Buffer.BlockCopy(response2, 0, test2, test.Length, response2.Length);

            GlobalVariables.stream.Write(test2, 0, test2.Length);
        }
    }
}
