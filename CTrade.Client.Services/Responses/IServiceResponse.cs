
namespace CTrade.Client.Services.Responses
{
    public interface IServiceResponse
    {
        bool HasError { get; }
        string Error { get; }
    }
}
