   M S B A M L   `   `   `  ����I  ESSCOUIModels, Version=1.0.2.709, Culture=neutral, PublicKeyToken=nullS9clr-namespace:SSCOUIModels.Controls;assembly=SSCOUIModelsSSCOUIModels.Controls  5      I ESSCOControls, Version=1.0.2.709, Culture=neutral, PublicKeyToken=nullA0clr-namespace:SSCOControls;assembly=SSCOControlsSSCOControls 5	      Q8clr-namespace:SSCOUIModels.Helpers;assembly=SSCOUIModelsSSCOUIModels.Helpers  5
      H DSSCOUIViews, Version=1.0.2.727, Culture=neutral, PublicKeyToken=null;"clr-namespace:SSCOUIViews.ControlsSSCOUIViews.Controls 5         SSCOUIViews.Controls.Card   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5      Ic9clr-namespace:SSCOUIModels.Controls;assembly=SSCOUIModels      5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Econtrols0clr-namespace:SSCOControls;assembly=SSCOControls     5	      Lhelpers8clr-namespace:SSCOUIModels.Helpers;assembly=SSCOUIModels     5
      <viewsControls"clr-namespace:SSCOUIViews.Controls     5      -   5        $	��644��5        � 	Resources  5      5   /SSCOUIModels.Helpers.StateToVisibilityConverter   StateToVisibilityConverter-   'SSCOUIModels.Helpers.ItemImageConverter  ItemImageConverter��%B   &	        &	        5   
   5        5   
   5      5       � RowDefinitions 5      �� 5   
   $	��184 �5      �� 5   
   $
��Auto �5      .+�5      �� $��2��5   	     glowBorderStyle#��[ 5   	   � 5      $��0��5    	   .+�5!   
   �� -   5        $��itemReceipt��5"      $	��649��5#      .+�5$      ]�   mainFontRegular#��[ 5%      $��
24,18,24,0q�5&      .+�5'      #  SSCOControls.MeasureTextBlock  -   5        $��itemDescription��5(      $	��356��5)      $��90��5*         cardItemDescriptionTextStyle#��[ 5,      ��5+      �� *Description+5,      !  SSCOControls.PriceTextBlock  5-      -   5        $��
priceWhole��5.      $	��245��5/      $��58��50      $	��Top=�51        cardItemPriceTextStyle#��[ 54         Value 52      �� *Price+   IsPartialTender 53      �� *IsPartialPayment+54      55      �� 56      -   5        $��extraLinesControl��57      ��.  ��59      �� *
ExtraLines+
 � Tag 5:      �� *.+��5;      �� 5<      .R�5=      ��  �� Orientation$ Vertical=�5>       K� IsItemsHost . 5?      5@      ��5B      �� 5C      .R�5D      �� $��extraLinesPanel��5E      $
��Left=�5F      $	��596��5G      $ 
Horizontal=�5H      $��	24,5,24,0q�5I        mainFontLight#��[ 5J      .+�5K        $��itemDescriptionLines1��5L   !     cardItemExtraLineTextStyle#��[ 5N   !   ��5M   !   �� 5N   !   5O      5P      5R      �� 5S      -   5        $��subItemsControl��5T      ��.  ��5V      �� *
SubItems+��5W      �� 5X      .R�5Y      �� $ Vertical=�5Z       . 5[      5\      ��5^      �� 5_      .R�5`      �� $��subItemsPanel��5a      $ 
Horizontal=�5b      $��	24,5,24,0q�5c      .+�5d        $��subItemsDescription��5e   !   $	��392��5f   !    # cardSubItemDescriptionTextStyle#��[ 5h   !   ��5g   !   �� *Description+5h   !     5i      $��subItemPriceWhole��5k   !   $	��204��5j   !    	 cardSubItemPriceTextStyle#��[	 5m   !    5l   !   �� *Price+5m   !   5n       �� Triggers 5o      �� 5p       �� Value$  ��5r   !   	 �� Binding	 5q   !   �� *Price+.a�5s   "   �� 
 �� 
TargetName$
 subItemsPanel��5v   %    �� Property � y� �� Value$ 	Collapsed=�5u   %   5v   %   5w      5y      5{      5|      5}      � 5~      -   5        $��	FullPanel��5   	   $��1��5�   	   $	��274��5�   	   $��Bottom=�5�   	    � ColumnDefinitions 5�   
   �� 5�      $
��Auto �5�      �� 5�      $��3* �5�      .+�5�   
   � $��Bottom=�5�      $��
24,0,24,24q�5�      .+�5�      �� ��.  .��5�      �� -   5        $��imageHolder��5�      
�������$��Bottom=�5�      $	��240��5�      $	��240��5�      ._�5�      �� -	   5        $��	itemImage��5�      $��1��5�      $
��Fill=�5�      ��5�      �� *
ItemCode+ �� 	Converter# [  �� ConverterParameter$
 Card��5�      ��5�      �� 5�       �� 
TargetType" ��5�       �� Triggers 5�      �� 5�      	 5�   !   �� *Source+ �� ElementName$ 	itemImage�� 5�   !   Z� .a�5�   "   ��  � y�$ Hidden=�5�   %   5�      5�      5�      5�      � 5�   
   $��1��5�      .+�5�      #  SSCOControls.AnimationControl    
 bagItemAnimationControlStyle#��[
 5�      $��	0,0,24,24q�5�      y�5�      �� *" DataContext.BackgroundStateParam+ �� RelativeSource ��  �� AncestorType    SSCOUIModels.Controls.View"  # [  $	 Bag��  5�      -
   5        $��AssistanceAnimation��5�       # assistanceAnimationControlStyle#��[ 5�      $��
0,0,24,100q�5�      &   SSCOUIViews.Controls.CardMessage  5�      -   5          Name$ CardMessage��5�      $y�	Collapsed=�5�      �� 5�      -   5        $��ApprovedPanel��5�      $��Center=�5�      $��Right=�5�      $��
0,0,24,100q�5�      $y�	Collapsed=�5�      .+�5�      ��   ApprovedImageStyle#��[ 5�        5�        ApprovedTextStyle#��[ 5�      ��5�        SSCOControls.LocExtension  *
Approved+5�      5�      5�      5�      � 5�      -   5        $��QuickPickPanel��5�   	   $��1��5�   	   $	��126��5�   	   $��Bottom=�5�   	   $y�	Collapsed=�5�   	    5�   
   �� 5�      $	��210 �5�      �� 5�      $	��430 �5�      .+�5�   
   � $��Bottom=�5�      $
��Left=�5�      $��
24,0,24,24q�5�      .+�5�      �� ��.  .��5�      �� $��Bottom=�5�      $��91��5�      $	��102��5�      ._�5�      �� -   5        $��quickpickItemImage��5�      $
��Fill=�5�      ��5�      �� *
ItemCode+# [ $ 	QuickPick��5�      ��5�      �� 5�      " ��5�       5�      �� 5�      	 5�   !   �� *Source+$ quickpickItemImage�� 5�   !   Z� .a�5�   "   ��  � y�$ Hidden=�5�   %   5�      5�      5�      5�      � 5�   
   $��1��5�      .+�5�         ) %quickPickBagItemAnimationControlStyle#��[ 5�      $��0,0,8,8q�5�      y�5�      �� *" DataContext.BackgroundStateParam+ �� "  # [  $	 Bag��5�        5�      -   5        $"��QuickPickAssistanceAnimation��5�       , (quickPickAssistanceAnimationControlStyle#��[ 5�      $��	0,0,24,24q�5�        5�      -   5        $ QuickPickCardMessage��5�      $y�	Collapsed=�5�      �� 5      -   5        $��QuickPickApprovedPanel��5     $��Bottom=�5     $��Right=�5     $��	0,0,24,24q�5     $y�	Collapsed=�5     .+�5     ��   ApprovedImageQuickPickStyle#��[ 5       5       ApprovedTextQuickPickStyle#��[ 5
     ��5	       *
Approved+5
     5     5     5     5     