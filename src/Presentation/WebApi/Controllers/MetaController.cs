using Microsoft.AspNetCore.Mvc;

using System;
using System.Diagnostics;
using System.Reflection;

namespace WebApi.Controllers;

public class MetaController : BaseApiController
{
    [HttpGet("/info")]
    public ActionResult<string> Info()
    {
        Assembly assembly = typeof(Startup).Assembly;

        DateTime lastUpdate = System.IO.File.GetLastWriteTime(assembly.Location);
        string version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

        return Ok($"Version: {version}, Last Updated: {lastUpdate}");
    }
}