   M S B A M L   `   `   `  ����I  ESSCOControls, Version=1.0.2.709, Culture=neutral, PublicKeyToken=nullA0clr-namespace:SSCOControls;assembly=SSCOControlsSSCOControls  5      I ESSCOUIModels, Version=1.0.2.709, Culture=neutral, PublicKeyToken=nullQ8clr-namespace:SSCOUIModels.Helpers;assembly=SSCOUIModelsSSCOUIModels.Helpers 5      S9clr-namespace:SSCOUIModels.Controls;assembly=SSCOUIModelsSSCOUIModels.Controls 5      H DSSCOUIViews, Version=1.0.2.727, Culture=neutral, PublicKeyToken=null;"clr-namespace:SSCOUIViews.ControlsSSCOUIViews.Controls 5
      &    SSCOUIViews.Controls.CartControl   5      =controls0clr-namespace:SSCOControls;assembly=SSCOControls  5      Dhelpers8clr-namespace:SSCOUIModels.Helpers;assembly=SSCOUIModels  5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation       5      Ic9clr-namespace:SSCOUIModels.Controls;assembly=SSCOUIModels       5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5	      <viewsControls"clr-namespace:SSCOUIViews.Controls     5
      -   5        $��cartControlBox��5      $	��340��5         cartControlStyle#��[  5        � RowDefinitions  5      �� 5   
   $
��Auto �5      �� 5   
   $��8* �5      �� 5   
   $
��Auto �5      �� 5   
   $
��Auto �5      .+�5      ��  �� Orientation$ Vertical=�5   	   .+�5   
   ]� ��.    cartControlHeaderStyle#��[ 5      .+�5      ��  K� ZIndex$ 1��5         cartImageStyle#��[ 5!      $��
12,20,0,20q�5"      #   SSCOControls.MeasureTextBlock  5#      -   5        $��ReceiptItemCount��5$      $��8,0,0,0q�5%      $	��100��5&      $��65��5'        itemsCountAndTextStyle#��[ 5(         SSCOControls.ImageButton  5)      -   5        $��EditCartBtn��5*      $��45��5+      $��96��5,      $	��125��5-      $��
0,20,20,20q�5.      {�.    CartButtonStyle#��[ 51         Text 5/         SSCOControls.LocExtension  *
EditCart+��52      �� *ActionCommand+  53      -   5        $��	GoBackBtn��54      $��36��55      $	��126��56      $��
0,20,20,20q�57      #��[ 59      $��Cancel��5;       58        *GoBack+��5:      �� *ActionCommand+5;        5<      -   5        $��CancelAllBtn��5=      $��36��5>      $	��128��5?      $��
0,20,20,20q�5@        RedCartButtonStyle#��[ 5B      $��	CancelAll��5D       5A        *	CancelAll+��5C      �� *ActionCommand+5D      5E      �� 5F   
     cartTopSeparatorStyle#��[ 5G      �� 5H   
   -   5        $��
RewardInfo��5I      $��0,12,0,0q�5J      $y�	Collapsed=�5K        cartControlRewardsAreaStyle#��[ 5L      .+�5M        $��	20,0,20,5q�5N        rewardGreetingStyle#��[ 5O      $
��Wrap=�5Q      ��5P      �� *CustomerMessage+5Q      � 5R        5S      �� 5T      $
��Auto �5U      �� 5V      $
��Auto �5W      .+�5Y        $��20,0,0,0q�5Z        	 pointsAmountEarnedLabelStyle#��[	 5[      ��5\        *PointsEarned+  5]      $��0,0,20,0q�5^        
 pointsAmountEarnedValueStyle#��[
 5_      ��5`      �� *PointEarned+  5a      $��	20,0,0,10q�5b      $��1��5c      #��[	 5d      ��5e        *AmountSaved+  5f      $��0,0,20,0q�5g      $��1��5h      #��[
 5i      ��5j      �� *Saving+5k      �� 5l      #��[ 5m      5n      5o      .  (SSCOUIModels.Controls.SSCOUITouchListBox  5p      -   5        $��CartReceipt��5r   	   $��1��5q   	    �� SelectionMode$ Single=�5s   	   $ �� IsSynchronizedWithCurrentItem$
 TrueY�5t   	     cartReceiptStyle#��[ 5v   	      SSCOControls.TouchListBox   AutoScrollIntoView$ Bottom=�5x   	   ��5u   	   �� *CustomerReceipt+��5z   
   �� 5{      .R�5|      �� .+�5}      '  !SSCOUIViews.Controls.CartListItem     ShowInterventionSection .    ShowRemoveSection . $	��450��5�        CartListItemSelectedStyle#��[ 5�      5�      5�      ��5�   
   �� 5�      	 �� 
TargetType"	 ��5�      .�5�      �� 
 �� Property
 �@�FocusVisualStyle  mainFocusVisualStyle �� Value# [ 5�      5�      5�      �� 5�      $��2��5�   	     cartBottomSeparatorStyle#��[ 5�   	   � 5�      $��2��5�   	   $��70��5�   	     cartControlTotalAreaStyle#��[ 5�   	     5�   
   �� 5�      $��* �5�      �� 5�      $��* �5�      .+�5�   
     -   5        $��TaxText��5�      $��	10,0,20,0q�5�      $��0��5�        taxTextStyle#��[ 5�      $y�Visible=�5�      ��5�        *Tax+5�      !   SSCOControls.PriceTextBlock  5�   
   -	   5        $��TaxValue��5�      $��	20,0,18,0q�5�      $��Right=�5�      $��1��5�      $��0��5�        taxValueTextStyle#��[ 5�      $y�Visible=�5�        5�   
   -
   5        $��	TotalText��5�      $��	10,0,20,0q�5�      $��1��5�        totalTextStyle#��[ 5�      ��5�        *Total+  5�   
   -   5        $��totalAmountValue��5�      $��0,0,18,0q�5�      $��Right=�5�      $��1��5�      $��1��5�        totalValueTextStyle#��[ 5�      5�      ]� 5�      -   5        $��Total��5�   	   $��3��5�   	   $��0,0,0,0q�5�   	   $��72��5�   	     cartControlAreaDueStyle#��[ 5�   	   .+�5�   
       dueTextStyle#��[ 5�      $��10,0,0,0q�5�      $	��115��5�      ��5�        *	DueCart+y�5�      ��  �� ElementName$ Total�� �� Path$ 
Visibility�5�        5�   
   -   5        $��dueAmountValue��5�      $��Right=�5�      $	��200��5�        totalAmountStyle#��[ 5�      $��	0,0,15,12q�5�      5�        5�      -   5        $��	PayButton��5�   	   $��3��5�   	   $��	FinishPay��5�   	     payCartButtonStyle#��[ 5�   	   $��72��5�   	   ��5�   	   �� *ActionCommand+��5�   	   �� *ActualWidth+$ cartControlBox��.��5�   
   ]� ��5�      �� *ActualWidth+$ 	PayButton����5�      �� *ActualHeight+$ 	PayButton��.+�5�          payTextStyle#��[ 5�      $��10,0,0,0q�5�      $��80��5�      ��5�        *	CartPay+y�5�      �� $ 	PayButton��$ 
Visibility�5�      �� 5�      $ 1��5�        payArrowForwardStyle#��[ 5�      $��0,0,15,0q�5�      �� 5�      $ 
Horizontal=�5�      $��Center=�5�      $��Right=�5�      $��Center=�5�      .+�5�        -   5        $��payAmountValue��5�      $	��200��5�        payAmountStyle#��[ 5�      5�      5�      5�      5�      