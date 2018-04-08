using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;

namespace BadgeScan.ViewModels
{
    public class ScanPageViewModel : BindableObject
    {
        public static readonly BindableProperty SearchTextProperty = BindableProperty.Create(nameof(SearchText),typeof(string),typeof(ScanPage),default(string),propertyChanged: SearchTextPropertyChanged);
        public static readonly BindableProperty SuggestionsProperty =BindableProperty.Create(nameof(Suggestions),typeof(ObservableCollection<string>),typeof(ScanPage),new ObservableCollection<string>());
        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem),typeof(object), typeof(ScanPageViewModel));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value);}
        }

        public ObservableCollection<string> Suggestions
        {
            get { return (ObservableCollection<string>)GetValue(SuggestionsProperty); }
            set { SetValue(SuggestionsProperty, value); }
        }

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; } = (text, values) =>
        {
            var list = values.Where(x => x.ToLower().Contains(text.ToLower())).OrderBy(x => x).Select(s => s.Split(':')[0]).ToList();
            Debug.WriteLine($"{string.Join(",", list)}");
            return list;
        };

        private async static void SearchTextPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var vm = (ScanPageViewModel)bindable;

            if (Settings.Reload)
            {
                Debug.WriteLine($"Clearing list");
                vm.Suggestions.Clear();

                var contacts = await ServiceProxy.GetAllContacts();
                Debug.WriteLine($"{contacts.Count()} records retrieved from D365");
                foreach (var c in contacts)
                {
                    switch (Settings.SearchAttribute)
                    {
                        case "contactid":
                            if (!vm.Suggestions.Contains($"{c.contactid}")) vm.Suggestions.Add($"{c.contactid}");
                            break;
                        case "employeeid":
                            if (!vm.Suggestions.Contains($"{c.employeeid}")) vm.Suggestions.Add($"{c.employeeid}");
                            break;
                        case "externaluseridentifier":
                            if (!vm.Suggestions.Contains($"{c.externaluseridentifier}")) vm.Suggestions.Add($"{c.externaluseridentifier}");
                            break;
                        case "governmentid":
                            if (!vm.Suggestions.Contains($"{c.governmentid}")) vm.Suggestions.Add($"{c.governmentid}");
                            break;
                        case "fullname":
                            if (!vm.Suggestions.Contains($"{c.fullname}"))
                            {
                                var text = (c.parentcustomerid_account != null) ? $"{c.fullname}:{c.parentcustomerid_account.name}" : $"{c.fullname}:";
                                vm.Suggestions.Add(text);
                            }
                            break;
                    }
                }
                Debug.WriteLine($"{vm.Suggestions.Count()} unique records added to list");
                Settings.Reload = false;
            }
        }
    }
}