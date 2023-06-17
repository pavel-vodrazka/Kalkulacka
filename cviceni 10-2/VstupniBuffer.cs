using System;
using System.Text.RegularExpressions;
using System.Globalization;

namespace Kalkulacka
{
    /// <summary>
    /// Reprezentuje vstupní vyrovnávací paměť stisknutých kláves (příchozích znaků).
    /// Příchozí znaky, které v kontextu zadávání nedávají smysl, jsou zahozeny.
    /// </summary>
    internal class VstupniBuffer
    {
        private readonly CultureInfo cultureInfo;
        private readonly string numberDecimalSeparator; 
        /// <summary>
        /// Testovaný řetězec bude validní float.
        /// </summary>
        private readonly Regex hotoveCislo;
        /// <summary>
        /// Testovaný řetězec je potřebuje zadat další numerické znaky.
        /// </summary>
        private readonly Regex nehotoveCislo;
        private string _retezec;
        public Action? Zmenen;
        /// <summary>
        /// Vlastní hodnota, řetězec vzniklý přidáváním akceptovaných znaků (kláves).
        /// </summary>
        public string Retezec
        {
            get => _retezec;
            private set
            {
                _retezec = value;
                PoZmene();
            }
        }
        /// <summary>
        /// Testuje, zda řetězec ve vyrovnávací paměti reprezentuje float.
        /// </summary>
        public bool JeHotoveCislo { get => hotoveCislo.IsMatch(Retezec); }
        /// <summary>
        /// Testuje, zda řetězec ve vyrovnávací paměti může reprezentovat float po přidání dalších znaků.
        /// </summary>
        public bool JeNehotoveCislo { get => nehotoveCislo.IsMatch(Retezec); }
        /// <summary>
        /// Číselná hodnota. Pokud JeNehotoveCislo, vraci 0.
        /// </summary>
        public float Cislo
        {
            get
            {
                _ = float.TryParse(Retezec, NumberStyles.Float, cultureInfo, out float cislo);
                return cislo;
            }
        }

        public VstupniBuffer(string culture)
        {
            cultureInfo = CultureInfo.GetCultureInfo(culture);
            numberDecimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;
            hotoveCislo = new(@$"^((-?{numberDecimalSeparator}[0-9]+)|(-?[0-9]+({numberDecimalSeparator}[0-9]*)?))$");
            nehotoveCislo = new(@$"^(-{numberDecimalSeparator}?)?$");
            _retezec = string.Empty;
        }

        public override string ToString() => Retezec;
        /// <summary>
        /// Připojí k vyrovnávací paměti zadaný znak, pokud výsledný řetězec bude reprezentovat rozdělaný nebo hotový float.
        /// </summary>
        /// <param name="retezec">Řetězec, který se má přidat.</param>
        /// <returns><b>true</b>, pokud byl řetězec akceptován, jinak <b>false</b>.</returns>
        public bool PridejZnak(char znak)
        {
            string novyRetezec;
            string staryRetezec;
            if (Retezec == "0" && znak != numberDecimalSeparator[0])
                Retezec = string.Empty;
            if (Retezec == string.Empty && znak == numberDecimalSeparator[0])
                Retezec = $"0{znak}";
            novyRetezec = Retezec + znak;
            staryRetezec = Retezec;
            Retezec = string.Empty;
            if (hotoveCislo.IsMatch(novyRetezec) || nehotoveCislo.IsMatch(novyRetezec))
            {
                Retezec = novyRetezec;
                return true;
            };
            Retezec = staryRetezec;
            return false;
        }
        /// <summary>
        /// Pokud řetězec ve vyrovnávací paměti reprezentuje float, vrátí toto číslo a vymaže vyrovnávací paměť.
        /// Jinak vrátí null.
        /// </summary>
        /// <returns><b>float?</b> reprezentovaný řetězcem ve vyrovnávací paměti.</returns>
        public float? VratCisloAVymaz()
        {
            float? cislo;
            cislo = JeHotoveCislo ? Cislo : null;
            Vymaz();
            return cislo;
        }
        /// <summary>
        /// Vymaže vyrovnávací paměť.
        /// </summary>
        public void Vymaz()
        {
            Retezec = string.Empty;
        }

        protected virtual void PoZmene() => Zmenen?.Invoke();
    }
}
