namespace BookFindersAPI.Controllers
{
    public class ControllerStartUpTracker
    {
        private static ControllerStartUpTracker self = null;
        private volatile bool _isInitialRunOfControllers = true;

        public static ControllerStartUpTracker GetInstance()
        {
            self ??= new ControllerStartUpTracker();
            return self;
        }

        private ControllerStartUpTracker()
        {
            // Do nothing
        }

        public bool IsInitialRunOfControllers()
        {
            return self._isInitialRunOfControllers;
        }

        public void SetIsInitialRunOfControllers(bool isInitialRunOfControllers)
        {
            self._isInitialRunOfControllers = isInitialRunOfControllers;
        }
    }
}
