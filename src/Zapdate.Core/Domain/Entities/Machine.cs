using System.Globalization;

namespace Zapdate.Core.Domain.Entities
{
    public class Machine
    {
        private string _systemLanguage;

        public Machine(string hardwareId, string systemLanguage, OperatingSystem operatingSystem)
        {
            Id = hardwareId;

            var culture = CultureInfo.GetCultureInfo(systemLanguage);
            _systemLanguage = culture.TwoLetterISOLanguageName;
            OperatingSystem = operatingSystem;
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
