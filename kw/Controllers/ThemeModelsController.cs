using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using kw.Data;
using kw.Models;
using kw.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace kw.Controllers
{
    public class ThemeModelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        
        public ThemeModelsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: ThemeModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.ThemeModel.Include(t => t.User).OrderBy(t => t.DateTime);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ThemeModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var themeModel = await _context.ThemeModel
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);

            List<Comment> comments = _context.Comment
                .OrderBy(a => a.DateTime)
                .Where(a => a.ThemeId == id).ToList();
            if (themeModel == null)
            {
                return NotFound();
            }
            CommentsInThemeViewModel viewModel = new CommentsInThemeViewModel()
            {
                Id = themeModel.Id,
                Title = themeModel.Title,
                Body = themeModel.Body,
                DateTime = themeModel.DateTime,
                UserName = themeModel.Name,
                Comments = comments
            };
           

            return View(viewModel);
        }

        // GET: ThemeModels/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: ThemeModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Body")] ThemeViewModel themeModel)
        {
            var user = await _userManager.GetUserAsync(User);

            ThemeModel model = new ThemeModel()
            {
                Title = themeModel.Title,
                Body = themeModel.Body,
                DateTime = DateTime.Now,
                UserId = user.Id,
                User = user,
                Name = user.NormalizedUserName
            };
            if (ModelState.IsValid)
            {
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(themeModel);
        }

        public PartialViewResult ShowComments(int id)
        {
            List<Comment> comments = _context.Comment
                .OrderBy(a => a.DateTime)
                .Where(a => a.ThemeId == id).ToList();
            CommentViewModel viewModel = new CommentViewModel()
            {
                Comments = comments
            };
            return PartialView("Comments", viewModel);
        }

        public async Task<PartialViewResult> CreateComment(int themeId, string text)
        {
            ThemeModel theme = _context.ThemeModel.FirstOrDefault(t => t.Id == themeId);
            var user = await _userManager.GetUserAsync(User);
            List<Comment> comments = _context.Comment
                .OrderBy(a => a.DateTime)
                .Where(a => a.ThemeId == themeId).ToList();
            CommentViewModel viewModel = new CommentViewModel()
            {
                Comments = comments
            };
            theme.CommentCount += 1;
            if (ModelState.IsValid)
            {
                Comment model = new Comment()
                {
                    UserId = user.Id,
                    Text = text,
                    DateTime = DateTime.Now,
                    ThemeId = theme.Id,
                    ThemeModel = theme,
                    User = user
                };
                _context.Update(theme);
                _context.Add(model);
                await _context.SaveChangesAsync();
            }
            
            return PartialView("Comments", viewModel);
        }

        

        // GET: ThemeModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var themeModel = await _context.ThemeModel.FindAsync(id);
            if (themeModel == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", themeModel.UserId);
            return View(themeModel);
        }

        // POST: ThemeModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,DateTime,UserId")] ThemeModel themeModel)
        {
            if (id != themeModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(themeModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ThemeModelExists(themeModel.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", themeModel.UserId);
            return View(themeModel);
        }

        // GET: ThemeModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var themeModel = await _context.ThemeModel
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (themeModel == null)
            {
                return NotFound();
            }

            return View(themeModel);
        }

        // POST: ThemeModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var themeModel = await _context.ThemeModel.FindAsync(id);
            _context.ThemeModel.Remove(themeModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ThemeModelExists(int id)
        {
            return _context.ThemeModel.Any(e => e.Id == id);
        }
    }
}
