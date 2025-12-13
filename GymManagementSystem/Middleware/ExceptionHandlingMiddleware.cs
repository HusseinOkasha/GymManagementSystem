using GymManagementSystem.Services.Exceptions;

namespace GymManagementSystem.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (EntityNotFoundException ex)
        {
            _logger.LogWarning(ex, "Entity not found");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (UserCreationFailureException ex)
        {
            _logger.LogError(ex, "Failed to create user");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsJsonAsync(ex.identityResult);
        }
        catch (AssigningUserRoleException ex)
        {
            _logger.LogError(ex, "Failed Assign role to user");
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            await context.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (UnauthorizedException ex)
        {
            _logger.LogError(ex, "Failed login");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
        }
    }
}