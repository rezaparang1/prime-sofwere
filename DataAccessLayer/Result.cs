using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public int? StatusCode { get; }

        private Result(bool isSuccess, string message, int? statusCode = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = statusCode;
        }

        public static Result Success(string message)
            => new Result(true, message);

        public static Result Failure(string message, int? statusCode = 400)
            => new Result(false, message, statusCode);
    }
}
