using SchoolSystem.Core.Common.BaseClasses;
using Microsoft.AspNetCore.Http;

namespace SchoolSystem.Core.Exceptions.Api;


public class InternalServerErrorRestException(string message) : BaseRestException(StatusCodes.Status500InternalServerError, new { error = message })
{
}
