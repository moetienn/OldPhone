using System;
using System.Data.Common;
using System.Dynamic;
using System.Runtime.Serialization;

namespace OldPhoneKeyPad
{
    public class KeypadDictionary
    {
        public static readonly Dictionary<char, string> Map = new()
	    {
			{'1', ".,?!:'\"()-1"},
			{'2', "ABC2"},
			{'3', "DEF3"},
			{'4', "GHI4"},
			{'5', "JKL5"},
			{'6', "MNO6"},
			{'7', "PQRS7"},
			{'8', "TUV8"},
			{'9', "WXYZ9"},
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
                // Ignore all unknown character
                if (!KeypadDictionary.Map.ContainsKey(c) && c != '*' && c != '#' && c != ' ')
                    continue;
                if (c == '#')
                    break;
                if (c == '*')
                {
                    if (result.Length > 0)
                         result = result.Substring(0, result.Length);
                    count = 0;
                    lastChar = '\0';
                    continue;
                }
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