using System;
using System.Windows.Media.Media3D;

namespace SSCOControls
{
    public class Wall : LayoutBase
    {
        protected override Motion GetBeforeMotion(int deltaIndex, ElementFlow.NavigationDirection direction)
        {
            double ItemWidth = Owner.ElementWidth / Owner.ElementHeight;
            Motion m = new Motion();
            m.Axis = new Vector3D(1, 0, 0);
            m.Angle = 0;

            int column = 2 - Math.Abs(ElementFlow.NavigationDirection.Left == direction ? deltaIndex + 1 : deltaIndex - 1);
            m.FromX = (column - 2) * (ItemWidth + Owner.ItemGap);
            m.FromY = 0;
            bool fromSelection = ElementFlow.NavigationDirection.Left == direction  && -1 == deltaIndex;
            m.FromZ = fromSelection ? 0 : - Owner.PopoutDistance;
            m.FromOpacity = fromSelection ? 1 : 0.4;

            column = 2 - Math.Abs(deltaIndex);
            m.ToX = (column - 2) * (ItemWidth + Owner.ItemGap);
            m.ToY = 0;
            m.ToZ = -Owner.PopoutDistance;
            m.ToOpacity = 0.4;
            
            return m;
        }

        protected override Motion GetAfterMotion(int deltaIndex, ElementFlow.NavigationDirection direction)
        {
            double ItemWidth = Owner.ElementWidth / Owner.ElementHeight;
            Motion m = new Motion();
            m.Axis = new Vector3D(1, 0, 0);
            m.Angle = 0;

            int column = 2 + (ElementFlow.NavigationDirection.Left == direction ? deltaIndex + 1 : deltaIndex - 1);
            m.FromX = (column - 2) * (ItemWidth + Owner.ItemGap);
            m.FromY = 0;
            bool fromSelection = ElementFlow.NavigationDirection.Right == direction && 1 == deltaIndex;
            m.FromZ = fromSelection ? 0 : -Owner.PopoutDistance;
            m.FromOpacity = fromSelection ? 1 : 0.4;

            column = 2 + deltaIndex;
            m.ToX = (column - 2) * (ItemWidth + Owner.ItemGap);
            m.ToY = 0;
            m.ToZ = -Owner.PopoutDistance;
            m.ToOpacity = 0.4;
                        
            return m;
        }

        protected override Motion GetSelectionMotion(ElementFlow.NavigationDirection direction)
        {
            double ItemWidth = Owner.ElementWidth / Owner.ElementHeight;
            Motion m = new Motion();
            m.Axis = new Vector3D(1, 0, 0);
            m.Angle = 0;

            int column = ElementFlow.NavigationDirection.Left == direction ? 3 : 1;
            m.FromX = (column - 2) * (ItemWidth + Owner.ItemGap);
            m.FromY = 0;
            m.FromZ = -Owner.PopoutDistance;
            m.FromOpacity = 0.4;

            m.ToX = 0;
            m.ToY = 0;
            m.ToZ = 0;
            m.ToOpacity = 1;
            
            return m;
        }

        protected override Motion GetSelectionChangeMotion()
        {
            Motion m = new Motion();
            m.Axis = new Vector3D(1, 0, 0);
            m.Angle = 90;

            m.FromX = 0;
            m.FromY = 0;
            m.FromZ = 0;
            m.FromOpacity = 0;
            
            m.ToX = 0;
            m.ToY = 0;
            m.ToZ = 0;
            m.ToOpacity = 1;
            
            return m;
        }

        protected override Motion GetAddedMotion()
        {
            double ItemWidth = Owner.ElementWidth / Owner.ElementHeight;
            Motion m = new Motion();
            m.Axis = new Vector3D(1, 0, 0);
            m.Angle = 0;

            int column = 3;
            m.FromX = (column - 2) * (ItemWidth + Owner.ItemGap);
            m.FromY = 0;
            m.FromZ = 0;
            m.FromOpacity = 1;
            
            m.ToX = 0;
            m.ToY = 0;
            m.ToZ = 0;
            m.ToOpacity = 1;
                        
            return m; 
        }
    }
}