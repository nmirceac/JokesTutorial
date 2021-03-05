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

        // POST: Jokes/SearchResults
        public async Task<IActionResult> SearchResults(String Search)
        {
            string apiBase, apiKey, apiEndPoint;
            bool apiDebug;
            Uri apiUri;
            HttpRequestMessage request;

            apiBase = Environment.GetEnvironmentVariable("API_CLIENT_BASE_URL");
            apiKey = Environment.GetEnvironmentVariable("API_CLIENT_SECRET");
            apiDebug = Convert.ToBoolean(Environment.GetEnvironmentVariable("API_CLIENT_DEBUG"));

            apiEndPoint = "/api/users/getAuthData/1212312321421";
            apiEndPoint = "/api/leads/getForIntermediary/4";
            apiEndPoint = "/api/quote/setClientName";
            apiUri = new Uri(apiBase + apiEndPoint);

            if(true) {
                request = new HttpRequestMessage(HttpMethod.Post, apiUri);
            } else {
                request = new HttpRequestMessage(HttpMethod.Get, apiUri);
            }

            
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("X-Api-Key", apiKey);


            var client = new HttpClient();

            var response = await client.SendAsync(request);

            var SuccessfulResponseGet = new { success = false, data = new ExpandoObject() };
            var SuccessfulResponsePost = new { success = false, data = new ExpandoObject(), ack = false };
            var UnsuccessfulResponse = new { success = false, data = new ExpandoObject(), message="" };

            var stringResponse = await response.Content.ReadAsStringAsync();

            dynamic jsonResponse;

            if(response.IsSuccessStatusCode)
            {
                jsonResponse = JsonConvert.DeserializeAnonymousType(stringResponse, SuccessfulResponseGet);
            } else {
                jsonResponse = JsonConvert.DeserializeAnonymousType(stringResponse, UnsuccessfulResponse);
            }
            
            
            //dynamic jsonResponse = JsonConvert.DeserializeObject(stringResponse);
            //dynamic jsonResponse = JArray.Parse(stringResponse);
            //dynamic jsonResponse = JObject.Parse(stringResponse);
            
            //Console.WriteLine(stringResponse);
            //Console.WriteLine(jsonResponse);
            //return Content(stringResponse);
            return Json(jsonResponse);
            //}


            //Task<IActionResult> task = new Task<IActionResult>()
            //{
            //Console.WriteLine(Student);
            //return Json(Student);
            //};


            //return await Task.Run(() =>
            //{
            //var Test = new { Amount = 108, Message = "Hello" };
            //var Student = (s: Search, t: "asdasdsa");


            //Console.WriteLine(Student);
            //return Json(Student);
            //});

            //return Content(" I like cheese ");

            //return Json(new { search = Student, results = _context.Joke.Where(q => q.Question.Contains(Search)).ToList() });
            //return Json(new { search = Search, results = await _context.Joke.Where(q => q.Question.Contains(Search)).ToListAsync() });
            return View("SearchResults", ( Search, await _context.Joke.Where(q => q.Question.Contains(Search)).ToListAsync() ));
        }

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
