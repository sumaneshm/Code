using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace ExploreFluentAssertions.Business
{
    static class Utils
    {
        public static void Throws(Action action, string reason = "")
        {
            action.ShouldThrow<Exception>(reason);
        }
    }
}
