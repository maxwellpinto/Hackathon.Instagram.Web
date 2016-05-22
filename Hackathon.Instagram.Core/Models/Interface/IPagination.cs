using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackathon.Instagram.Core.Models.Interface
{
    public interface IPagination<T>
    {
        Pagination Pagination { get; set; }
        List<T> Data { get; set; }
    }
}
