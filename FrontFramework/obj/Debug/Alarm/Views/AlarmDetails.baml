   M S B A M L   `   `   `  ����H  DMahApps.Metro, Version=1.6.0.0, Culture=neutral, PublicKeyToken=nullV;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.MetroMahApps.Metro.Controls  5   	    FrontFramework-   'FrontFramework.Alarm.Views.AlarmDetails   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   PControls;clr-namespace:MahApps.Metro.Controls;assembly=MahApps.Metro     5   	   R NMahApps.Metro.IconPacks, Version=1.9.0.0, Culture=neutral, PublicKeyToken=null=	iconPacks-http://metro.mahapps.com/winfx/xaml/iconpacks  5   	     � Title$  AlarmDetails��5   	      AccentColorBrush(   "MahApps.Metro.Controls.MetroWindow   	GlowBrush# �   5   	    � Icon$ ../../resources/icon.png��5   	   $��16(�5	   	   $��等线)�5
   	    � WindowStartupLocation$ CenterScreen=�5   	   $	��500��6/   $	��800��6<   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   6,   �� 5      $��* �6   6)   .+�5   
   o� $��0��6   .@�5      m� -   5        $��menuOperation��5      $��操作��6,   .@�5      m� -   5        $��	menuPrint��5      $��打印��6,    m� Icon 5      �� 5      $&�� ../../resources/images/print.png��6    $��16��6J   5      5      5      � 5   
   $��1��6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   62   �� 5      $��* �6#   6/   .+�5      � $��0��6   .+�5       �� .��6   aaa6    5!      �� 5"      $��1��6   .��5#      &   System.Windows.Controls.DataGrid  -   5        $��alarmDataGrid��5#         CanUserAddRows .  $��3 3 3 3q�6G     AzureDataGrid#��[ 5$         Columns 5%      0  *System.Windows.Controls.DataGridTextColumn  5&      ,  &System.Windows.Controls.DataGridColumn	   Width5  /System.Windows.Controls.DataGridLengthConverter$	 1* 6-   
   Header$	
 ***��68     5'      $	 1* 6-   $	
 ***��68     5(      $	 2* 6-   $	
 ***��68     5)      $	 2* 6-   $	
 ***��68     5*      $	 1* 6-   $	
 ***��68     5+      $	 5* 6-   $	
 ***��68     5,      $	 3* 6-   $	
 ***��68   5.      5/      50      51      52      