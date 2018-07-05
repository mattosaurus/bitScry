using bitScry.Function.Models;
using bitScry.Function.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using bitScry.Function.Models.PasswordGenerator;
using bitScry.Function.Models.PasswordGenerator.Types;

namespace bitScry.Function.AppCode
{
    public static class Common
    {
        public static string GenerateWordPassword(WordConfig config, List<string> baseWordList)
        {
            string password = "";

            // Select all words of desired length
            List<string> filteredWordList = baseWordList.Where(x => x.Length >= config.WordLengthMin && x.Length <= config.WordLengthMax).ToList();

            // Create random
            Random random = new Random();

            // Select desired number of words
            List<string> finalWordList = filteredWordList.GetRandomElement(config.NumberOfWords).ToList();
            //List<string> finalWordList = filteredWordList.Shuffle(random).Take(config.NumberOfWords).ToList();

            // Update word casing
            if (config.CaseTransform == CaseTransform.Alternating)
            {
                for (int i = finalWordList.Count - 1; i >= 0; i--)
                {
                    if (i % 2 == 0)
                    {
                        finalWordList[i] = finalWordList[i].ToLower();
                    }
                    else
                    {
                        finalWordList[i] = finalWordList[i].ToUpper();
                    }
                }
            }
            else if (config.CaseTransform == CaseTransform.Capitalise)
            {
                finalWordList = finalWordList.Select(x => x.First().ToString().ToUpper() + x.Substring(1).ToLower()).ToList();
            }
            else if (config.CaseTransform == CaseTransform.Invert)
            {
                finalWordList = finalWordList.Select(x => x.First().ToString().ToLower() + x.Substring(1).ToUpper()).ToList();
            }
            else if (config.CaseTransform == CaseTransform.Lower)
            {
                finalWordList = finalWordList.Select(x => x.ToLower()).ToList();
            }
            else if (config.CaseTransform == CaseTransform.None)
            {
            }
            else if (config.CaseTransform == CaseTransform.Random)
            {
                for (int i = finalWordList.Count - 1; i >= 0; i--)
                {
                    if (random.Next(0, 2) == 0)
                    {
                        finalWordList[i] = finalWordList[i].ToLower();
                    }
                    else
                    {
                        finalWordList[i] = finalWordList[i].ToUpper();
                    }
                }
            }
            else if (config.CaseTransform == CaseTransform.Upper)
            {
                finalWordList = finalWordList.Select(x => x.ToUpper()).ToList();
            }

            // Get word seperator
            string seperator = "";

            if (config.SeperatorAlpabet.Length > 0)
            {
                seperator = config.SeperatorAlpabet.ToArray().GetRandomElement(1).FirstOrDefault().ToString();
            }

            // Padding
            string startPadding = "";
            string endPadding = "";

            // Number padding
            if (config.PaddingDigitsBefore > 0)
            {
                for (int i = 0; i < config.PaddingDigitsBefore; i++)
                {
                    startPadding += Common.GetRandomInteger(0, 9).ToString();
                }
            }

            if (config.PaddingDigitsAfter > 0)
            {
                for (int i = 0; i < config.PaddingDigitsAfter; i++)
                {
                    endPadding += Common.GetRandomInteger(0, 9).ToString();
                }
            }

            // Symbol padding
            if (config.PaddingType == Padding.Fixed)
            {
                char paddingCharacter = config.PaddingAlphabet.ToArray().GetRandomElement(1).FirstOrDefault();

                if (config.PaddingCharactersBefore > 0)
                {
                    startPadding = startPadding.PadLeft(config.PaddingDigitsBefore + config.PaddingCharactersBefore, paddingCharacter);
                }

                if (config.PaddingCharactersAfter > 0)
                {
                    endPadding = endPadding.PadRight(config.PaddingDigitsAfter + config.PaddingCharactersAfter, paddingCharacter);
                }
            }
            else if (config.PaddingType == Padding.Adaptive)
            {
                char paddingCharacter = config.PaddingAlphabet.ToArray().GetRandomElement(1).FirstOrDefault();

                int passwordLength = config.PaddingDigitsBefore + finalWordList.Sum(x => x.Length) + config.PaddingDigitsAfter + config.NumberOfWords;

                if (config.PadToLength > passwordLength)
                {
                    endPadding = endPadding.PadRight(config.PadToLength - passwordLength, paddingCharacter);
                }
            }

            if (!string.IsNullOrEmpty(startPadding))
            {
                finalWordList.Insert(0, startPadding);
            }

            if (!string.IsNullOrEmpty(endPadding))
            {
                finalWordList.Add(endPadding);
            }

            password = string.Join(seperator, finalWordList);

            return password;
        }

        public static string GenerateStringPassword(StringConfig config)
        {
            string password = "";

            // Select list of all desired characters
            string chars = "";

            if (config.Uppercase)
            {
                chars += string.Join("", config.UppercaseAlphabet.ToUpper().ToList().Distinct());
            }

            if (config.Lowercase)
            {
                chars += string.Join("", config.LowercaseAlphabet.ToLower().ToList().Distinct());
            }

            if (config.Numbers)
            {
                chars += string.Join("", config.NumericAlphabet.ToList().Distinct());
            }

            if (config.SpecialCharacters)
            {
                chars += string.Join("", config.SpecialCharacterAlphabet.ToList().Distinct());
            }

            password = GetRandomString(config.Length, chars);

            return password;
        }

        public static string GetRandomString(int length, string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_")
        {
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                byte[] data = new byte[length];

                // If chars.Length isn't a power of 2 then there is a bias if we simply use the modulus operator. The first characters of chars will be more probable than the last ones.
                // buffer used if we encounter an unusable random byte. We will regenerate it in this buffer
                byte[] buffer = null;

                // Maximum random number that can be used without introducing a bias
                int maxRandom = byte.MaxValue - ((byte.MaxValue + 1) % chars.Length);

                crypto.GetBytes(data);

                char[] result = new char[length];

                for (int i = 0; i < length; i++)
                {
                    byte value = data[i];

                    while (value > maxRandom)
                    {
                        if (buffer == null)
                        {
                            buffer = new byte[1];
                        }

                        crypto.GetBytes(buffer);
                        value = buffer[0];
                    }

                    result[i] = chars[value % chars.Length];
                }

                return new string(result);
            }
        }

        public static int GetRandomInteger(int minValue = 0, int maxValue = int.MaxValue)
        {
            if (maxValue < minValue)
            {
                throw new ArgumentOutOfRangeException("Maximum value must be greater than minimum value");
            }
            else if (maxValue == minValue)
            {
                return 0;
            }

            Int64 diff = maxValue - minValue;

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                while (true)
                {
                    byte[] fourBytes = new byte[4];
                    crypto.GetBytes(fourBytes);

                    // Convert that into an uint.
                    UInt32 scale = BitConverter.ToUInt32(fourBytes, 0);

                    Int64 max = (1 + (Int64)UInt32.MaxValue);
                    Int64 remainder = max % diff;
                    if (scale < max - remainder)
                    {
                        return (Int32)(minValue + (scale % diff));
                    }
                }
            }
        }
    }
}
