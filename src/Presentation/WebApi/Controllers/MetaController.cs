using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace WebApi.Controllers;

public class MetaController : BaseApiController
{
    [HttpGet("/info")]
    public ActionResult<string> Info()
    {
        System.Reflection.Assembly assembly = typeof(Startup).Assembly;

        System.DateTime lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
        string version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

        return Ok($"Version: {version}, Last Updated: {lastUpdate}");
    }
}