using MainLibrary.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainLibrary.Services
{
    public class TestService
    {
        public int DoSomething()
        {
            try
            {
               TestRepo repo = new TestRepo();
               return repo.TestOperation();


                //string[] colors = new string[] { "Red", "Green", "Blue" };
                //string color = colors[5];

                //return 0;

            } 
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
