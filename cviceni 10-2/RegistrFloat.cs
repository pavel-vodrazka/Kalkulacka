using System.Globalization;

namespace Kalkulacka
{
    internal class RegistrFloat : Registr<float?>
    {
        private CultureInfo cultureInfo;

        public override string Retezec { get => _hodnota?.ToString(cultureInfo) ?? string.Empty; }

        public RegistrFloat(string culture) : base()
        {
            cultureInfo = CultureInfo.GetCultureInfo(culture);
        }
    }
}
