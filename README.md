# Asterion
My little program in C #.

Implemented graphical interface for the WebP console converter.
(This format created by Google, allows a much higher compression then JPEG format).

Because the tool provided by Google is hard to use (command-line only with numerous tuning options via [parameters] (https://developers.google.com/speed/webp/docs/cwebp), I decided to create this interface to personal use.

For portability, the converter was built into the executable as a resource and copied next to the application on first use or if it was not found.

Also the source code of WpfFolderBrowser is built into the application, for portability.

**NOTE:** This application is at an early stage of development and still contains **many errors/many features are not yet available**. If there is a large public interest, I can improve the application. This is my first C# application so use the application at your risk and **be friendly with your criticism** :+1:

## Usage
1. Install the application (if you don't have the .NET 4.0 framework installed, an internet connection is required to download the installer)
2. Choose a valid input image
3. Check the options 
3. Click Start and see the output results

##### How to view webP images?
Google Chrome can open WebP files natively. Or install Windows CODEC (https://developers.google.com/speed/webp/docs/webp_codec) if you want to view WebP files in Windows Photo Viewer and their thumbnails in Windows Explorer(Windows 7 or older)

## Why Should I Use this App?
You can use one of many encoder tools and plugins available, but none of them have the same options of the official encoder tool provided by Google. Also, some plugins (like the Photoshop Plugin) are outdated and don’t produce the better output. If you want to encode your images with total control. The application allows you to convert a package directory or only selected files.

## Know bugs/Not implemented yet
- save/read settings
- Integrate webP library
- A lot of ‘dumb/simple code’ (this is a very simple application) so don’t use this as a reference

### Resources used
* The library was used https://github.com/McNeight/WpfFolderBrowser for the directory selection dialog.
* The converter is located at https://developers.google.com/speed/webp/docs/precompiled.
