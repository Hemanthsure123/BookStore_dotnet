using BookStore.Data;
using BookStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[Authorize]
public class CartController : Controller
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    // VIEW CART
    public async Task<IActionResult> Index()
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var cartItems = await _context.CartItems
            .Include(c => c.Book)
            .Where(c => c.UserId == userId)
            .ToListAsync();

        return View(cartItems);
    }

    // ADD TO CART
    [HttpPost]
    public async Task<IActionResult> AddToCart(int bookId, int quantity)
    {
        int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(c => c.BookId == bookId && c.UserId == userId);

        if (existingItem != null)
        {
            existingItem.Quantity += quantity;

            if (existingItem.Quantity > 10)
                existingItem.Quantity = 10;
        }
        else
        {
            _context.CartItems.Add(new CartItem
            {
                BookId = bookId,
                UserId = userId,
                Quantity = quantity
            });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    // REMOVE
    public async Task<IActionResult> Remove(int id)
    {
        var item = await _context.CartItems.FindAsync(id);

        if (item != null)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction("Index");
    }
}