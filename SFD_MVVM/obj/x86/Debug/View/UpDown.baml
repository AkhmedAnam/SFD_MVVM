   M S B A M L   `   `   `  ����C  ?SFD_MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:SFD_MVVM.ViewSFD_MVVM.View  5          SFD_MVVM.View.UpDown   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      -localclr-namespace:SFD_MVVM.View     5      -   5          >�Name$
  root��5      .��5	      �  � 	Resources 5
   
   *   $SFD_MVVM.View.Int64ToStringConverter   	converter��%   &	          5      6?   5       � RowDefinitions 5   
   �� 5      6   �� 5      6    � ColumnDefinitions 5   
   �� 5      $��4* �6   6,   �� 5      $��* �6   6+   .+�5   
   �� -   5         ��Name$ Btn_Up��5      $��0��6'   $��1��64   $��
0, 0, 0, 1q�6D   $��11(�6X   $��0q�6f   $��Center=�6r   $��Center=�6�   $��+��6�   6�   �� 5   
   -   5        $ Btn_Down��5      $��1��6)   $��1��66   $��
0, 1, 0, 0q�6F   $��11(�6Z   $��0q�6h   $��Center=�6u   $��Center=�6�   $��-��6�   6�   �� 5   
   -   5         ��Name$	 txt��5      $��0��6   $��2��6/   $��16(�6V   $��Center=�6�   $��Center=�6  ��6�   ��  �� ElementName$
 root�� �� Path$ CurrentValue� �� Mode$ TwoWay=�	 �� 	Converter#	 [  68  5      5      