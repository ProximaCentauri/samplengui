   M S B A M L   `   `   `  ����I  ESSCOControls, Version=1.0.2.709, Culture=neutral, PublicKeyToken=nullA0clr-namespace:SSCOControls;assembly=SSCOControlsSSCOControls  5      I ESSCOUIModels, Version=1.0.2.709, Culture=neutral, PublicKeyToken=nullQ8clr-namespace:SSCOUIModels.Helpers;assembly=SSCOUIModelsSSCOUIModels.Helpers 5      S9clr-namespace:SSCOUIModels.Controls;assembly=SSCOUIModelsSSCOUIModels.Controls 5      H DSSCOUIViews, Version=1.0.2.727, Culture=neutral, PublicKeyToken=null;"clr-namespace:SSCOUIViews.ControlsSSCOUIViews.Controls 5      .   (SSCOUIViews.Controls.ExtendedCartControl   5      =controls0clr-namespace:SSCOControls;assembly=SSCOControls  5      Dhelpers8clr-namespace:SSCOUIModels.Helpers;assembly=SSCOUIModels  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Gc9clr-namespace:SSCOUIModels.Controls;assembly=SSCOUIModels     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5	      <viewsControls"clr-namespace:SSCOUIViews.Controls     5      -   5        $��cartControlBox��5         cartControlStyle#��[  5        � RowDefinitions  5      �� 5   
   $
��.70* �5      �� 5   
   $
��Auto �5      �� 5   
   $
��Auto �5      .+�5      .  (SSCOUIModels.Controls.SSCOUITouchListBox  -   5        $��CartReceipt��5   	    �� SelectionMode$ Single=�5   	      SSCOControls.TouchListBox   AutoScrollIntoView$ Bottom=�5   	   $ �� IsSynchronizedWithCurrentItem$
 TrueY�5   	     cartReceiptStyle#��[ 5   	   ��5   	   �� *CustomerReceipt+��5   
   �� 5      .R�5       '  !SSCOUIViews.Controls.CartListItem  
   NMI .    ShowInterventionSection . $
��1024��5#        CartListItemSelectedStyle#��[ 5$      5%      5'      �� 5(      $��1��5)   	     cartBottomSeparatorStyle#��[ 5*   	   � 5+      $��1��5,   	     cartControlTotalAreaStyle#��[ 5-   	     5.   
   �� 5/      $��* �50      �� 51      $��* �52      .+�54   
   #   SSCOControls.MeasureTextBlock  -   5        $��TaxText��55      $��8,0,8,0q�56      
   NMI .   NMITaxTextStyle#��[ 58      $y�	Collapsed=�5:      ��59         SSCOControls.LocExtension  *Tax+5:      !   SSCOControls.PriceTextBlock  5;   
   -   5        $��TaxValue��5<      $��8,0,12,0q�5=       .   NMITaxValueTextStyle#��[ 5?      $y�	Collapsed=�5@        5A   
   -   5        $��	TotalText��5B      $��8,0,8,0q�5C      $��1��5D       .   NMITotalTextStyle#��[ 5F      ��5G        *Total+  5H   
   -   5        $��totalAmountValue��5I      $��8,0,12,0q�5J      $��Right=�5K      $��1��5L       . $��1��5N        NMITotalValueTextStyle#��[ 5O      5P      ]� 5Q      -   5        $��Total��5S   	   $��2��5R   	   $��0,0,0,8q�5T   	    	 cartControlAreaDueStyle#��[	 5U   	   .+�5V   
      
 NMIDueTextStyle#��[
 5W       . $��8,0,8,8q�5Y      ��5Z        *	DueCart+  5[   
   -   5        $��dueAmountValue��5\      $��Right=�5]       . $	��500��5_        NMITotalAmountStyle#��[ 5`      $��0,0,12,8q�5a      5b      5c      