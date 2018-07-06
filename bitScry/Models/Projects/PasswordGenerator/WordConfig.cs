using bitScry.Helpers;
using bitScry.Models.Projects.PasswordGenerator.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace bitScry.Models.Projects.PasswordGenerator
{
    public class WordConfig
    {
        public WordConfig()
        {
            NumberOfWords = 3;
            WordLengthMin = 4;
            WordLengthMax = 8;
            CaseTransform = CaseTransform.Alternating;
            SeperatorAlpabet = "!@$%^&*-_+=:|~?/.;";
            PaddingDigitsBefore = 2;
            PaddingDigitsAfter = 2;
            PaddingType = Padding.Fixed;
            PadToLength = 32;
            PaddingAlphabet = "!@$%^&*-_+=:|~?/.;";
            PaddingCharactersBefore = 2;
            PaddingCharactersAfter = 2;
            NumberOfPasswords = 1;
        }

        [JsonProperty("numberOfWords")]
        [DisplayName("Number Of Words")]
        public int NumberOfWords { get; set; }

        [JsonProperty("wordLengthMin")]
        [DisplayName("Minimum Word Length")]
        public int WordLengthMin { get; set; }

        [JsonProperty("wordLengthMax")]
        [DisplayName("Maxiumum Word Length")]
        public int WordLengthMax { get; set; }

        [JsonProperty("caseTransform")]
        [DisplayName("Word Casing")]
        [JsonConverter(typeof(EnumConverter<CaseTransform>))]
        public CaseTransform CaseTransform { get; set; }

        [JsonProperty("seperatorAlphabet")]
        [DisplayName("Seperator Alphabet")]
        public string SeperatorAlpabet { get; set; }

        [JsonProperty("paddingDigitsBefore")]
        [DisplayName("Padding Digits Before")]
        public int PaddingDigitsBefore { get; set; }

        [JsonProperty("paddingDigitsAfter")]
        [DisplayName("Padding Digits After")]
        public int PaddingDigitsAfter { get; set; }

        [JsonProperty("paddingType")]
        [DisplayName("Padding Type")]
        [JsonConverter(typeof(EnumConverter<Padding>))]
        public Padding PaddingType { get; set; }

        [JsonProperty("padToLength")]
        [DisplayName("Pad To Length")]
        public int PadToLength { get; set; }

        [JsonProperty("paddingAlphabet")]
        [DisplayName("Padding Alphabet")]
        public string PaddingAlphabet { get; set; }

        [JsonProperty("paddingCharactersBefore")]
        [DisplayName("Padding Characters Before")]
        public int PaddingCharactersBefore { get; set; }

        [JsonProperty("paddingCharactersAfter")]
        [DisplayName("Padding Characters After")]
        public int PaddingCharactersAfter { get; set; }

        [JsonProperty("numberOfPasswords")]
        [DisplayName("Number Of Passwords")]
        public int NumberOfPasswords { get; set; }
    }
}
