using System.Runtime.CompilerServices;
using ShortUrlService.AsyncDataServices;

namespace ShortUrlService.Helper.Counter
{
    public static class Counter
    {
        public static int CurrentNumber { get; set; } = 1;
        public static int Limit { get; set; } = 100;

        public async static Task SetCounterRange(ICounterRangeRpcClient counterRangeRpcClient)
        {
            var counterRangeResponse = await counterRangeRpcClient.GetNextCounterRange();
            CurrentNumber = counterRangeResponse.Start;
            Limit = counterRangeResponse.Max;
        }

        public async static Task IncrementCounter(ICounterRangeRpcClient counterRangeRpcClient)
        {
            CurrentNumber++;
            if (CurrentNumber > Limit) await SetCounterRange(counterRangeRpcClient);
        }
    }
}