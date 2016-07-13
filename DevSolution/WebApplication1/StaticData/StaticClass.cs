using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.StaticData
{
    public static class StaticClass
    {
        public static Dictionary<string, int> dict = new Dictionary<string, int>() {
            {"h", 1 }
        };
    }
}