namespace General
{
    public static class EventManager
    {
        public delegate void OnLoadingStartsEventHandler();
        public delegate void OnLoadingEndsEventHandler();

        public static event OnLoadingEndsEventHandler LoadingEnds;
        public static event OnLoadingStartsEventHandler LoadingStarts;

        public static void OnLoadingEnds()
        {
            LoadingEnds?.Invoke();
        }
        public static void OnLoadingStarts()
        {
            LoadingStarts?.Invoke();
        }
    }
}