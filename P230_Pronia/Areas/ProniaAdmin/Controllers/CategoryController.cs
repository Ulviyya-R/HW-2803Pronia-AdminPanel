using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using P230_Pronia.DAL;
using P230_Pronia.Entities;

namespace P230_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class CategoryController:Controller
    {
        private readonly ProniaDbContext _context;

        public CategoryController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Category>  categories = _context.Categories.AsEnumerable();
            return View(categories);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category newCategory) 
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("Name", "You can't add dublicate data");
                return View();
            }
            _context.Categories.Add(newCategory);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if(category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(int id, Category edited)
        {
            if (id != edited.Id) return BadRequest();
            Category category = _context.Categories.FirstOrDefault(c =>c.Id==id);
            if(category == null) return NotFound();
            bool dublicate = _context.Categories.Any(c=>c.Name==edited.Name);
            if (dublicate)
            {
                ModelState.AddModelError("", "You cannot dublicate category name");
                return View();
            }
            category.Name=edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if(id==0) return NotFound();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if(category == null) return NotFound();
            return View(category);
        }
        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if(category == null) return NotFound();
            return View(category);
        }

        [HttpPost]
        public IActionResult Delete(int id, Category delete)
        {
            if (id != delete.Id) return BadRequest();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if (category == null) return NotFound();
            delete = _context.Categories.FirstOrDefault(c => c.Id == id);
            _context.Categories.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
