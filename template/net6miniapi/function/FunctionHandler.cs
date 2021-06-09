using Foundation;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Function
{
    public class FunctionHandler : IFunctionHandler
    {
        public async Task<string> Handle(HttpRequest request)
        {
            var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();
            return $"Hi there - Method: {request.Method}\n Path: {request.Path}\n QueryString: {request.QueryString}\n Body: {body}";
        }
    }
}
