   M S B A M L   `   `   `  ����W  SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U<clr-namespace:System.Windows.Input;assembly=PresentationCoreSystem.Windows.Input  5   	   H DMahApps.Metro, Version=1.6.0.0, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls 5   	    FrontFramework    FrontFramework.LoginWindow   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml      5   	   Ninput<clr-namespace:System.Windows.Input;assembly=PresentationCore      5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro      5   	   -   5        $	��290��5   	     � SizeToContent$  Height=�5   	    � 
ResizeMode$ NoResize=�5   	   $��16(�5	   	   $��等线)�5
   	   
�������   AccentColorBrush(  "MahApps.Metro.Controls.MetroWindow   	GlowBrush# �   5   	    � WindowStartupLocation$ CenterScreen=�5   	    � Icon$ resources/icon.png��5   	    � 	Resources 5       �� 
TargetType �� Property �� Value��%�   '��       �� 5   
   " ��6   .�5      ��  � ��$
 Left=�64   �� 5       � ��$ Center=�62   �� 5       � ��$ 0 7 0 7q�6'   �� 5       � ��$ 16(�6)   �� 5       � ��$ 等线)�6+   5      5      .��5      � $��
15 20 15 3q�6   	 � RowDefinitions	 5    
   �� 5!      $
��Auto �6   6,   �� 5"      $
��Auto �6   6,   �� 5#      $
��Auto �6   6,   .+�5%   
   � $��0��6   
 � ColumnDefinitions
 5&      �� 5'      $
��Auto �6#   62   �� 5(      $��* �6#   6/   	 5*      �� 5+      $��50 �6    6.   �� 5,      $��50 �6    6.   �� 5-      $��50 �6    6.   .+�5/      �� -   5        $��labelUserName��5/   1   $��0��6   $��0��6!   61   �� 50      -   5        $��labelPassword��50   1   $��1��6   $��0��6!   61   �� 51      -   5        $��labelLanguage��51   1   $��2��6   $��0��6!   61   �� 53      -   5        $��userNameTextbox��53   3   $��0��6   $��1��6#    �� TextWrapping$
 Wrap=�6J   $��Stretch=�54      $��Stretch=�64   $��Center=�6P    �� 	MaxLength$ 16��55       �� IsInputMethodEnabled .  $��0 7 5 7q�6T   $��16(�56      $��等线)�6$   G� 57      -   5        $��passwordTextbox��57   7   $��1��6   $��1��6'   $��Stretch=�58      $��Stretch=�68   $��Center=�6T    G� 	MaxLength$ 16��59      $��0 7 5 7q�6)   $��16(�5:      $��等线)�6(   �� 5;      -   5        $��languageComboBox��5;   4   $��2��6   $��1��6$   $��Stretch=�5<      $��Stretch=�65   $��Center=�6Q   $��0 7 5 7q�5=   K   $��16(�5>      $��等线)�6%   5?      � 5@   
   $��1��6   $��0 5 0 0q�6   
 5A      �� 5B      6%   �� 5C      6%   .+�5E      �� -   5        $��buttonLogin��5E   %   $��0��6   $��Stretch=�68   $��Stretch=�6V   $��16(�6r   $��等线)�6�   ���	#FF337AB7
�������$��2 5 10 3q�5F   R    �� 	IsDefault . .��6u   Button6}   �� 5G      -	   5        $��buttonCancel��5G   %   $��1��6   $��Stretch=�69   $��Stretch=�6W   $��16(�6s   $��等线)�6�   
����������	#FFD9534F$��10 5 2 3q�5H   S    �� IsCancel . .��6u   Button6}   5I      � 5J   
   $��2��6   .+�5K      �� -
   5        $��statusDisplay��5K      $��Stretch=�6)   $��Center=�6G   $��Right=�6b   $��0q�6�   $
��Auto��5L      
���  ��6   5M      5N      5O      