using _16noyabr.DAL;
using _16noyabr.Models;
using _16noyabr.Utilities.Extention;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _16noyabr.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class SlideController : Controller
    {

        public AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SlideController(AppDbContext context, IWebHostEnvironment env)

        {
            _context = context;
            _env = env;
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
            if (!slide.Image.ValidateType("image/"))
            {
                ModelState.AddModelError("Photo", "yanlis file tipi");
                return View();
            }

            if (slide.Image.ValidateSize(2 * 1024*1024))
            {
                ModelState.AddModelError("Image", "2mb olsun");
                return View();
            }



            slide.Images = await slide.Image.CreateFile(_env.WebRootPath, "assets", "images", "slider");

            await _context.Slides.AddAsync(slide);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0) return BadRequest();
            Slide existed = await _context.Slides.FirstOrDefaultAsync(slide => slide.Id == id);
            if (existed is null) return NotFound();
            return View(existed);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, Slide slide)
        {
            Slide existed = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (existed is null) return NotFound(nameof(existed));
            if (ModelState.IsValid)
            {
                return View(existed);
            }

            if (slide.Image is not null)
            {
                if (!slide.Image.ValidateType("image/"))
                {
                    ModelState.AddModelError("Photo", "yanlis file tipi");
                    return View(existed);
                }

                if (slide.Image.ValidateSize(2 * 1024 * 1024))
                {
                    ModelState.AddModelError("Image", "2mb olsun");
                    return View(existed);
                }
                string newImage = await slide.Image.CreateFile(_env.WebRootPath, "assets", "images", "slider");
                existed.Images.DeleteFile(_env.WebRootPath, "assets", "images", "slider");
                existed.Images = newImage;
                 }
        existed.Title = slide.Title;
            existed.Description = slide.Description;
            existed.SubTitle = slide.SubTitle;
            existed.Order = slide.Order;

            _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));



        }





        public async Task<IActionResult> Details(int id)
        {
            if (id <= 0) return BadRequest();
            var slide = await _context.Slides.FirstOrDefaultAsync(s => s.Id == id);
            if (slide is null) return NotFound();

            return View(slide);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();

            Slide slide = await _context.Slides.FirstOrDefaultAsync(slide => slide.Id == id);
            if (slide is null) return NotFound();
           slide.Images.DeleteFile(_env.WebRootPath,"assets","images","slider");

            _context.Slides.Remove(slide);
           await _context.SaveChangesAsync();
            return RedirectToAction("Index");


        }
    }
 }
   
