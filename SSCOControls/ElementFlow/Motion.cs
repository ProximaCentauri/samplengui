using System.Windows.Media.Media3D;

namespace SSCOControls
{
	public struct Motion
	{
        public Vector3D Axis { get; set; }
        public double Angle { get; set; }
        public double FromX { get; set; }
        public double FromY { get; set; }
        public double FromZ { get; set; }
        public double FromOpacity { get; set; }        
        public double ToX { get; set; }
		public double ToY { get; set; }
		public double ToZ { get; set; }
        public double ToOpacity { get; set; }        
	}
}