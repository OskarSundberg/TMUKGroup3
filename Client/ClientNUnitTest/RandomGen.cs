using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ClientNUnitTest
{
    internal class RandomGen
    {
        public Random Rand { get; set; }
        public RandomGen()
        {
            this.Rand = new Random(DateTime.Now.Millisecond);
        }
        public string Gen_String(int length)
        {
            string randomStr = string.Empty;
            for (int i = 0; i < length; i++)
            {
                randomStr.Append(Convert.ToChar(Gen_Int()));
            }
            return randomStr;
        }
        public int Gen_Int()
        {
            int random = Rand.Next(0, 127);
            return random;
        }


    }
}
