using System;

using UIKit;
using ARKit;

namespace BookFinders.iOS
{
	public partial class ARViewController : UIViewController
	{
        private readonly ARSCNView sceneView;

        public ARViewController () : base ("ARViewController", null)
		{
            this.sceneView = new ARSCNView
            {
                AutoenablesDefaultLighting = true,
                DebugOptions = ARSCNDebugOptions.ShowFeaturePoints
            };

            this.View.AddSubview(this.sceneView);
        }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            this.sceneView.Frame = this.View.Frame;
        }

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            this.sceneView.Session.Run(new ARWorldTrackingConfiguration
            {
                AutoFocusEnabled = true,
                LightEstimationEnabled = true,
                WorldAlignment = ARWorldAlignment.Gravity
            }, ARSessionRunOptions.ResetTracking | ARSessionRunOptions.RemoveExistingAnchors);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            this.sceneView.Session.Pause();
        }
    }
}


