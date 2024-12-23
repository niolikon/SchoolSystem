using SchoolSystem.Core.Common.BaseClasses;
using Microsoft.AspNetCore.Http;

namespace SchoolSystem.Core.Exceptions.Api;


public class MalformedModelRestException(string message) : BaseRestException(StatusCodes.Status400BadRequest, new { error = message })
{
}
