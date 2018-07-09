using bitScry.Function.Helpers;
using bitScry.Function.Models.PasswordGenerator.Types;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace bitScry.Function.Models.PasswordGenerator
{
    public class WordConfig
    {
        public WordConfig()
        {
            NumberOfWords = 3;
            WordLengthMin = 4;
            WordLengthMax = 8;
            CaseTransform = CaseTransform.Alternating;
            SeperatorAlphabet = "!@$%^&*-_+=:|~?/.;";
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
        public int NumberOfWords { get; set; }

        [JsonProperty("wordLengthMin")]
        public int WordLengthMin { get; set; }

        [JsonProperty("wordLengthMax")]
        public int WordLengthMax { get; set; }

        [JsonProperty("caseTransform")]
        [JsonConverter(typeof(EnumConverter<CaseTransform>))]
        public CaseTransform CaseTransform { get; set; }

        [JsonProperty("seperatorAlphabet")]
        public string SeperatorAlphabet { get; set; }

        [JsonProperty("paddingDigitsBefore")]
        public int PaddingDigitsBefore { get; set; }

        [JsonProperty("paddingDigitsAfter")]
        public int PaddingDigitsAfter { get; set; }

        [JsonProperty("paddingType")]
        [JsonConverter(typeof(EnumConverter<Padding>))]
        public Padding PaddingType { get; set; }

        [JsonProperty("padToLength")]
        public int PadToLength { get; set; }

        [JsonProperty("paddingAlphabet")]
        public string PaddingAlphabet { get; set; }

        [JsonProperty("paddingCharactersBefore")]
        public int PaddingCharactersBefore { get; set; }

        [JsonProperty("paddingCharactersAfter")]
        public int PaddingCharactersAfter { get; set; }

        [JsonProperty("numberOfPasswords")]
        public int NumberOfPasswords { get; set; }
    }
}
