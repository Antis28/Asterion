# Asterion
Моя небольшая программа на C #.

Реализован графический интерфейс для консольного конвертера WebP.
(Это формат, созданный Google, позволяет получить более высокое сжатие, чем формат JPEG).

Поскольку инструмент, предоставляемый Google, трудно использовать (только в командной строке с многочисленными параметрами настройки через [параметры] (https://developers.google.com/speed/webp/docs/cwebp), я решил создать этот интерфейс для персонального использования.

Для переносимости конвертер был встроен в исполняемый файл в качестве ресурса и будет скопирован рядом с приложением при первом использовании или если он не был найден.

** ПРИМЕЧАНИЕ: ** Это приложение находится на ранней стадии разработки и все еще содержит ** много ошибок / многие функции еще не доступны **. Если есть большой общественный интерес, я могу улучшить приложение. Это мое первое приложение на C #, поэтому используйте приложение на свой риск и ** будьте дружелюбны с вашей критикой **: +1:

## Применение
1. Установите .NET 4.0 (если у вас нет установленной среды .NET 4.0, для загрузки установщика требуется подключение к Интернету)
2. Выберите правильное входное изображение(JPEG, PNG).
3. Выберите параметры
3. Нажмите «Начать» и посмотрите результаты вывода(будут созданы в новой папке рядом с оригиналом).

##### Как просматривать изображения webP?
Google Chrome может открывать файлы WebP изначально. Или установить Windows [CODEC](https://developers.google.com/speed/webp/docs/webp_codec) Если вы хотите просматривать файлы WebP в Windows Photo Viewer и их эскизы в Проводнике Windows (Windows 7 или более поздней версии)

## Почему я должен использовать это приложение?
Вы можете использовать один из многих инструментов энкодера и плагинов, но ни один из них не имеет тех же опций, что и официальный инструмент Google Encoder. Кроме того, некоторые плагины (например, Photoshop Plugin) устарели и не дают лучший результат. Если вы хотите кодировать ваши изображения с полным контролем. Приложение позволяет конвертировать каталог пакетов или только выбранные файлы.

## Известные ошибки / Не реализовано
- сохранение / загрузка настроек
- Интеграция библиотеки webP
- Много «неэффективного / простого кода» (это очень простое приложение), поэтому не используйте его как ссылку

### Создание из исходного кода

Asterion был создан с использованием Microsoft Visual Studio Community 2015.
Файлы решения / проекта содержатся в репозитории исходного кода.

### Credits
- Developed by: Antis28
- Includes [WpfFolderBrowser](https://github.com/McNeight/WpfFolderBrowser) Library 
- Includes [cwebp.exe](https://developers.google.com/speed/webp/download) encoding tool v.0.4.1(v.0.6.0) - Copyright (c) 2010, Google Inc

### License

The MIT License (MIT)

Copyright (c) 2017 Antis28

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
