using System.ComponentModel;
using System.Globalization;

namespace Kalkulacka
{

    /// <summary>
    /// Představuje hlavní logiku (controller) kalkulačky.
    /// </summary>
    public class KalkulackaController : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private VstupniBuffer vstup;
        private RegistrFloat registrA;
        private RegistrFloat registrB;
        private RegistrFloat registrV;
        private RegistrOperatoru registrOperatoru;
        private AritmetickaJednotka aritmetickaJednotka;
        public HlavniDisplej HlavniDisplej { get; private set; }
        public DisplejVypoctu DisplejVypoctu { get; private set; }
        public static readonly string Culture = "cs";
        private static readonly CultureInfo cultureInfo = CultureInfo.GetCultureInfo(Culture);
        private static readonly string numberDecimalSeparator = cultureInfo.NumberFormat.NumberDecimalSeparator;
        private static readonly string ciselneZnaky = $"0123456789{numberDecimalSeparator}";
        private static readonly string znakRovnaSe = "=";
        private static readonly string klavesaC = "C";
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
            registrOperatoru.Zmenen += () =>
            {
                DisplejVypoctu.Prekresli();
            };
            registrB = new(Culture);
            registrB.Zmenen += () =>
            {
                DisplejVypoctu.Prekresli();
            };
            registrV = new(Culture);
            registrV.Zmenen += () =>
            {
                HlavniDisplej.Zobraz(registrV);
                DisplejVypoctu.Prekresli();
            };
            aritmetickaJednotka = new(registrA, registrB, registrV, registrOperatoru);
            HlavniDisplej = new();
            DisplejVypoctu = new(/*"vstup: ", vstup, "| A: ", registrA, "| op: ", registrOperatoru, "| B: ", registrB, "| V: ", registrV, "\t",*/
                registrA, registrOperatoru, registrB);
        }

        private void VyvolejZmenu(string vlastnost)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(vlastnost));
        }
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
            //aritmetickaJednotka.Vypocitej();
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
