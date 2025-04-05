using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Moq;
using personal_ai.Middleware;
using Xunit;

public class JsonExceptionHandlingMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_MalformedJson_ReturnsBadRequest()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var mockNext = new Mock<RequestDelegate>();
        var middleware = new JsonExceptionHandlingMiddleware(mockNext.Object);
        httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{invalid json}"));
        httpContext.Request.ContentType = "application/json";

        // Act
        await middleware.InvokeAsync(httpContext);

        // Assert
        Assert.Equal(400, httpContext.Response.StatusCode);
        // Add assertions to check the response body's content.
    }

    [Fact]
    public async Task InvokeAsync_ValidJson_CallsNextMiddleware()
    {
        //Arrange
        var httpContext = new DefaultHttpContext();
        var mockNext = new Mock<RequestDelegate>();
        var middleware = new JsonExceptionHandlingMiddleware(mockNext.Object);
        httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes("{\"test\":\"test\"}"));
        httpContext.Request.ContentType = "application/json";

        //Act
        await middleware.InvokeAsync(httpContext);

        //Assert
        mockNext.Verify(next => next(httpContext), Times.Once);
    }

    // Add more test cases for other scenarios (e.g., invalid enum values).
}
