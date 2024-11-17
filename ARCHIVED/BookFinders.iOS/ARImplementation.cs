using System;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(BookFinders.iOS.ARImplementation))]

namespace BookFinders.iOS
{
	public class ARImplementation : IARImplmentation
	{
		public ARImplementation()
		{
		}
        public void LaunchAR()
        {
            ARViewController viewController = new ARViewController();
            UIApplication.SharedApplication.KeyWindow.RootViewController.
            PresentViewController(viewController, true, null);
        }
    }
}

