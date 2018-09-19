using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            await Task.Delay(5000);

            if (HttpContext.RequestAborted.IsCancellationRequested)
            {
                // Won't ever fire even if the client disconnected during the delay
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CancellationToken token)
        {
            await ClientDisconnectGuard.ExecuteAsync(() => ReadDataFromClient(HttpContext), HttpContext.RequestAborted);

            if (token.IsCancellationRequested)
            {
                // Yep it worked if the client disconnected during reading data from the client
            }

            return View();
        }

        private static async Task ReadDataFromClient(HttpContext context)
        {
            var bytesRead = 0;
            do
            {
                if (context.RequestAborted.IsCancellationRequested)
                {
                    Logger.WriteLog(true);
                    break;
                }

                var buffer = new byte[1];
                bytesRead = await context.Request.Body.ReadAsync(buffer, 0, 1, context.RequestAborted);
            }
            while (bytesRead != 0);
        }
    }
}
