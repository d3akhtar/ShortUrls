namespace ShortUrlService.Helper.Counter
{
    public static class Counter
    {
        public static int CurrentNumber { get; set; } = 1;
        public static int Limit { get; set; } = 100;

        public static void IncrementCounter()
        {
            CurrentNumber++;
            if (CurrentNumber >= Limit)
            {
                Limit += 100; // later on, we will reset current nuber and limit based on message queue
            }
        }
    }
}