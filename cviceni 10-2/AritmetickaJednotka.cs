using System;
using System.Collections.Generic;

namespace Kalkulacka
{
    internal class AritmetickaJednotka
    {

        public static List<string> Operatory { get; } = new() { "+", "-", "*", "/" };
        private RegistrFloat RegistrA { get; set; }
        private RegistrFloat RegistrB { get; set; }
        private RegistrFloat RegistrV { get; set; }
        private RegistrOperatoru RegistrOperatoru { get; set; }

        public AritmetickaJednotka(RegistrFloat registrA, RegistrFloat registrB, RegistrFloat registrV, RegistrOperatoru registrOperatoru)
        {
            RegistrA = registrA;
            RegistrB = registrB;
            //RegistrB.Zmenen += () => { Vypocitej(); };
            RegistrV = registrV;
            RegistrOperatoru = registrOperatoru;
        }

        public void Vypocitej()
        {
            float? operandA = RegistrA?.Hodnota;
            string? operace = RegistrOperatoru?.Hodnota;
            float? operandB = RegistrB?.Hodnota;

            if (operandA.HasValue && operace is not null && operandB.HasValue)
            {
                try
                {
                    switch (operace)
                    {
                        case "+":
                            RegistrV.VlozHodnotu(operandA + operandB); break;
                        case "-":
                            RegistrV.VlozHodnotu(operandA - operandB); break;
                        case "*":
                            RegistrV.VlozHodnotu(operandA * operandB); break;
                        case "/":
                            RegistrV.VlozHodnotu(operandA / operandB); break;
                    }
                }
                catch (DivideByZeroException)
                {
                    throw new DivideByZeroException("Nulou nelze dělit!");
                }
            }
        }
    }
}
