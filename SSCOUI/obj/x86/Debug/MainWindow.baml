   M S B A M L   `   `   `  ����C  ?SSCOUI, Version=1.0.2.727, Culture=neutral, PublicKeyToken=null1clr-namespace:SSCOUI.ControlsSSCOUI.Controls  5      I ESSCOUIModels, Version=1.0.2.727, Culture=neutral, PublicKeyToken=nullQ8clr-namespace:SSCOUIModels.Helpers;assembly=SSCOUIModelsSSCOUIModels.Helpers 5      H DSSCOUIViews, Version=1.0.2.727, Culture=neutral, PublicKeyToken=nullP7clr-namespace:SSCOUIViews.Controls;assembly=SSCOUIViewsSSCOUIViews.Controls 5          SSCOUI.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35I ESSCOControls, Version=1.0.2.727, Culture=neutral, PublicKeyToken=nullF 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      +cclr-namespace:SSCOUI.Controls     5      Lhelpers8clr-namespace:SSCOUIModels.Helpers;assembly=SSCOUIModels     5      Lcontrols7clr-namespace:SSCOUIViews.Controls;assembly=SSCOUIViews     5      -   5           mainWindowStyle#��[  5        � Title$  NCR NEXTGENUI��5	       � Icon$ ncr.ico��5
       � Left$ 0��5      
 � Top$ 0��5      $
��1024��5      $	��768��5       � WindowStyle$
 None=�5      -  'System.Windows.Input.KeyboardNavigation   TabNavigation$
 None=�5         ControlTabNavigation$
 None=�5         DirectionalNavigation$
 None=�5       � 
ResizeMode$ NoResize=�5      	 � 	Resources	 5      5  /SSCOUIModels.Helpers.StateToVisibilityConverter  StateToVisibilityConverter��%!   &	         5   
   5      5      .��5      � .+�5   
   %   SSCOUI.Controls.MainAreaControl  -   5        
  Name$
 MainAreaControl��5      %   SSCOUI.Controls.MainAreaControl   ContentRendered$% MainAreaControl_ContentRendered��5      (  "SSCOUIViews.Controls.HeaderControl  5   
   -   5          Name$ HeaderControl��5      $	��Top=�5       1  +SSCOUIViews.Controls.SystemFunctionsControl  5!   
   -   5          Name$ SystemFunctionsControl��5"      $��Bottom=�5#      &   SSCOUIViews.Controls.CartControl  5$   
   -   5          Name$ CartControl��5%      $��Right=�5&      +  %SSCOUIViews.Controls.DecoratorControl  5'   
   -   5          Name$ DecoratorControl��5(       K� ZIndex$	 102��5)      "	   SSCOUI.Controls.PopupControl	  5*   
   -   5         	 Name$ PopupControl��5+      5,      5-      