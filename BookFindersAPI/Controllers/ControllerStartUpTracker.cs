namespace BookFindersAPI.Controllers
{
    public class ControllerStartUpTracker
    {
        private static ControllerStartUpTracker self = null;
        private volatile bool _isInitialRunPushNotificationController = true;

        public static ControllerStartUpTracker GetInstance()
        {
            self ??= new ControllerStartUpTracker();
            return self;
        }

        private ControllerStartUpTracker()
        {
            // Do nothing
        }

        public bool IsInitialRunPushNotificationController()
        {
            return self._isInitialRunPushNotificationController;
        }

        public void SetIsInitialRunPushNotificationController(bool isInitialRunPushNotificationController)
        {
            self._isInitialRunPushNotificationController = isInitialRunPushNotificationController;
        }
    }
}
