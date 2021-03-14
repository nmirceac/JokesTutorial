using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using JokesTutorial.Data;
using JokesTutorial.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

namespace JokesTutorial.Controllers
{
    public class JokesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JokesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Jokes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Joke.ToListAsync());
        }


        // GET: Jokes/SearchForm
        public IActionResult SearchForm()
        {
            return View("SearchForm");
        }

        // GET: Jokes/Policy
        // public IActionResult Policy()
        // {
        //     string apiEndPoint;
        //     Dictionary<string, string> endpointParams;
        //     dynamic endpointData;

        //     apiEndPoint = "api/policies/attachFile/{id}";
        //     endpointParams = new Dictionary<string, string>() {{ "id", "1" }};

        //     endpointData = new ExpandoObject();
        //     endpointData.file = ApiClientTools.File.getFilePayloadFromPath("test.pdf");

        //     var data = ApiClientTools.Client.doPost(apiEndPoint, endpointParams, endpointData);
        //     return Json(data.Result);
        // }

        // // GET: Jokes/Will
        // public IActionResult Will()
        // {
        //     return Json(Api.Region.getResource());

        //     string apiEndPoint;
        //     Dictionary<string, string> endpointParams;
        //     dynamic endpointData;

        //     apiEndPoint = "api/wills/attachDetailFile/{id}";
        //     endpointParams = new Dictionary<string, string>() {{ "id", "1" }};

        //     endpointData = new ExpandoObject();
        //     endpointData.file = ApiClientTools.File.getFilePayloadFromPath("test.xlsx");

        //     var data = Api.Client.doPost(apiEndPoint, endpointParams, endpointData);
        //     return Json(data.Result);
        // }
        

        // GET: Jokes/Test
        public async Task<IActionResult> Test()
        {
            ApiClientTools.Response response;
            string apiEndPoint;
            Dictionary<string, string> endpointParams;
            dynamic endpointData;

            endpointData = new ExpandoObject();
            endpointData.quote_id = 45;
            endpointData.client_name = "ApiTools Rulz";

            return Json(Api.Quote.setClientName(endpointData));


            return Json(Api.Region.get(3));

            endpointParams = new System.Collections.Generic.Dictionary<string, string>()
            {
            };
            return Json(Api.Region.index(page:2));


            

            apiEndPoint = "api/users/getAuthData/{id}";
            apiEndPoint = "api/regions/getResource";
            
            endpointParams = new Dictionary<string, string>() {
                { "id", "1" },
                // { "something", "u2" },
                // { "that", "take that" },
            };
            endpointData = new Dictionary<string, string>() {
                // {"page", "1"},
                // {"perPage", "25"},
                // {"search", "Mircea&Jeandrew like cheese"},
            };


            response = await ApiClientTools.Client.doGet(apiEndPoint, endpointParams, endpointData);
            return Json(response);


            dynamic testPayload = ApiClientTools.File.getFilePayloadFromPath("test.pdf");

            apiEndPoint = "api/policies/attachFile/{id}";
            endpointParams = new Dictionary<string, string>() {
                { "id", "1" },
                { "something", "u2" },
                { "that", "take that" },
            };

            endpointData = new ExpandoObject();
            endpointData.file = testPayload;

            response = await ApiClientTools.Client.doPost(apiEndPoint, endpointParams, endpointData);
            return Json(response);
        }


        // // POST: Jokes/SearchResults
        // public async Task<IActionResult> SearchResults(String Search)
        // {
        //     string apiEndPoint;
        //     Dictionary<string, string> endpointParams;
        //     dynamic endpointData;

        //     apiEndPoint = "api/quote/setClientName";
        //     endpointParams = new Dictionary<string, string>() {
        //         { "id", "4" },
        //         { "something", "u2" },
        //         { "that", "take that" },
        //     };

        //     endpointData = new ExpandoObject();
        //     endpointData.quote_id = 45;
        //     endpointData.client_name = "do post client name";

        //     apiEndPoint = "api/regions/getResource";
        //     endpointParams = new Dictionary<string, string>() {
        //         // { "id", "4" },
        //         // { "something", "u2" },
        //         // { "that", "take that" },
        //     };
        //     endpointData = new Dictionary<string, string>() {
        //         // {"page", "1"},
        //         // {"perPage", "25"},
        //         // {"search", "Mircea&Jeandrew like cheese"},
        //     };


        //     var data = await ApiClientTools.Client.doGet(apiEndPoint, endpointParams, endpointData);
        //     return Json(data);

            
            
        //     //dynamic jsonResponse = JsonConvert.DeserializeObject(stringResponse);
        //     //dynamic jsonResponse = JArray.Parse(stringResponse);
        //     //dynamic jsonResponse = JObject.Parse(stringResponse);
            
        //     //Console.WriteLine(stringResponse);
        //     //Console.WriteLine(jsonResponse);
        //     //return Content(stringResponse);
        //     //return Json(jsonResponse);
        //     //}


        //     //Task<IActionResult> task = new Task<IActionResult>()
        //     //{
        //     //Console.WriteLine(Student);
        //     //return Json(Student);
        //     //};


        //     //return await Task.Run(() =>
        //     //{
        //     //var Test = new { Amount = 108, Message = "Hello" };
        //     //var Student = (s: Search, t: "asdasdsa");


        //     //Console.WriteLine(Student);
        //     //return Json(Student);
        //     //});

        //     //return Content(" I like cheese ");

        //     //return Json(new { search = Student, results = _context.Joke.Where(q => q.Question.Contains(Search)).ToList() });
        //     //return Json(new { search = Search, results = await _context.Joke.Where(q => q.Question.Contains(Search)).ToListAsync() });
        //     return View("SearchResults", ( Search, await _context.Joke.Where(q => q.Question.Contains(Search)).ToListAsync() ));
        // }

        // GET: Jokes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // GET: Jokes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Jokes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Question,Answer")] Joke joke)
        {
            if (ModelState.IsValid)
            {
                _context.Add(joke);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(joke);
        }

        // GET: Jokes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke.FindAsync(id);
            if (joke == null)
            {
                return NotFound();
            }
            return View(joke);
        }

        // POST: Jokes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Question,Answer")] Joke joke)
        {
            if (id != joke.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(joke);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JokeExists(joke.Id))
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
            return View(joke);
        }

        // GET: Jokes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var joke = await _context.Joke
                .FirstOrDefaultAsync(m => m.Id == id);
            if (joke == null)
            {
                return NotFound();
            }

            return View(joke);
        }

        // POST: Jokes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var joke = await _context.Joke.FindAsync(id);
            _context.Joke.Remove(joke);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JokeExists(int id)
        {
            return _context.Joke.Any(e => e.Id == id);
        }
    }
}
