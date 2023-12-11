using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

const string host = "http://localhost:8181";
const string method = "/echo";
const string path = host + method;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
var app = builder.Build();
app.MapGet(method, (string input) => input);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
app.RunAsync(host);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

SendWebClient();
await SendWebRequest();
await SendHttpClient();

await app.StopAsync();
return;

string PathWithParameter(string input) => $"{path}?input={input}";

// WebClient (Legacy)
// Does not support HTTP/2.
// Lacks advanced features like progress reporting and direct stream handling.
void SendWebClient()
{
    const string webClientConst = "WebClient";
    using var client = new WebClient();
    var content = client.DownloadStringTaskAsync(PathWithParameter(webClientConst)).Result;
    Console.WriteLine(content);
}

// WebRequest (Legacy)
// Supports advanced features like cookies and headers.
// Synchronous by default.
// Does not support HTTP/2.
async Task SendWebRequest()
{
    const string webRequestConst = "WebRequest";
    var request = (HttpWebRequest)WebRequest.Create(PathWithParameter(webRequestConst));
    request.Method = "GET";
    using var response = (HttpWebResponse)await request.GetResponseAsync();
    await using var stream = response.GetResponseStream();
    using var reader = new StreamReader(stream);
    var content = reader.ReadToEnd();
    Console.WriteLine(content);
}

// HttpClient (Modern)
// Asynchronous by nature
// Supports HTTP/2.
// Reusable
async Task SendHttpClient()
{
    const string httpClientConst = "HttpClient";
    var client = new HttpClient();
    var content = await client.GetStringAsync(PathWithParameter(httpClientConst));
    Console.WriteLine(content);
}