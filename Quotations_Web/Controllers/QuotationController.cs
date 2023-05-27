using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;

namespace Quotations_Web.Controllers
{
    public class QuotationController : Controller
    {
        // GET: QuotationController
        public ActionResult Index(Response t)
        {
            return View(t);
        }

        // GET: QuotationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QuotationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QuotationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAsync(Parameter collection)
        {
            try
            {
                var token = await AuthenticateAsync();
                HttpClient client = new HttpClient();
                //client.BaseAddress = new Uri("https://localhost:44342/");
                client.BaseAddress = new Uri("https://localhost:44300");

                var myContent = JsonConvert.SerializeObject(collection);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                client.DefaultRequestHeaders.Authorization =
    new AuthenticationHeaderValue("Bearer", token);
                // Add an Accept header for JSON format.
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //var content = new FormUrlEncodedContent();
                // List all Names.

                HttpResponseMessage response = client.PostAsync("api/Quotation", byteContent).Result;


                var stream = await response.Content.ReadAsStreamAsync();
                Response t = await System.Text.Json.JsonSerializer.DeserializeAsync<Response>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
                
                return RedirectToAction("Index",t);
            }
            catch
            {
                return View();
            }
        }


        public async Task<string> AuthenticateAsync()
        {
            HttpClient client = new HttpClient();
            //client.BaseAddress = new Uri("https://localhost:44342/");
            client.BaseAddress = new Uri("https://localhost:44300");

            var myContent = JsonConvert.SerializeObject(new Users() { Name="user1",Password="password1"});
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            // Add an Accept header for JSON format.
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //var content = new FormUrlEncodedContent();
            // List all Names.

            HttpResponseMessage response = client.PostAsync("api/Quotation/authenticate", byteContent).Result;  // Blocking call!


            
            var stream = await response.Content.ReadAsStreamAsync();
            Tokens t = await System.Text.Json.JsonSerializer.DeserializeAsync<Tokens>(stream, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            return t.Token;
        }

        // POST: QuotationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: QuotationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QuotationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
