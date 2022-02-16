using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookStore.Data;
using BookStore.Models;

namespace BookStore.Controllers
{
    public class BorrowingsController : Controller
    {
        private readonly BookContext _context;

        public BorrowingsController(BookContext context)
        {
            _context = context;
        }

        // GET: Borrowings
        public async Task<IActionResult> Index()
        {
            var bookContext = _context.Borrowings.Include(b => b.Book).Include(b => b.User);
            return View(await bookContext.ToListAsync());
        }

        // GET: Borrowings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowing == null)
            {
                return NotFound();
            }

            return View(borrowing);
        }

        // GET: Borrowings/Create
        public IActionResult Create()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Borrowings/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BookId,UserId,BorrowFrom,BorrowTo,Returned")] Borrowing borrowing)
        {
            if (ModelState.IsValid)
            {
                //First Validation For Borrowing
                var book = _context.Books
                    .Include(x => x.Borrowings)
                    .FirstOrDefault(b => b.Id == borrowing.BookId);
                int borrowCount = book.Borrowings.Where(x => x.Returned == null).Count();

                if (book.Copies > borrowCount)
                {
                    _context.Add(borrowing);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Error", "Sorry, No More Copies from this Book");
                }
             
                
             
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", borrowing.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", borrowing.UserId);
            return View(borrowing);
        }
        // GET: Borrowings/Return
        public IActionResult Return()
        {
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name");
            return View();
        }

        // POST: Borrowings/Return

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Return([Bind("BookId,UserId,ReuturnedDate")] ReturnBookViewModel returnBook)
        {
            if (ModelState.IsValid)
            {
                //Second Validation For Returning
                var borrowing = _context.Borrowings
                    .FirstOrDefault(b => b.BookId == returnBook.BookId
                    && b.UserId == returnBook.UserId
                    && b.Returned == null);
               

                if (borrowing != null)
                {
                    borrowing.Returned = returnBook.ReuturnedDate;
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("Error", "Sorry, No Existing Book Borowing For this User");
                }



            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", returnBook.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", returnBook.UserId);
            return View(returnBook);
        }

        // GET: Borrowings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowings.FindAsync(id);
            if (borrowing == null)
            {
                return NotFound();
            }
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", borrowing.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", borrowing.UserId);
            return View(borrowing);
        }

        // POST: Borrowings/Edit/5
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BookId,UserId,BorrowFrom,BorrowTo,Returned")] Borrowing borrowing)
        {
            if (id != borrowing.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(borrowing);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowingExists(borrowing.Id))
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
            ViewData["BookId"] = new SelectList(_context.Books, "Id", "Name", borrowing.BookId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Name", borrowing.UserId);
            return View(borrowing);
        }

        // GET: Borrowings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowing = await _context.Borrowings
                .Include(b => b.Book)
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowing == null)
            {
                return NotFound();
            }

            return View(borrowing);
        }

        // POST: Borrowings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrowing = await _context.Borrowings.FindAsync(id);
            _context.Borrowings.Remove(borrowing);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowingExists(int id)
        {
            return _context.Borrowings.Any(e => e.Id == id);
        }
    }
}
