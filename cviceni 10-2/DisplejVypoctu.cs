using System.Collections.Generic;

namespace Kalkulacka
{
    public class DisplejVypoctu : Displej
    {
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

        internal void Prekresli()
        {
            if (Vstupy is not null)
            {
                Retezec = string.Concat(Vystup(Vstupy));
            }
        }

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
