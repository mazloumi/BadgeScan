using System;
using System.Diagnostics;
using System.IO;
using CsvHelper;
using Xamarin.Forms;

namespace BadgeScan
{
    public class Extensions
    {
        public static void LoadFile(string path, bool redirect = true)
        {
            try
            {
                StreamReader reader = new StreamReader(path);
                var parser = new CsvParser(reader);
                var header = parser.Read();
                Debug.WriteLine($"Header: {string.Join(",", header)}");

                while (true)
                {
                    var row = parser.Read();
                    if (row == null)
                    {
                        break;
                    }
                    Debug.WriteLine($"Row: {string.Join(",", row)}");

                    if (header.Length != row.Length) break;
                    for (var i = 0; i < header.Length; i++) {
                        var key = header[i];
                        var value = row[i];
                        switch(key) {
                            case "Authority":
                                Settings.Authority = value;
                                break;
                            case "Resource":
                                Settings.Resource = value;
                                break;
                            case "ApplicationId":
                                Settings.ApplicationId = value;
                                break;
                            case "SearchAttribute":
                                Settings.SearchAttribute = value;
                                break;
                            case "Keyboard":
                                Settings.Keyboard = value;
                                break;
                            case "UseScanner":
                                bool flag;
                                if (Boolean.TryParse(value, out flag)) Settings.UseScanner = flag;
                                break;
                        }
                        Debug.WriteLine($"{key}: {value}");
                    }
                }
                if (redirect) App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"BadgeScan.Extensions.LoadFile Error {ex.Message}, {ex.StackTrace}");
            }
        }


        public static void LoadConfiguration(string configuration) {

            var tempFile = Path.GetTempFileName();
            tempFile = Path.ChangeExtension(tempFile, "badgescan");
            File.WriteAllText(tempFile, configuration);

            LoadFile(tempFile, false);
        }
    }
}
