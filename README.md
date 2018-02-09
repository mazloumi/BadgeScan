
# BadgeScan
Cross-platform badge barcode scanner mobile app for iOS and Android using Xamarin.Forms. 
## Background
At conferences and events, participant identification may be a requirement for entering various spaces. This scenario can be addressed in many different ways. Assume in this case that a decision was made to print badges that participants carry with them. The badge shows the name of the participant and has a bar code. Guides at the facility can request the badge of someone to be scanned to the identity. Also assume that no photo of the participant will be printed on the badge. The use case is simple: Scan the badge and the app shows the name and photo of the person.

![Badge Scanner Mobile App using Xamarin.Forms](https://nimamazloumi.files.wordpress.com/2018/02/img_6906.png?w=305&h=542)

Check out [my blog post](https://wp.me/p9B5ok-v) for more details.

## Configuration Steps
Before running the app, make sure you have completed the below steps.

First configure the `Settings.cs` file:

 - **Resource**: Provide the URL for your trial Dynamics CRM 365 Online system.
 - **Application Id**: Go to your Azure trial and register this application and grant permissions.
 - **ImageUrl**: Enter a valid link to an image here as a place holder, which is shown until the app shows the photo of the contact.
 - **SearchAttribute**: Tell you app what attribute of the Entity Contact should be used for the search.

Now, create some sample data:

 - **Creating Sample Contacts** - Go to your D365 site and create a couple of contacts. Provide for each one of them an image and in the attribute you selected enter a unique alpha-numeric code.
 - **Generate barcodes** - Use one of the many online websites to generate barcodes for each of your sample contacts using the same code you entered in D365.
## Credits
This app was created using:
 - ZXing.Net.Mobile.Forms
 - Newtonsoft.Json
 - Microsoft.IdentityModel.Clients.ActiveDirectory
 - [StackEdit](https://stackedit.io/app)
 - [MakeAppIcon](https://makeappicon.com)
 - Icon made by [www.flaticon.com](https://www.flaticon.com/authors/those-icons) and licensed under Creative Commons BY 3.0