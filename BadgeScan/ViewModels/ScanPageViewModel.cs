using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public Func<string, ICollection<string>, ICollection<string>> SortingAlgorithm { get; } = (text, values) => values.Where(x => x.ToLower().StartsWith(text.ToLower())).OrderBy(x => x).ToList();

        private async static void SearchTextPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            var vm = (ScanPageViewModel)bindable;

            if (vm.Suggestions != null && vm.Suggestions.Count() <= 1)
            {
                var contacts = await ServiceProxy.GetAllContacts();
                foreach (var c in contacts) vm.Suggestions.Add($"{c.fullname}");
            }
        }
    }
}