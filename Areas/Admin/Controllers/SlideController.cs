using _16noyabr.DAL;
using _16noyabr.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _16noyabr.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SlideController : Controller
    {

        public AppDbContext _context;
        public SlideController(AppDbContext context) 
        
        {
            _context = context;
        }

       

        public async Task<IActionResult> Index()
        {
            List<Slide> slides = await _context.Slides.ToListAsync();

            return View(slides);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Slide slide)
        {















            if (slide.Image is null)
            {
                ModelState.AddModelError("Image", "Sekil secin");
                return View();
            }
            if (!slide.Image.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", "yanlis file tipi");
                return View();
            }

            if (slide.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "2mb olsun");
                return View();
            }
            FileStream file = new FileStream(@"C:\Users\Zeynal\Desktop\Noyabr17\wwwroot\assets\images\slider\" + slide.Image.FileName, FileMode.Create);
            await slide.Image.CopyToAsync(file);

            slide.Images = slide.Image.FileName;

            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();
            var slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide is null) return NotFound();

            return View(slide);
        }
    }





          
        }
   
