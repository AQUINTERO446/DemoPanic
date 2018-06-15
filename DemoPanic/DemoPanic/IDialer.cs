using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DemoPanic
{
    public interface IDialer
    {
        Task<bool> DialAsync(string number);
    }
}
