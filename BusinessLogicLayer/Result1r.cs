using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Result<T> : Result
    {
        public T? Data { get; set; }

        public static Result<T> SuccessResult(T data, string message = "")
            => new Result<T> { Success = true, Data = data, Message = message };

        public static new Result<T> Failure(string message)
            => new Result<T> { Success = false, Message = message };
    }
}
