#### AvaloniaImageViewer
Кроссплатформенный контрол, для просмотра высокодетализированных изображений, позвволяет приближать отдалять и перемещать изображение в пределах контрола. Работает под Windows, Linux и Android.

### Подключение библиотеки
```
xmlns:ImgView="clr-namespace:ImageViewer;assembly=ImageViewer"
```
### Использование контрола
```
<ImgView:ImageViewerPanel MinScale="0.0000001"
                     MaxScale="4"
                     ImageFit="WidthTop"
                     ImageSource="{Binding CurrentImage}"/>
```
Обязательно:
* ImageSource - Bitmap объект с изображением
Необязательно:
* ImageFit - Подстраивает изображение под размер окна (Height - по высоте, WidthTop - по ширине и премещает центра вверх, WidthCenter - по ширине, WidthBottom - по ширине и перемещает центр вниз)
* MinScale - Минимальное приближение
* MaxScale - Максимальное приближение
