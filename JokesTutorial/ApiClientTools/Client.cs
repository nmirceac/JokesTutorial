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

namespace ApiClientTools
{
    public class Client
    {
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
    }
}
