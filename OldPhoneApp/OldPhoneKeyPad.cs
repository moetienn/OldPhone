using System;
using System.Data.Common;
using System.Dynamic;

namespace OldPhoneKeyPad
{
    public class KeypadDictionary
    {
        public static readonly Dictionary<char, string> Map = new()
	    {
			{'1', ""},
			{'2', "ABC"},
			{'3', "DEF"},
			{'4', "GHI"},
			{'5', "JKL"},
			{'6', "MNO"},
			{'7', "PQRS"},
			{'8', "TUV"},
			{'9', "WXYZ"},
			{'0', " "}
		};
	}
	class Keypad
	{
		public static String OldPhonePad(string input)
		{
			string result = "";
			char lastChar = '\0';
			int count = 0;
			for (int i = 0; i < input.Length; i++)
       		{
				char c = input[i];
                if (c == '#')
                    break;
                if (c == ' ')
                {
                    result += GetLetterFromDictionnary(lastChar, count);
                    lastChar = '\0';
                    count = 0;
                    continue;
                }
                if (c == lastChar)
                    count++;
                else
                {
                    if (lastChar != '\0')
                        result += GetLetterFromDictionnary(lastChar, count);
                    lastChar = c;
                    count = 1;
                }
        	}

            if (lastChar != '\0')
                result += GetLetterFromDictionnary(lastChar, count);
			return result;
		}

        private static char GetLetterFromDictionnary(char key, int count)
        {
            if (!KeypadDictionary.Map.TryGetValue(key, out var letters) || letters.Length == 0)
                return key;

            int index = (count - 1) % letters.Length;
                return letters[index];
        }
	}
}