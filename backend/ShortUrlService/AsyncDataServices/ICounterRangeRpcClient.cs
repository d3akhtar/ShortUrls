using ShortUrlService.Helper.Counter;

namespace ShortUrlService.AsyncDataServices
{
    public interface ICounterRangeRpcClient
    {
        Task<CounterRange> GetNextCounterRange();
    }
}