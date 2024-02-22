using System.Net;
using System.Text;

namespace WebServerHB;

public static class WebHelper
{
    // private static readonly DatabaseManager DatabaseManager = new();

    public static async Task ShowIndex(HttpListenerContext context, CancellationToken ctx)
    {
        await ShowFile(@"..\..\..\..\test-ex\index.html", context, ctx);
    }

    public static async Task ShowAdd(HttpListenerContext context, CancellationToken ctx)
    {
        await ShowFile(@"..\..\..\..\test-ex\add.html", context, ctx);
    }

    public static async Task ShowList(HttpListenerContext context, CancellationToken ctx)
    {
        await ShowFile(@"..\..\..\..\test-ex\list.html", context, ctx);
    }

    private static async Task ShowFile(string path, HttpListenerContext context, CancellationToken ctx)
    {
        context.Response.StatusCode = 200;
        context.Response.ContentType = Path.GetExtension(path) switch
        {
            ".js" => "application/javascript",
            ".css" => "text/css",
            ".html" => "text/html",
            ".svg" => "image/svg+xml",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            _ => "text/plain"
        };

        var file = await File.ReadAllBytesAsync(path, ctx);
        await context.Response.OutputStream.WriteAsync(file, ctx);
    }

    public static async Task ShowStatic(HttpListenerContext context, CancellationToken ctx)
    {
        var path = context.Request.Url?.LocalPath
            .Split("/")
            .Skip(1)
            .ToArray();
        var basePath = Path.Combine(Directory.GetCurrentDirectory(), @"..\..\..\..\SemestrWork");

        if (path != null)
        {
            for (var i = 0; i < path.Length - 1; i++)
            {
                basePath = Path.Combine(basePath, $@"{path[i]}\");
            }
        }

        basePath = Path.Combine(basePath, path?[^1] ?? string.Empty);

        if (File.Exists(basePath))
        {
            await ShowFile(basePath, context, ctx);
        }
        else
        {
            await Show404(context, ctx);
        }
    }

    private static async Task Show404(HttpListenerContext context, CancellationToken ctx)
    {
        context.Response.ContentType = "text/plain; charset=utf-8";
        context.Response.StatusCode = 404;
        await context.Response.OutputStream.WriteAsync(Encoding.UTF8.GetBytes("Нужная вам страница не найдена!"), ctx);
    }
}
