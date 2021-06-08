using Function;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<FunctionHandler>();

await using var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Map("{*url}", (Func<string, FunctionHandler, string>)(HandleRequest));

await app.RunAsync();

string HandleRequest([FromQuery] string input, [FromServices] FunctionHandler handler)
{
    return handler.Handle(input);
}