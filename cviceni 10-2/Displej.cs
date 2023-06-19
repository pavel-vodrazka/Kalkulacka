namespace Kalkulacka
{
    /// <summary>
    /// Obecný displej.
    /// </summary>
    public abstract class Displej
    {
        /// <summary>
        /// Textový řetězec, který je na displeji zobrazován.
        /// </summary>
        public string? Retezec { get; protected set; }

        /// <summary>
        /// Abstraktní metoda pro nulování, která musí být implementována ve zděděné třídě.
        /// </summary>
        public abstract void Nuluj();

        /// <summary>
        /// Zobrazí předaný objekt.
        /// </summary>
        /// <param name="o">Objekt, který má být zobrazen (prostřednictvím jeho metody ToString().</param>
        internal void Zobraz(object o)
        {
            Retezec = o.ToString();
        }
    }
}
