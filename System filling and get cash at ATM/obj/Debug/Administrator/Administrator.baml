   M S B A M L   `   `   `  ����]  YSystem filling and get cash at ATM, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nulls>clr-namespace:System_filling_and_get_cash_at_ATM.Administrator0System_filling_and_get_cash_at_ATM.Administrator  5   	   D    >System_filling_and_get_cash_at_ATM.Administrator.Administrator   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   Plocal>clr-namespace:System_filling_and_get_cash_at_ATM.Administrator     5   	   $��336.405��5        � WindowStartupLocation$  CenterScreen=�6.   $��350.405��6S   $��700.025��6h   $��700.025��6{    � Topmost . .��5      � .+�5	   
   �� -   5        $��Panel2��5	      .+�5
      ��  �� Padding$ 15q�6   
�������$��2q�65   $	��307��6I   ���	#FF898989._�5      �� $��-17,-17,-17,-29q�6   ���	#FF898989.@�5      �� $0��*Забл/Розбл користувача��6   $��30��6>   .��5      � .+�5      &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5   '   $��382,21,23,45q�69      AutoGenerateColumns . ���	#FFB2B2B26j   �� 5      -   5         ��Name$ buttonRefresh��5   %   $��Обновити��6<   $
��Left=�6O   $��421,242,0,0q�6j   $	��Top=�6   $��96��6�   6�   �� 5      -   5        $ buttonBlockUnblockUser��5   %   $!��Змінити статус��6E   $
��Left=�6^   $��257,197,0,0q�6y   $	��Top=�6�   $��90��6�   6�   �� 5      -   5         ��Name$ textBoxBlockUnblockUser��5   &   $
��Left=�6G   $��23��6b   $��240,169,0,0q�6n    �� TextWrapping$
 Wrap=�6�   $	��Top=�6�   $	��120��6�   ���	#FFB2B2B26�   �� 5      -   5         ��Name$  labelSetALimitCalculations��5   $   $<��6Встановити ліміт розрахунків��6H   $
��Left=�6o   $��
201,21,0,0q�6�   $	��Top=�6�   �� 5      -   5        $ BlockUnblockUser��5   $   	 K� ZIndex$	 1��6>   $>��8Встановити статус користувачу��6O   $
��Left=�6w   $��201,138,0,0q�6�   $	��Top=�6�   $��31��6�   �� 5      -   5        $$ textBoxIdSetALimitCalculations��5   &   $
��Left=�6N   $��23��6i   $��
249,57,0,0q�6u   $
 Wrap=�6�   $�� ��6�   $	��Top=�6�   $	��120��6�   ���	#FFB2B2B26�   �� 5      -	   5        $! buttonSetALimitCalculations��5   %   $��Встановити��6J   $
��Left=�6_   $��276,115,0,0q�6z   $	��Top=�6�   $��75��6�   6�   �� 5      -
   5        $' !textBoxLimitSetALimitCalculations��5   &   $
��Left=�6Q   $��23��6l   $
 Wrap=�6x   $	��Top=�6�   $	��120��6�   $��
249,85,0,0q�6�   ���	#FFB2B2B26�   �� 5      -   5        $" labelIdSetALimitCalculations��5   $   $	 1��6J   $
��ID :��6[   $
��Left=�6j   $	��Top=�6�   $��
201,54,0,0q�6�   �� 5      -   5        $% labelLimitSetALimitCalculations��5   $   $	 1��6M   $��Ліміт :��6^   $
��Left=�6p   $	��Top=�6�   $��
201,85,0,0q�6�   �� 5      -   5        $ labelBlockUnblockUser��5   $   $
��ID :��6C   $
��Left=�6R   $	��Top=�6m   $��213,166,0,0q�6�   
 J� RenderTransformOrigin$
 1.273,0.423&�6�   �� 5      -   5         ��Name$ textBlockUsersDataGrid��5   (   $
��Left=�6H   $��	405,0,0,0q�6c   $
��Wrap=�6v   $7��1Зареєстровані користувачі��6�   $	��Top=�6�   �� 5      -   5         ��Name$ image��5   $   $
��Left=�63   $
��Auto��6N   $��Stretch=�6\   $	��213��6x   $F��@/System filling and get cash at ATM;component/Resource/block.png��6�   $��
-5,10,0,10q�6�   �� 5      -   5        $ 
buttonExit��5   %   $��
Вийти��69   $
��Left=�6I   $��556,242,0,0q�6d   $	��Top=�6y   $	��131��6�   $��29��6�   6�   5      5      �� 5       $5��/Надати виписку по рахунку��6   $��30��6A   .��5!      � .+�5"      �� -   5         ��Name$ richTextBoxProvideBilling��5"   *   $��0,40,-2,-11q�6M   ���	#FFB2B2B2. �5#   "   /� .T�5$   &   J� .*�60   .*�5%   *   �� $� ��6.   5&   '   5'   #   5(      �� 5)      -   5        $ buttonProvideBilling��5)   %   $!��Надати виписку��6C   $
��Left=�6\   $��
443,12,0,0q�6w   $	��Top=�6�   $��99��6�   6�   �� 5*      -   5        $ textBoxProvideBilling��5*   &   $
��Left=�6E   $��23��6`   $
 Wrap=�6l   $	��Top=�6�   $	��120��6�   $��
292,12,0,0q�6�   ���	#FFB2B2B26�   �� 5+      -   5        $ textBlockProvideBilling��5+   (   $
��Left=�6I   $
��Wrap=�6d   $Y��SВкажіть ID за яким ви хочете получиит виписку :��6x   $	��Top=�6�   $��	10,12,0,0q�6�   5-      5.      5/      50      51      52      53      