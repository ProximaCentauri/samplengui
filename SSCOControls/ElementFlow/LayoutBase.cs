using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;
using System.Windows.Media;
using System.Windows;

namespace SSCOControls
{
	public abstract class LayoutBase
	{
        public void SelectElement(int selectionIndex, bool isAdded, bool isNavigatingLeft, bool isNavigatingRight)
		{
            bool isSelectionChange = !isAdded && !isNavigatingLeft && !isNavigatingRight;
            ModelUIElement3D model;
            int modelIndex = 0;
            for (int i = 0; i < Owner.GetContainerCount(); i++)
            {
                model = Owner.ModelContainer.Children[i] as ModelUIElement3D;
                if (Visibility.Visible == model.Visibility)
                {
                    modelIndex = Owner.GetIndexFromModel(model);
                    if ((modelIndex < selectionIndex - 2) || (modelIndex > selectionIndex + 1) || (!isAdded && modelIndex == (selectionIndex - 2)))
                    {
                        Owner.RemoveModel(model);
                    }
                }
            }
            if (selectionIndex > 0)
            {
                if (isAdded && (selectionIndex > 1))
                {
                    Animate(selectionIndex - 2, GetBeforeMotion(-2, isAdded || isNavigatingLeft ?
                        ElementFlow.NavigationDirection.Left : ElementFlow.NavigationDirection.Right), true);
                }
                Animate(selectionIndex - 1, GetBeforeMotion(-1, isAdded || isNavigatingLeft ?
                    ElementFlow.NavigationDirection.Left : ElementFlow.NavigationDirection.Right), !isSelectionChange);
            }
            Motion motion = GetSelectionMotion(isNavigatingLeft ? ElementFlow.NavigationDirection.Left : ElementFlow.NavigationDirection.Right);
            if (isAdded)
            {
                motion = GetAddedMotion();                
            }
            else if (isSelectionChange)
            {
                motion = GetSelectionChangeMotion();
            }
            Animate(selectionIndex, motion, true);
            if ((selectionIndex + 1) < Owner.Items.Count)
            {
                Animate(selectionIndex + 1, GetAfterMotion(1, isAdded || isNavigatingLeft ?
                    ElementFlow.NavigationDirection.Left : ElementFlow.NavigationDirection.Right), !isSelectionChange);
            }           
		}

        public ElementFlow Owner { get; internal set; }

        private void Animate(int index, Motion motion, bool transition)
        {
            ModelUIElement3D model = Owner.GetModel(index);
            if (null != model)
            {
                Storyboard storyBoard = this.Owner.PrepareTemplateStoryboard(Owner.ModelContainer.Children.IndexOf(model));
                PrepareStoryboard(storyBoard, motion, transition);
                storyBoard.Begin(this.Owner.Viewport);                                
            }
        }
        
        private void PrepareStoryboard(Storyboard sb, Motion motion, bool transition)
		{
            AxisAngleRotation3D rotation = (sb.Children[0] as Rotation3DAnimation).From as AxisAngleRotation3D;
            DoubleAnimation xAnim = sb.Children[1] as DoubleAnimation;
            DoubleAnimation yAnim = sb.Children[2] as DoubleAnimation;
            DoubleAnimation zAnim = sb.Children[3] as DoubleAnimation;
            DoubleAnimation oAnim = sb.Children[4] as DoubleAnimation;
            
            rotation.Axis = motion.Axis;
            rotation.Angle = motion.Angle;

            xAnim.To = motion.ToX;
            yAnim.To = motion.ToY;
            zAnim.To = motion.ToZ;
            oAnim.To = motion.ToOpacity;            
            
            if (transition)
            {
                xAnim.From = motion.FromX;
                yAnim.From = motion.FromY;
                zAnim.From = motion.FromZ;
                oAnim.From = motion.FromOpacity;
            }
            else
            {
                (sb.Children[0] as Rotation3DAnimation).From = (sb.Children[0] as Rotation3DAnimation).To;
                xAnim.From = xAnim.To;
                yAnim.From = yAnim.To;
                zAnim.From = zAnim.To;
                oAnim.From = oAnim.To;
            }
		}

        protected abstract Motion GetBeforeMotion(int deltaIndex, ElementFlow.NavigationDirection direction);
        protected abstract Motion GetSelectionMotion(ElementFlow.NavigationDirection direction);
        protected abstract Motion GetSelectionChangeMotion();
        protected abstract Motion GetAfterMotion(int deltaIndex, ElementFlow.NavigationDirection direction);
        protected abstract Motion GetAddedMotion();	
	}
}