   M S B A M L   `   `   `  ����  UniCade    NotificationWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5        � Title$  Notification Popup��5      $	��300��6    � SizeToContent$ Height=�6*    � WindowStyle$
 None=�5       � AllowsTransparency . 
������ $��148.772��6I    � 
ResizeMode$ NoResize=�6Z   .��5      �  J� RenderTransformOrigin$	 0,1&�6   .+�5   
   �� ���	#FFD5D6CE
���   � �� CornerRadius$ 1��6<    J� Opacity$
 0.65Q�6M   ._�5      �� -   5         ��Name$ 
TextBlock0��5      $
��Wrap=�6,   $
��Body��6@   $��10,49,10,10q�6L   5      �� 5   
   -   5        $ TextBlock11��5      $
��Left=�6)   $��	10,10,0,0q�6D   $
��Wrap=�6W   $��Title��6k   $	��Top=�6x   $��30��6�   $	��280��6�   $
��Bold"�6�   $��14(�6�   	 � Triggers	 5   
   :� 5      
 :� RoutedEvent$
 FrameworkElement.Loaded��6   .[�5      �� .��5      �� .��5      U�  �� TargetProperty$9 3(UIElement.RenderTransform).(ScaleTransform.ScaleY)�68   .\�5      ��  O� KeyTime$ 0:0:0��63    O� Value$ 0Q�6C   �� 5      $ 0:0:0.5��63   $ 1Q�6E   5      U� 5      -   5        $ (UIElement.Opacity)�5   8   .\�5      �� $ 0:0:2��63   $ 1Q�6C   �� 5      $ 0:0:4��63   $ 0Q�6C   5      5      5      5       z�5#   
   �� 5$       �� ScaleY$ 1Q�6   5'      5)      