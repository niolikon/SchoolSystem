using SchoolSystem.Core.Common.BaseClasses;
using Microsoft.AspNetCore.Http;

namespace SchoolSystem.Core.Exceptions.Api;


public class NotFoundRestException(string message) : BaseRestException(StatusCodes.Status404NotFound, new { error = message })
{
}
