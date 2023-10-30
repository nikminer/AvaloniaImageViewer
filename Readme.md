### Подключение библиотеки
```
xmlns:bkImg="clr-namespace:BK.Controls;assembly=BK.Controls.ImageViewer"
```
### Использование контрола
```
<bk:ImageViewerPanel MinScale="0.1"
                MaxScale="4"
				ImageSource="{Binding CurrentImage}"/>
```
Обязательно:
* ImageSource - Bitmap объект с изображением
Необязательно:
* ImageFit - Подстраивает изображение под размер окна (Height - по высоте, WidthTop - по ширине и премещает центра вверх, WidthCenter - по ширине, WidthBottom - по ширине и перемещает центр вниз)
* MinScale - Минимальное приближение
* MaxScale - Максимальное приближение
