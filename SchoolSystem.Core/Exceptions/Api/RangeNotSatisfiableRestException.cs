using SchoolSystem.Core.Common.BaseClasses;
using Microsoft.AspNetCore.Http;

namespace SchoolSystem.Core.Exceptions.Api;


public class RangeNotSatisfiableRestException(string message) : BaseRestException(StatusCodes.Status416RangeNotSatisfiable, new { error = message })
{
}
