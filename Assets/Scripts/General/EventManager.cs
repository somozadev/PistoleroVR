namespace General
{
    public static class EventManager
    {
        public delegate void OnLoadingEndsEventHandler();

        public static event OnLoadingEndsEventHandler LoadingEnds;

        public static void OnLoadingEnds()
        {
            LoadingEnds?.Invoke();
        }
    }
}