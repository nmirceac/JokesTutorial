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
using JokesTutorial.Libraries;
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

        public static string getApiBaseUrl()
        {
            var apiBaseUrl = Environment.GetEnvironmentVariable("API_CLIENT_BASE_URL").TrimEnd(new char[] {'/'});
    
            return apiBaseUrl;
        }

        public static string buildUrl(string endpoint, Dictionary<string, string> endpointParams, Dictionary<string, string> endpointData)
        {
            var endpointUrl = endpoint;

            foreach (Match match in Regex.Matches(endpoint, @"{(.*?)}")) {
                var matchedParam = match.Value.TrimStart('{').TrimEnd('}');
                if(endpointParams.ContainsKey(matchedParam)) {
                    endpointUrl = endpointUrl.Replace(match.Value, HttpUtility.UrlEncode(endpointParams[matchedParam]));
                }
            }

            if(endpointData.Count>0) {
                List<string> dataParts = new List<string>();
                foreach(KeyValuePair<string, string> part in endpointData) {
                    dataParts.Add(HttpUtility.UrlEncode(part.Key)+"="+HttpUtility.UrlEncode(part.Value));
                }
                endpointUrl += "?"+String.Join("&", dataParts.ToArray());
            }
            
            return getApiBaseUrl() + '/' + endpointUrl.TrimStart(new char[] {'/'});
        }

        public static HttpRequestMessage prepareRequest(HttpMethod method, string url)
        {
            HttpRequestMessage request;
            string apiKey;

            request = new HttpRequestMessage(method, url);
            apiKey = Environment.GetEnvironmentVariable("API_CLIENT_SECRET");

            request.Headers.Add("Accept", "application/json");
            request.Headers.Add("X-Api-Key", apiKey);

            return request;
        }

        public static async Task<ExpandoObject> doGet(string endpoint, Dictionary<string, string> endpointParams, Dictionary<string, string> endpointData)
        {
            //apiDebug = Convert.ToBoolean(Environment.GetEnvironmentVariable("API_CLIENT_DEBUG"));

            string endpointUrl;
            HttpRequestMessage request;
            HttpClient client;

            endpointUrl = buildUrl(endpoint, endpointParams, endpointData);
            request = prepareRequest(HttpMethod.Get, endpointUrl);
            client = new HttpClient();

            var response = await client.SendAsync(request);

            
            var stringResponse = await response.Content.ReadAsStringAsync();
            return processResponse(HttpMethod.Get, endpointUrl, stringResponse, response.IsSuccessStatusCode);
        }

        public static async Task<ExpandoObject> doPost(string endpoint, Dictionary<string, string> endpointParams, ExpandoObject data)
        {
            //apiDebug = Convert.ToBoolean(Environment.GetEnvironmentVariable("API_CLIENT_DEBUG"));

            string endpointUrl;
            HttpRequestMessage request;
            HttpClient client;

            endpointUrl = buildUrl(endpoint, endpointParams, new Dictionary<string, string> {});
            request = prepareRequest(HttpMethod.Post, endpointUrl);

            var jsonPayload = JsonConvert.SerializeObject(data);
            var requestContent = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            request.Content = requestContent;

            client = new HttpClient();

            var response = await client.SendAsync(request);

            
            var stringResponse = await response.Content.ReadAsStringAsync();
            return processResponse(HttpMethod.Post, endpointUrl, stringResponse, response.IsSuccessStatusCode);
        }

        public static ExpandoObject processResponse(HttpMethod method, string endpointUrl, string stringResponse, bool isSuccessful)
        {
            dynamic jsonResponse;
            dynamic SuccessfulResponse;

            if(method == HttpMethod.Get) {
                SuccessfulResponse = new { success = false, data = new ExpandoObject() };
            } else {
                SuccessfulResponse = new { success = false, data = new ExpandoObject(), ack = false };
            }
            
            
            var UnsuccessfulResponse = new { success = false, data = new ExpandoObject(), message="" };


            if(isSuccessful)
            {
                try {
                    jsonResponse = JsonConvert.DeserializeAnonymousType(stringResponse, SuccessfulResponse);
                } catch (JsonReaderException ex) {
                    throw new ArgumentException("There was a problem parsing the JSON", stringResponse, ex);
                }
                
            } else {
                jsonResponse = JsonConvert.DeserializeAnonymousType(stringResponse, UnsuccessfulResponse);
                throw new ArgumentException(jsonResponse.message, stringResponse);
            }

            return jsonResponse.data;
        }

        public static ExpandoObject getFilePayloadFromPath(string filePath, string role = "files", int order = 0)
        {
            dynamic fileData;

            if(!System.IO.File.Exists(filePath)) {
                throw new ArgumentException("Couldn't find a file at path", filePath);
            }

            

            fileData = new ExpandoObject();

            FileInfo fileInfo;
            string fileMime;

            new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider().TryGetContentType(filePath, out fileMime);
            fileInfo = new FileInfo(filePath);
            

            String filePayload = Convert.ToBase64String(System.IO.File.ReadAllBytes(filePath));

            fileData.mime = fileMime;
            fileData.name = Path.GetFileNameWithoutExtension(filePath);
            fileData.extension = Path.GetExtension(filePath).TrimStart('.');

            fileData.basename = fileInfo.Name;
            fileData.size = fileInfo.Length;
            fileData.lastModified = System.IO.File.GetLastWriteTimeUtc(filePath).ToString("yyyy-MM-dd HH:mm:ss");
            fileData.originalPath = Path.GetFullPath(filePath);

            fileData.role = role;
            fileData.order = order;

            fileData.content = filePayload;

            return fileData;
        }

        // GET: Jokes/Test
        public IActionResult Test()
        {
            dynamic testPayload = getFilePayloadFromPath("test.pdf");

            string apiEndPoint;
            Dictionary<string, string> endpointParams;
            dynamic endpointData;

            apiEndPoint = "api/policies/attachFile/{id}";
            endpointParams = new Dictionary<string, string>() {
                { "id", "1" },
                { "something", "u2" },
                { "that", "take that" },
            };

            dynamic fileData = new ExpandoObject();


            endpointData = new ExpandoObject();
            // endpointData.file = fileData;
            endpointData.file = testPayload;

            var data = doPost(apiEndPoint, endpointParams, endpointData);
            return Json(data.Result);
        }


        // POST: Jokes/SearchResults
        public async Task<IActionResult> SearchResults(String Search)
        {
            string apiEndPoint;
            Dictionary<string, string> endpointParams;
            dynamic endpointData;

            apiEndPoint = "api/quote/setClientName";
            endpointParams = new Dictionary<string, string>() {
                { "id", "4" },
                { "something", "u2" },
                { "that", "take that" },
            };

            endpointData = new ExpandoObject();
            endpointData.quote_id = 45;
            endpointData.client_name = "do post client name";


            var data = await doPost(apiEndPoint, endpointParams, endpointData);
            return Json(data);

            apiEndPoint = "api/users/getAuthData/{id}";
            endpointParams = new Dictionary<string, string>() {
                { "id", "4" },
                { "something", "u2" },
                { "that", "take that" },
            };
            endpointData = new Dictionary<string, string>() {
                {"page", "1"},
                {"perPage", "25"},
                {"search", "Mircea&Jeandrew like cheese"},
            };


            //var data = await doGet(apiEndPoint, endpointParams, endpointData);
            return Json(data);

            
            
            //dynamic jsonResponse = JsonConvert.DeserializeObject(stringResponse);
            //dynamic jsonResponse = JArray.Parse(stringResponse);
            //dynamic jsonResponse = JObject.Parse(stringResponse);
            
            //Console.WriteLine(stringResponse);
            //Console.WriteLine(jsonResponse);
            //return Content(stringResponse);
            //return Json(jsonResponse);
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
