using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Zapdate.Core.Domain.Entities
{
    public class Machine
    {
        private string _systemLanguage;

        public Machine(string hardwareId, string systemLanguage)
        {
            Id = hardwareId;
            SystemLanguage = systemLanguage;

        }

        public string Id { get; private set; }

        public OperatingSystem OperatingSystem { get; set; }

        public string SystemLanguage
        {
            get => _systemLanguage;
            set {
                var culture = CultureInfo.GetCultureInfo(value);
                _systemLanguage = culture.TwoLetterISOLanguageName;
            }
        }

    }
}
