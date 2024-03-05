using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class Result:IResult
    {
        

        public Result(bool Success, string message):this(Success)//İki constructoru birden çalıştırıyor. Constructorun Base'ler veya kendi içindeki yapılarla çalışmasına güzel bir örnek.
        {
            Message=message;
        }

        public Result(bool Success)//Overloading yaptık, mesajsız şekilde yaptık.
        {
            Success = Success;
        }

        public bool Success { get; }
        public string Message { get; }
    }
}
