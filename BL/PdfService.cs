using MongoDB.Driver.Core.Operations;
using PuppeteerSharp;
using System;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

namespace hameluna_server.BL
{

    public class PdfService
    {
        
        public static async Task<string> GeneratePdfAsync(string html, Dog dog)
        {

            try
            {
                string path = System.IO.Directory.GetCurrentDirectory();


                //check for shelters diractory if noe exists create new one with shleter id
                string shelterDir = Path.Combine(path, "uploadedFiles");
                if (!Directory.Exists(shelterDir))
                {
                    Directory.CreateDirectory(shelterDir);
                }
                await new BrowserFetcher().DownloadAsync();
                using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
                using var page = await browser.NewPageAsync();
                await page.SetContentAsync(html);

                string filePath = $"{shelterDir}/{dog.ShelterNumber}/{dog.NumberId}_{dog.Name}_אימוץ.pdf";

                await page.PdfAsync(filePath, new PdfOptions()
                {
                    Width = "800px",
                    Height = "450px"
                });

                return filePath;
            }
            catch (Exception e)
            {
                DBservices.WriteToErrorLog(e);
                return e.Message;
            }
        }

      
    }
}
