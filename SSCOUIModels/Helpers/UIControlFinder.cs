using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace SSCOUIModels.Helpers
{
    public static class UIControlFinder
    {
        public static T FindVisualChild<T>(DependencyObject parent, string name) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                    return (T)child;
                else
                {
                    T childOfChild = FindVisualChild<T>(child, name);
                    if (childOfChild != null)
                    {
                        var grandChild = childOfChild as FrameworkElement;

                        if (grandChild != null && grandChild.Name.Equals(name))
                            return childOfChild;
                    }
                }
            }
            return null;
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static T FindAncestorOrSelf<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                T objTest = obj as T;
                if (objTest != null)
                {
                    return objTest;
                }
                obj = GetParent(obj);
            }
            return null;
        }

        public static DependencyObject GetParent(DependencyObject obj)
        {
            if (obj == null)
            {
                return null;
            }
            ContentElement ce = obj as ContentElement;
            DependencyObject parent;
            if (ce != null)
            {
                parent = ContentOperations.GetParent(ce);
                if (parent != null)
                {
                    return parent;
                }
                FrameworkContentElement fce = ce as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }
            parent = VisualTreeHelper.GetParent(obj);
            if (null == parent && obj is FrameworkElement)
            {
                parent = ((FrameworkElement)obj).Parent;
            }
            return parent;
        }
    }
}
