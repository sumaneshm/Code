// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncLab
{
    using System.ComponentModel;
    using System.Windows.Media;

    public class LinkInfo : INotifyPropertyChanged
    {
        private string html;
        private string title;
        private int length;
        private Color color;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                title = value;
                NotifyPropertyChanged("Title");
            }
        }

        public string Html
        {
            get
            {
                return html;
            }

            set
            {
                html = value;
                NotifyPropertyChanged("Html");
            }
        }

        public int Length
        {
            get
            {
                return length;
            }

            set
            {
                length = value;
                NotifyPropertyChanged("Length");
            }
        }

        public Color Color
        {
            get
            {
                return color;
            }

            set
            {
                color = value;
                NotifyPropertyChanged("Color");
            }
        }

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
