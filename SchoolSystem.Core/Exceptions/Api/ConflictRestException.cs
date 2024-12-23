using SchoolSystem.Core.Common.BaseClasses;
using Microsoft.AspNetCore.Http;

namespace SchoolSystem.Core.Exceptions.Api;


public class ConflictRestException(string message) : BaseRestException(StatusCodes.Status409Conflict, new { error = message })
{
}
