   M S B A M L   `   `   `  ����C  ?SFD_MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:SFD_MVVM.ViewModelSFD_MVVM.ViewModel  5   	       SFD_MVVM.View.CashWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   4localVM clr-namespace:SFD_MVVM.ViewModel     5   	     � Title$  
CashWindow��5   	   $	��600��6   $	��700��6)    � 	Resources 5      &    SFD_MVVM.ViewModel.CashViewModel   VM��%   &	          5   
   6-   5      .��5	      �  � DataContext# [  6    � ColumnDefinitions 5
   
   �� 5      $��* �6   6+   �� 5      $
��auto �6   6.   �� 5      $��2* �6   6,   .+�5   
   �� $��0��6   $ �� IsSynchronizedWithCurrentItem$
 TrueY�6E    �� SelectionMode$ Single=�6j   ��6#   ��  �� Path$
 Data���5      �� 5      .R�5      �� $��0, 0, 10, 0q�6"   .+�5      �� $��50��6=   $	��300��6L   $��50��6\   $	��300��6j   ��6    �� *	ImagePath+6{   �� 5       �� 
IsReadOnly .  �� TextWrapping$
 Wrap=�64   $��50��6H   $	��300��6W   $��50��6g   $	��300��6u   $��Center=�6�   $��Center=�6�   $��12(�6�   $��5q�6�   ��6�   �� *Description+6  5      5      5      �� 5   
   $��1��6   $��3��6'   $��Stretch=�61   $��Center=�6M   6l   � 5   
   $��2��6   	 � RowDefinitions	 5      �� 5      $��4* �6    6.   �� 5      $��* �6    6-   .+�5       �� $��0��6   $
��Fill=�6H   ��6!   �� $ Data/ImagePath�6Y   �� 5!      $��1��6   $��5q�6#    . $��16(�6A   $
 Wrap=�6O   $��Center=�6�   $��Center=�6�   ��6c   �� $ Data/Description�6�   5"      5#      5$      