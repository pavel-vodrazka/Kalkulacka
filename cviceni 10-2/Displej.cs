namespace Kalkulacka
{
    public abstract class Displej
    {
        public string? Retezec { get; protected set; }

        public abstract void Nuluj();

        internal void Zobraz(object o)
        {
            Retezec = o.ToString();
        }
    }
}
