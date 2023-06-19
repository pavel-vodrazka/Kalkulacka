namespace Kalkulacka
{
    /// <summary>
    /// Hlavní displej kalkulačky.
    /// </summary>
    public class HlavniDisplej : Displej
    {
        public HlavniDisplej() : base()
        {
            Retezec = string.Empty;
        }
        public override void Nuluj()
        {
            Retezec = string.Empty;
        }
    }
}
