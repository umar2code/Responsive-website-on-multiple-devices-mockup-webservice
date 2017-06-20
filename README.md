# Responsive-website-on-multiple-devices-mockup-webservice
Webservice that merges url parameters into an image of multiple devices showing a website

## Getting Started

- This is written in C# .Net 4.5
- You will need MS Visual Studio
- You will need ImageMagick
- You will need IIS installed with full privilages, as this webservice is "calling" imagemagick's magick.exe

### Prerequisites

- Download and install imagemagick version 7.0 or newer (http://www.imagemagick.org) - static version recommended

### Installing

- Download evrything from the src folder
- Open the ".csproj" file with Visual Studio and build it
- You can also use the built version from the bin folder (please note inside the bin folder there is another bin folder)
- Copy the built version to your IIS root folder - by default this is c:\inetpub\wwwroot
- In IIS settings set the running application pool to asp.net 4.0 (default might be 2.0)

- Edit the Web.config file to meet your needs:

This is where you installed imagemagick

```
<add key="ImageMagickPath" value="c:\Program Files\ImageMagick-7.0.3-Q8"></add>

```

Make the background fully transparent or use white backgound

```
<add key="TransparentBackgroud" value="true"></add> 

```

Use logo image or not

```
<add key="Logoimage" value="true"></add> 

```

Phase 1 refers to generating images
Phase 2 refers to creating the device mockup

Name and positions for each device screen

```
<!--        PHASE1        -->
<!--        IMG1        -->
<add key="img1_name" value="Laptop" />


<add key="img1_screen_x" value="205" />

<add key="img1_screen_y" value="61" />

<add key="img1_screen_w" value="1306" />
<add key="img1_screen_h" value="817" />

<add key="img1_global_pos_x" value="2156" />
<add key="img1_global_pos_y" value="1002" />

<add key="img1_shadow_x" value="1734" />
<add key="img1_shadow_y" value="1012" />

```



## Running the tests

Call the service - you can simply copy this to your browser if you want:
http://localhost/ChainMockupService.svc/script?Companyname=Anna%20Didden&Phone_no=798115437&Street=Gerbergasse%2014&Zip=4001&City=Basel&Accentcolor=%2347b403&Textcolor=%23444444&Mainpicture=http%3A%2F%2Fplacehold.it%2F860x500%2F3369E8%2Fffffff&Button_CTA_Text=Jetzt%20zum%20Angebot&ID=1&LogoImageURL=

Please note, you have to URL encode every value you use here. Spaces, hashmarks, etc.
You can use this online tool for URL encoding/decoding: https://meyerweb.com/eric/tools/dencoder/

## Deployment

As above:
- Copy the built version to your IIS root folder - by default this is c:\inetpub\wwwroot
- In IIS settings set the running application pool to asp.net 4.0 (default might be 2.0)

## Built With

* [MS Visual Studio](https://www.visualstudio.com)

## License

This is a private project, do not use

