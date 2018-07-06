using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.Models.Projects.PasswordGenerator
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
        [DisplayName("Include Uppercase Characters")]
        public bool Uppercase { get; set; }

        [JsonProperty("lowercase")]
        [DisplayName("Include Lowercase Characters")]
        public bool Lowercase { get; set; }

        [JsonProperty("numbers")]
        [DisplayName("Include Numeric Characters")]
        public bool Numbers { get; set; }

        [JsonProperty("specialCharacters")]
        [DisplayName("Include Special Characters")]
        public bool SpecialCharacters { get; set; }

        [JsonProperty("uppercaseAlphabet")]
        [DisplayName("Uppercase Alphabet")]
        public string UppercaseAlphabet { get; set; }

        [JsonProperty("lowercaseAlphabet")]
        [DisplayName("Lowercase Alphabet")]
        public string LowercaseAlphabet { get; set; }

        [JsonProperty("numericAlphabet")]
        [DisplayName("Numeric Alphabet")]
        public string NumericAlphabet { get; set; }

        [JsonProperty("specialCharacterAlphabet")]
        [DisplayName("Special Character Alphabet")]
        public string SpecialCharacterAlphabet { get; set; }

        [JsonProperty("length")]
        [DisplayName("Length")]
        public int Length { get; set; }

        [JsonProperty("numberOfPasswords")]
        [DisplayName("Number Of Passwords")]
        public int NumberOfPasswords { get; set; }
    }
}
