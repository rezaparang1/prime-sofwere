using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class Result
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;

        public static Result SuccessResult(string message = "")
            => new Result { Success = true, Message = message };

        public static Result Failure(string message)
            => new Result { Success = false, Message = message };
    }
}
