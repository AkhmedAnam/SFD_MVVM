   M S B A M L   `   `   `  ����C  ?SFD_MVVM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:SFD_MVVM.ViewSFD_MVVM.View  5   	   7 clr-namespace:SFD_MVVM.ViewModelSFD_MVVM.ViewModel  5   	   #clr-namespace:SFD_MVVMSFD_MVVM  5   	   O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5   	   ,    &SFD_MVVM.View.ExperementalDataFormView   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1	localViewclr-namespace:SFD_MVVM.View     5   	   4localVM clr-namespace:SFD_MVVM.ViewModel     5   	   (localclr-namespace:SFD_MVVM     5   	   6sys&clr-namespace:System;assembly=mscorlib     5   	   -   5          �Name$
  root��5	   	    � Title$ ExperementalDataFormView��5   	   $	��450��6*   $	��800��67    � 	Resources 5
      6   0SFD_MVVM.ViewModel.ExperementalDataFormViewModel 
  dataVM  dataFromOrgansEnum S� 
ObjectType  System.Enum S� 
MethodName S� MethodParameters M� TypeName-   'SFD_MVVM.View.TimeSpanToStringConverter  timeSpanToStr&    SFD_MVVM.DoubleToStringConverter  dblToStr��%�   &	        &	      &	 �     &	 �       5   
   6A   S� 5   
   "  68   $ 	GetValues��6W    5      M� 5      $ localVM:Organs��6"   6>   5        5   
   6F     5   
   6;   5      .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   6+   �� 5      $
��1.5* �6   6.   .+�5   
   � $��0��6    � RowDefinitions 5      �� 5      $��* �6    6-   �� 5      $
��auto �6    60   .+�5       �� $��1��6   $
��Save��6"   $��30��6�   $��60��6�   $	��100��6�   $��50��6�   $��5q�6�   ��61   �� 	 �� Source#	 [  
 �� Path$
 SaveButtonClick���6z   ��  �� ElementName$
 root��6�   � 5"      $��0��6    � DataContext# [  6     5#      �� 5$      $
��auto �6'   66   �� 5%      6)    5'      �� 5(      6&   �� 5)      6&   �� 5*      6&   �� 5+      6&   �� 5,      6&   .+�5.      �� $��0��6   $��0��6(   $��5q�65   $��Center=�6@   $��Center=�6d   $��14(�6�   $��Mouse number:��6�   �� 5/      $��0��6   $��1��6(   $��5q�65   $��Center=�6@   $��Center=�6d   $��14(�6�   $��Organ:��6�   �� 50      $��0��6   $��2��6(   $��5q�65   $��Center=�6@   $��Center=�6d   $��14(�6�   $��Injected dose:��6�   �� 51      $��0��6   $��3��6(   $��5q�65   $��Center=�6@   $��Center=�6d   $��14(�6�   $��Current concentration:��6�   �� 52      $��0��6   $��4��6(   $��5q�65   $��Center=�6@   $��Center=�6d   $��14(�6�   $��Time interval:��6�      SFD_MVVM.View.UpDown  53      $��1��6#   $��0��63   $��5q�6@      CurrentValue 54      �� 55      $
 MouseNumber�6"    �� Mode$ OneWayToSource=�65    �� ValidationRules 56      4   .SFD_MVVM.View.EnteredMouseNumberValidationRule  57   "   6O   59      5;      �� 5<      $��14(�6   $��Center=�6)   $��Center=�6M   $��1��6o   $��1��6   $��10q�6�   ��6�   �� #	 [  �� SelectedItem 6�   �� #	 [  $
 Organ�5=      �� 5>      $��1��6   $��5q�6*   $��2��65   $��14(�6B    �� TextWrapping$
 Wrap=�6P    �� TextAlignment$ Center=�6d   $��Center=�6{   .��6�   ��5?      �� 5@      $

 Dose�6"    �� UpdateSourceTrigger$ PropertyChanged=�6.   $ OneWayToSource=�6T    �� 	Converter# [ 6j    5A      -   'SFD_MVVM.View.EnteredDoseValidationRule  5B   "   6H   5D      .��5E   $   5F      �� 5G      $��1��6N   $��5q�6^   $��Center=�6i   $��Center=�6�   $��14(�6�   $��3��6�   ��6   �� *CurrentConcentration+$ OneWay=�6�   �� 5H      $��1��6   $��4��6*   $
 Wrap=�67   $ Center=�6K   $��Center=�6b   $��14(�6�   $��5q�6�   .��6�   ��5I      �� 5J      $
 TimeSpanProp�6"   # [ 66   $ PropertyChanged=�6a    5K      1   +SFD_MVVM.View.EnteredTimeSpanValidationRule  5L   "   6L   5N      .��5O   $   5P      5Q      5R      �� 5S   
   -   5         ��Name$	 Img��5S      $
��Fill=�6   $��1��6,   6>   5T      5V      