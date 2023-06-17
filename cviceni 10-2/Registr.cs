using System;

namespace Kalkulacka
{
    public class Registr<T>
    {
        private protected T? _hodnota;
        public Action? Zmenen { get; set; }
        public T? Hodnota
        {
            get => _hodnota;
            private protected set
            {
                _hodnota = value;
                PoZmene();
            }
        }
        public virtual string Retezec { get => _hodnota?.ToString() ?? string.Empty; }

        public override string ToString() => Retezec;

        public virtual void VlozHodnotu(T? hodnota)
        {
            Hodnota = hodnota;
        }

        public T? VratHodnotuAVymaz()
        {
            T? hodnota = Hodnota;
            Vymaz();
            return hodnota;
        }

        public void Vymaz()
        {
            Hodnota = default;
        }

        protected virtual void PoZmene() => Zmenen?.Invoke();
    }
}
