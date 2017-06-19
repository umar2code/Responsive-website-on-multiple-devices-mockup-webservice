# Responsive-website-on-multiple-devices-mockup-webservice
Webservice that merges url parameters into an image of multiple devices showing a website

## Getting Started

- The script is written in C# .Net 4.5
- You will need MS Visual Studio
- You will need ImageMagick

### Prerequisites

- Download and install imagemagick version 7.0 or newer (http://www.imagemagick.org)

### Installing

- Download "Program.cs", "csv_imagemagick.csproj", "App.config", "Properties/AssemblyInfo.cs"
- Open "csv_imagemagick.csproj" with Visual Studio and build it
- Download the zipped "images" folder, this has all the black screen device images, shadows, etc. needed for the mockup

- You can also use the built version: "csv_imagemagick.exe", "csv_imagemagick.exe.config"

- Create an input csv file with 5 columns (header is skipped while processing)
...
ID;Desktop_Image_URL;Laptop_Image_URL;Phone_Image_URL;Tablet_Image_URL
...

- Edit the config file to meet your needs:

This is where the "blank" images of the devices are downloaded
...
 <add key="ImagesFolder" value="images"></add>
...

Your input CSV file
...
<add key="InputFilePath" value="Devicemockup_Input.csv"></add>    
...

The separator character used in the CSV file
...
<add key="CSVseparator" value=";"></add>
...

This is where you installed imagemagick
...
<add key="ImageMagickPath" value="c:\Program Files\ImageMagick-7.0.3-Q8"></add>
...

Make the background fully transparent or use white backgoung
...
<add key="TransparentBackgroud" value="true"></add> 
...

Name and positions for each device screen
...
<add key="img1_name" value="Laptop" />


<add key="img1_screen_x" value="205" />

<add key="img1_screen_y" value="61" />

<add key="img1_screen_w" value="1306" />
<add key="img1_screen_h" value="817" />

<add key="img1_global_pos_x" value="2156" />
<add key="img1_global_pos_y" value="1002" />

<add key="img1_shadow_x" value="1734" />
<add key="img1_shadow_y" value="1012" />
...



## Running the tests

Simply build and run

## Deployment

Copy "csv_imagemagick.exe", the edited "csv_imagemagick.exe.config" and the "images" folder to the same folder next to your input csv file.

## Built With

* [MS Visual Studio](https://www.visualstudio.com)

## License

This is a private project, do not use

