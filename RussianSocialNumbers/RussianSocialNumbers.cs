﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RussianSocialNumbers
{
    /*
     Класс для проверки корректности контрольной суммы ИНН, СНИЛС 
     */
    public static class RussianSocialNumbers
    {
        /*
        Проверка СНИЛС
         */
        public static bool IsSnilsValid(string snils)
        {
            if (string.IsNullOrEmpty(snils)
                || !System.Text.RegularExpressions.Regex.IsMatch(snils, @"^\d+$")
                || snils.Length != 11)
            {
                return false;
            }

            var s = 0;

            for (var i = 0; i < 9; i++)
            {
                s += Convert.ToInt32(snils.Substring(i, 1)) * (9 - i);
            }

            var checkSum = 0;
            if (s < 100)
                checkSum = s;
            else if (s > 101)
            {
                checkSum = s % 101;
                if (checkSum == 100)
                    checkSum = 0;
            }

            return checkSum == Convert.ToInt32(snils.Substring(9));
        }


        /*
         Проверка ИНН
         */
        public static bool IsInnValid(string inn)
        {
            if (string.IsNullOrEmpty(inn)
                || !System.Text.RegularExpressions.Regex.IsMatch(inn, @"^\d+$")
                || (inn.Length != 10 && inn.Length != 12))
            {
                return false;
            }

            if (inn.Length == 10)
            {
                return (Convert.ToInt32(inn.Substring(9, 1)) == CheckSumINN(inn, new int[] { 2, 4, 10, 3, 5, 9, 4, 6, 8 }));
            }

            if (inn.Length == 12)
            {
                var s11 = CheckSumINN(inn, new int[] { 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 });
                var s12 = CheckSumINN(inn, new int[] { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 });
                return s11 == Convert.ToInt32(inn.Substring(10, 1)) && s12 == Convert.ToInt32(inn.Substring(11, 1));
            }

            return false;
        }

        private static int CheckSumINN(string inn, int[] coefficients)
        {
            var s = 0;
            for (var i = 0; i < coefficients.Length; i++)
            {
                s += coefficients[i] * Convert.ToInt32(inn.Substring(i, 1));
            }

            return (s % 11) % 10;
        }
    }
}
