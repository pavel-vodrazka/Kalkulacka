using System.ComponentModel;
using System.Globalization;

namespace Kalkulacka
{

    /// <summary>
    /// Představuje hlavní logiku (controller) kalkulačky.
    /// Kalkulačka počítá s float čísly.
    /// </summary>
    public class KalkulackaController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// Buffer přijímající vstup z číselných kláves.
        /// </summary>
        private readonly VstupniBuffer vstup;
        /// <summary>
        /// Registr pro první operand.
        /// </summary>
        private readonly RegistrFloat registrA;
        /// <summary>
        /// Registr pro druhý operand.
        /// </summary>
        private readonly RegistrFloat registrB;
        /// <summary>
        /// Registr pro výsledek výpočtu.
        /// </summary>
        private readonly RegistrFloat registrV;
        /// <summary>
        /// Registr pro zadaný operátor.
        /// </summary>
        private readonly RegistrOperatoru registrOperatoru;
        /// <summary>
        /// Aritmetická jednotka kalkulačky.
        /// </summary>
        private readonly AritmetickaJednotka aritmetickaJednotka;
        /// <summary>
        /// Hlavní displej zobrazující právě zadávané číslo a pak výsledek.
        /// </summary>
        public HlavniDisplej HlavniDisplej { get; private set; }
        /// <summary>
        /// Displej zobrazující zadané operandy a operátor.
        /// </summary>
        public DisplejVypoctu DisplejVypoctu { get; private set; }
        /// <summary>
        /// Kultura (pro parsování a výstup desetinného oddělovače).
        /// </summary>
        public static readonly string Culture = "cs";
        private static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo(Culture);
        private static readonly string numberDecimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;
        /// <summary>
        /// Číselné klávesy - znaky (včetně desetinné čárky).
        /// </summary>
        private static readonly string ciselneZnaky = $"0123456789{numberDecimalSeparator}";
        /// <summary>
        /// Klávesa - znak "rovno" k provedení výpočtu.
        /// </summary>
        private static readonly string znakRovnaSe = "=";
        /// <summary>
        /// Klávesa - znak "C" pro vymazání vstupního buffferu, všech registrů a obou displejů.
        /// </summary>
        private static readonly string klavesaC = "C";
        /// <summary>
        /// Klávesa - znak "CE" pro vymazání vstupního bufferu (a hlavního displeje).
        /// </summary>
        private static readonly string klavesaCE = "CE";

        public KalkulackaController()
        {
            vstup = new(Culture);
            vstup.Zmenen += () =>
            {
                HlavniDisplej.Zobraz(vstup);
                DisplejVypoctu.Prekresli();
            };
            registrA = new(Culture);
            registrA.Zmenen += () =>
            {
                HlavniDisplej.Zobraz(registrA);
                DisplejVypoctu.Prekresli();
            };
            registrOperatoru = new();
            registrOperatoru.Zmenen += () => DisplejVypoctu.Prekresli();
            registrB = new(Culture);
            registrB.Zmenen += () => DisplejVypoctu.Prekresli();
            registrV = new(Culture);
            registrV.Zmenen += () =>
            {
                HlavniDisplej.Zobraz(registrV);
                DisplejVypoctu.Prekresli();
            };
            aritmetickaJednotka = new(registrA, registrB, registrV, registrOperatoru);
            HlavniDisplej = new();
            DisplejVypoctu = new(registrA, registrOperatoru, registrB);
        }

        private void VyvolejZmenu(string vlastnost) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(vlastnost));
        
        /// <summary>
        /// Metoda implementující logiku zpracování právě stisknuté klávesy. 
        /// </summary>
        /// <param name="klavesa">Řetězec reprezentující stisknutou klávesu ("0"-"9", ",", "=", "+", "-", "*", "/", "C", "CE").</param>
        public void ZpracujKlavesu(string? klavesa)
        {
            if (klavesa is null || klavesa == string.Empty)
                return;

            if (klavesaC.Contains(klavesa))
                C();
            else if (klavesaCE.Contains(klavesa))
                CE();
            else if (ciselneZnaky.Contains(klavesa))
            {
                if (registrV.Hodnota is not null)
                    C();
                vstup.PridejZnak(klavesa[0]);
            }
            else if (AritmetickaJednotka.Operatory.Contains(klavesa))
            {
                if (registrA.Hodnota is null)
                {
                    registrA.VlozHodnotu(vstup.VratCisloAVymaz() ?? 0);
                    registrOperatoru.VlozHodnotu(klavesa);
                }
                else
                {
                    if (registrOperatoru.Hodnota is null)
                        registrOperatoru.VlozHodnotu(klavesa);
                    else
                    {
                        if (registrB.Hodnota is null)
                        {
                            if (vstup.Retezec == string.Empty)
                                registrOperatoru.VlozHodnotu(klavesa);
                            else
                            {
                                registrB.VlozHodnotu(vstup.VratCisloAVymaz() ?? 0);
                                aritmetickaJednotka.Vypocitej();
                                PresunVDoAVymazBAOp();
                                registrOperatoru.VlozHodnotu(klavesa);
                            }
                        }
                        else
                        {
                            aritmetickaJednotka.Vypocitej();
                            PresunVDoAVymazBAOp();
                            registrOperatoru.VlozHodnotu(klavesa);
                        }
                    }
                }
            }
            else if (znakRovnaSe.Contains(klavesa))
            {
                if (registrA.Hodnota is not null
                    && registrOperatoru.Hodnota is not null)
                {
                    if (vstup.Retezec != string.Empty)
                        registrB.VlozHodnotu(vstup.VratCisloAVymaz() ?? 0);
                    else
                        registrB.VlozHodnotu(registrA.Hodnota);
                    aritmetickaJednotka.Vypocitej();
                }
            }
            VyvolejZmenu(nameof(HlavniDisplej));
            VyvolejZmenu(nameof(DisplejVypoctu));
        }

        private void PresunVDoAVymazBAOp()
        {
            registrA.VlozHodnotu(registrV.VratHodnotuAVymaz());
            registrB.Vymaz();
            registrOperatoru.Vymaz();
        }

        private void C()
        {
            registrA.Vymaz();
            registrB.Vymaz();
            registrOperatoru.Vymaz();
            registrV.Vymaz();
            vstup.Vymaz();
        }
        private void CE()
        {
            vstup.Vymaz();
        }
    }
}
