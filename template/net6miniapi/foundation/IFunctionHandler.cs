using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation
{
    public interface IFunctionHandler
    {
        Task<string> Handle(HttpRequest input);
    }
}
