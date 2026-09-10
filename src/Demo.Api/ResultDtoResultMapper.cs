using Demo.Application.Dtos;
using Demo.Domain.Enums;

namespace Demo.Api;

internal static class ResultDtoResultMapper
{
    internal static IResult ToHttpResult<T>(
        ResultDto<T> result,
        Func<T, IResult> onSuccess)
    {
        return result.ErrorCode switch
        {
            Error.None => onSuccess(result.Result),
            Error.ValidationError => Results.BadRequest(result.ErrorMessages),
            Error.Conflict => Results.Conflict(result.ErrorMessages),
            Error.NotFound => Results.NotFound(result.ErrorMessages),
            _ => Results.InternalServerError(result.ErrorMessages)
        };
    }
}
