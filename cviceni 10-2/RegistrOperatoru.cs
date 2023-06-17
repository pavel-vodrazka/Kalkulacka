namespace Kalkulacka
{
    internal class RegistrOperatoru : Registr<string?>
    {
        public override void VlozHodnotu(string? hodnota)
        {
            if (hodnota is null || AritmetickaJednotka.Operatory.Contains(hodnota))
            {
                Hodnota = hodnota;
            }
        }
    }
}
