
# BadgeScan
Cross-platform badge barcode scanner mobile app for iOS and Android using Xamarin.Forms. 
![Logo](https://nimamazloumi.files.wordpress.com/2018/02/barcode.png?w=305&h=542)

## Background
At conferences and events, participant identification may be a requirement for entering various spaces. This scenario can be addressed in many different ways. Assume in this case that a decision was made to print badges that participants carry with them. The badge shows the name of the participant and has a bar code. Guides at the facility can request the badge of someone to be scanned to the identity. Also assume that no photo of the participant will be printed on the badge. The use case is simple: Scan the badge and the app shows the name and photo of the person.

![Badge Scanner Mobile App using Xamarin.Forms](https://nimamazloumi.files.wordpress.com/2018/02/img_6906.png?w=256&h=256)

Check out [my blog post](https://wp.me/p9B5ok-v) for more details.

## Configuration Steps
Before running the app, make sure you have completed the below steps.

First make sure you have the following information available:

 - **Resource**: Hostname of your trial Dynamics CRM 365 Online system, e.x. mycomp.crm.microsoft.com
 - **Application Id**: Application Id provided to you by your Azure trial instance when you register this application and grant permissions.
 - **SearchAttribute**: Determine which attribute of the Contact entity is used for the barcode lookup. Currently supported options are `contactid`, `employeeid`, `externaluseridentifier`, and `governmentid`.

Now, create some sample data:

 - **Creating Sample Contacts** - Go to your D365 site and create a couple of contacts. Provide for each one of them an image and in the attribute you selected enter a unique alpha-numeric code.
 - **Generate barcodes** - Use one of the many online websites to generate barcodes for each of your sample contacts using the same code you entered in D365.

 ## Configuration file
You can create a configuration file with the extension `badgescan` that you can open in your mobile device to populate the settings of the application. It must be a comma-separated file with the following format:

    Authority,Resource,ApplicationId,SearchAttribute,Keyboard,UseScanner
    https://login.windows.net/common/oauth2/authorize,your.crm.dynamics.com,88a4c6e2-40a3-2048-bcb1-6391185507ef,contactid,Numeric,false

The column header are case-sensitive. The permitted values for `SearchAttribute`,`Keyboard`,`UseScanner` are:
- `SearchAttribute`: `contactid`, `employeeid`, `externaluseridentifier`, and `governmentid`
- `Keyboard`: `Text`, `Numeric`
- `UseScanner`: `true`, `false`

## Credits
This app was created using:
 - ZXing.Net.Mobile.Forms
 - CsvHelper
 - Xam.Plugins.Settings
 - Com.Airbnb.Xamarin.Forms.Lottie
 - Newtonsoft.Json
 - Microsoft.IdentityModel.Clients.ActiveDirectory
 - [StackEdit](https://stackedit.io/app)
 - [MakeAppIcon](https://makeappicon.com)
 - Icon made by [www.flaticon.com](https://www.flaticon.com/authors/those-icons) and licensed under Creative Commons BY 3.0
