# Asterion
My little program in C #.

Implemented graphical interface for the WebP console converter.
(This format created by Google, allows a much higher compression then JPEG format).

Because the tool provided by Google is hard to use (command-line only with numerous tuning options via [parameters](https://developers.google.com/speed/webp/docs/cwebp), I decided to create this interface to personal use.

For portability, the converter was built into the executable as a resource and copied next to the application on first use or if it was not found.

Also the source code of WpfFolderBrowser is built into the application, for portability.

**NOTE:** This application is at an early stage of development and still contains **many errors/many features are not yet available**. If there is a large public interest, I can improve the application. This is my first C# application so use the application at your risk and **be friendly with your criticism** :+1:

## Usage
1. Install the application (if you don't have the .NET 4.0 framework installed, an internet connection is required to download the installer)
2. Choose a valid input image
3. Check the options 
3. Click Start and see the output results

##### How to view webP images?
Google Chrome can open WebP files natively. Or install Windows [CODEC](https://developers.google.com/speed/webp/docs/webp_codec) if you want to view WebP files in Windows Photo Viewer and their thumbnails in Windows Explorer(Windows 7 or older)

## Why Should I Use this App?
You can use one of many encoder tools and plugins available, but none of them have the same options of the official encoder tool provided by Google. Also, some plugins (like the Photoshop Plugin) are outdated and don’t produce the better output. If you want to encode your images with total control. The application allows you to convert a package directory or only selected files.

## Know bugs/Not implemented yet
- save/read settings
- Integrate webP library
- A lot of ‘dumb/simple code’ (this is a very simple application) so don’t use this as a reference

### Building from Source

The Asterion has been built using Microsoft Visual Studio Community 2015. 
The solution / project files are contained in the source code repository.

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
