using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradingAPI.Business.Shared;

public class CustomException : Exception
{
    public int StatusCode { get; set; }

    public CustomException(int statusCode, string message) : base(message)
    {
        StatusCode = statusCode;
    }

    public static CustomException NotFoundException(string message = "Not Found")
    {
        return new CustomException(404, message);
    }

    public static CustomException InvalidResourceException(string message = "Invalid data input")
    {
        throw new CustomException(400, message);
    }
    public static CustomException UnauthorizedException(string message = "Unauthorized")
    {
        throw new CustomException(401, message);
    }
}
