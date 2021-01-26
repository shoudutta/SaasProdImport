using SaasProdImport.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SaasProdImport.UnitTests
{
    public class TestBase
    {
        public static SaasContext context;

        public TestBase()
        {
            context = new SaasContext();
        }
    }
}
