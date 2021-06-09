using Foundation;
using Function;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IFunctionHandler, FunctionHandler>();
builder.Services.AddHttpContextAccessor();

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Map("{*url}", (Func<IFunctionHandler, IHttpContextAccessor, Task<string>>)HandleRequestAsync);

await app.RunAsync();

async Task<string> HandleRequestAsync([FromServices] IFunctionHandler handler, [FromServices] IHttpContextAccessor httpContextAccessor)
{
    var request = httpContextAccessor?.HttpContext?.Request;
    return await handler.Handle(request);
}