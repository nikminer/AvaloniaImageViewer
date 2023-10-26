### Подключение библиотеки
```
xmlns:bkImg="clr-namespace:BK.Controls;assembly=BK.Controls.ImageViewer"
```
### Использование контрола
```
<bk:ImageViewer MinScale="0.1"
                MaxScale="4"
				ImageSource="{Binding CurrentImage}"/>
```
* ImageSource - Bitmap объект с изображением
* MinScale - Минимальное приближение
* MaxScale - Максимальное приближение
