using System;
using System.Collections.Generic;
using System.Text;

namespace Fakelaki.Api.Lib.Services.Interfaces
{
    public interface IQRCoderService
    {
        string Create(string code, string logo = null);
    }
}
