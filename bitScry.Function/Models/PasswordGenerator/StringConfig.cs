using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bitScry.Function.Models.PasswordGenerator
{
    public class StringConfig
    {
        public StringConfig()
        {
            Uppercase = true;
            Lowercase = true;
            Numbers = true;
            SpecialCharacters = true;
            UppercaseAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            LowercaseAlphabet = "abcdefghijklmnopqrstuvwxyz";
            NumericAlphabet = "0123456789";
            SpecialCharacterAlphabet = "!@$%^&*-_+=:|~?/.;";
            Length = 24;
            NumberOfPasswords = 1;
        }

        [JsonProperty("uppercase")]
        public bool Uppercase { get; set; }

        [JsonProperty("lowercase")]
        public bool Lowercase { get; set; }

        [JsonProperty("numbers")]
        public bool Numbers { get; set; }

        [JsonProperty("specialCharacters")]
        public bool SpecialCharacters { get; set; }

        [JsonProperty("uppercaseAlphabet")]
        public string UppercaseAlphabet { get; set; }

        [JsonProperty("lowercaseAlphabet")]
        public string LowercaseAlphabet { get; set; }

        [JsonProperty("numericAlphabet")]
        public string NumericAlphabet { get; set; }

        [JsonProperty("specialCharacterAlphabet")]
        public string SpecialCharacterAlphabet { get; set; }

        [JsonProperty("length")]
        public int Length { get; set; }

        [JsonProperty("numberOfPasswords")]
        public int NumberOfPasswords { get; set; }
    }
}
