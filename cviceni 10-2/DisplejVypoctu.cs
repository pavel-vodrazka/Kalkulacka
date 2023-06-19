using System.Collections.Generic;

namespace Kalkulacka
{
    /// <summary>
    /// Displej, na němž kalkulačka zobrazuje zadaná čísla a operátor (obecně cokoliv).
    /// </summary>
    public class DisplejVypoctu : Displej
    {
        /// <summary>
        /// Pole objektů předaných coby parametry konstruktoru.
        /// </summary>
        public object?[]? Vstupy { get; init; } = null;

        public DisplejVypoctu() : base()
        {
            Retezec = string.Empty;
        }

        public DisplejVypoctu(params object?[]? vstupy)
        {
            Vstupy = vstupy;
            Prekresli();
        }

        /// <summary>
        /// Překreslení displeje - do zobrazovaného textového řetězce převezme aktuální hodnoty vstupů.
        /// </summary>
        internal void Prekresli()
        {
            if (Vstupy is not null)
            {
                Retezec = string.Concat(Vystup(Vstupy));
            }
        }

        /// <summary>
        /// Vstupní objekty přetransformuje na <b>IEnumerable<![CDATA[<]]>string<![CDATA[>]]></b> s jejich textovými reprezentacemi.
        /// </summary>
        /// <param name="vstupy">Pole předaných objektů.</param>
        /// <returns><b>IEnumerable<![CDATA[<]]>string<![CDATA[>]]></b> s textovými reprezentacemi předaných objektů.</returns>
        private IEnumerable<string> Vystup(object?[]? vstupy)
        {
            if (vstupy is null)
                yield return string.Empty;
            else
            {
                foreach (object? o in vstupy)
                {
                    if (o is null || o.ToString() == string.Empty)
                        yield return string.Empty;
                    else
                        yield return $"  {o}";
                }
            }
        }

        public override void Nuluj()
        {
            Retezec = string.Empty;
        }
    }
}
