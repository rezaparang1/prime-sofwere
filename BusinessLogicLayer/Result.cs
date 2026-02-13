using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    // ========== Result غیر جنریک (بدون داده) ==========
    public class Result
    {
        public bool IsSuccess { get; }
        public string Message { get; }
        public int? StatusCode { get; }

        protected Result(bool isSuccess, string message, int? statusCode = null)
        {
            IsSuccess = isSuccess;
            Message = message;
            StatusCode = statusCode;
        }

        public static Result Success(string message = "عملیات با موفقیت انجام شد")
            => new Result(true, message);

        public static Result Failure(string message, int? statusCode = 400)
            => new Result(false, message, statusCode);
    }

    // ========== Result جنریک (همراه با داده) ==========
    public class Result<T> : Result
    {
        public T? Data { get; }

        private Result(bool isSuccess, T? data, string message, int? statusCode = null)
            : base(isSuccess, message, statusCode)
        {
            Data = data;
        }

        public static Result<T> Success(T data, string message = "عملیات با موفقیت انجام شد")
            => new Result<T>(true, data, message);

        public static new Result<T> Failure(string message, int? statusCode = 400)
            => new Result<T>(false, default, message, statusCode);
    }
}
