using System;
using System.Collections.Generic;

namespace Kalkulacka
{
    /// <summary>
    /// Reprezentuje aritmetickou jednotku provádějící výpočty kalkulačky.
    /// </summary>
    internal class AritmetickaJednotka
    {
        /// <summary>
        /// List operátorů, které umí aritmetická jednotka realizovat.
        /// </summary>
        public static List<string> Operatory { get; } = new() { "+", "-", "*", "/" };
        /// <summary>
        /// Registr pro první operand.
        /// </summary>
        private RegistrFloat RegistrA { get; set; }
        /// <summary>
        /// Registr pro druhý operand.
        /// </summary>
        private RegistrFloat RegistrB { get; set; }
        /// <summary>
        /// Registr pro výsledek.
        /// </summary>
        private RegistrFloat RegistrV { get; set; }
        /// <summary>
        /// Registr pro operátor.
        /// </summary>
        private RegistrOperatoru RegistrOperatoru { get; set; }

        public AritmetickaJednotka(RegistrFloat registrA, RegistrFloat registrB, RegistrFloat registrV, RegistrOperatoru registrOperatoru)
        {
            RegistrA = registrA;
            RegistrB = registrB;
            RegistrV = registrV;
            RegistrOperatoru = registrOperatoru;
        }

        /// <summary>
        /// Provede výpočet.
        /// </summary>
        /// <exception cref="DivideByZeroException"></exception>
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
