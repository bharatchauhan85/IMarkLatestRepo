using System;
namespace IMark.Core.Models.Result
{
    /// <summary>
    /// A type of result from a response.
    /// </summary>
    public enum ResultType
    {
        Ok,
        Invalid,
        Unauthorized,
        InternalError,
        PartialOk,
        NotFound,
        Unexpected
    }
}
