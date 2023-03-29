using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using P230_Pronia.DAL;
using P230_Pronia.Entities;

namespace P230_Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class TagController:Controller
    {
        private readonly ProniaDbContext _context;

        public TagController(ProniaDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Tag> tags = _context.Tags.AsEnumerable();
            return View(tags);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(Tag newTag)
        {
            //if (!ModelState.IsValid)
            //{
            //    ModelState.AddModelError("Name", "You can't add dublicate data");
            //    return View();
            //}
            _context.Tags.Add(newTag);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        public IActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        [HttpPost]
        public IActionResult Edit(int id, Tag edited)
        {
            if (id != edited.Id) return BadRequest();
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            if (tag == null) return NotFound();
            bool dublicate = _context.Tags.Any(c => c.Name == edited.Name);
            if (dublicate)
            {
                ModelState.AddModelError("", "You cannot dublicate category name");
                return View();
            }
            tag.Name = edited.Name;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            if (id == 0) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            if (tag == null) return NotFound();
            return View(tag);
        }
        public IActionResult Delete(int id)
        {
            if (id == 0) return NotFound();
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        [HttpPost]
        public IActionResult Delete(int id, Tag delete)
        {
            if (id != delete.Id) return BadRequest();
            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            if (tag == null) return NotFound();
            delete = _context.Tags.FirstOrDefault(c => c.Id == id);
            _context.Tags.Remove(delete);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
