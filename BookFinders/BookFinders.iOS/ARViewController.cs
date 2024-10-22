using System;

using UIKit;
using ARKit;
using SceneKit;

namespace BookFinders.iOS
{
	public partial class ARViewController : UIViewController
	{
        private readonly ARSCNView sceneView;

        public ARViewController () : base ("ARViewController", null)
		{
        }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

            startAR();
        }

        public void startAR()
        {
            var sceneView = new ARSCNView();
            sceneView.Frame = View.Frame;
            View = sceneView;

            CreateARScene(sceneView);
            PositionScene(sceneView);
        }

        public void CreateARScene(ARSCNView sceneView)
        {
            var scene = SCNScene.FromFile("art.scnassest/arrow");
            sceneView.Scene = scene;

            sceneView.DebugOptions = ARSCNDebugOptions.ShowWorldOrigin
                | ARSCNDebugOptions.ShowFeaturePoints;
        }

        public void PositionScene(ARSCNView sceneView)
        {
            var arConfig = new ARWorldTrackingConfiguration
            {
                PlaneDetection = ARPlaneDetection.Horizontal,
                LightEstimationEnabled = true
            };

            sceneView.Session.Run(arConfig, ARSessionRunOptions.ResetTracking);

            var sceneNode = sceneView.Scene.RootNode.FindChildNode("arrow-001", true);
            sceneNode.Position = new SCNVector3(0, -25, -40);
            sceneNode.Scale = new SCNVector3(10, 10, 10);

            sceneView.Scene.RootNode.AddChildNode(sceneNode);
        }

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}
    }
}


