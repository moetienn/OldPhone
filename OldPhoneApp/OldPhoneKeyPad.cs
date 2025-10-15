using System;
using System.Data.Common;
using System.Dynamic;
using System.Runtime.Serialization;

namespace OldPhoneKeyPad
{
    /// <summary>
    /// Defines the mapping between numeric keys and their corresponding
    /// characters on a traditional mobile phone keypad.
    /// </summary>
    /// <remarks>
    /// Each numeric key (2–9) maps to a sequence of uppercase letters followed by the digit itself.
    /// The '1' key maps to punctuation marks, and the '0' key maps to a space character.
    /// </remarks>
    public class KeypadDictionary
    {
        /// <summary>
        /// Provides the numeric-to-character mapping data used by <see cref="Keypad"/>.
        /// </summary>
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
    /// <summary>
    /// Decodes a sequence produced by a traditional multi-tap mobile keypad.
    /// </summary>
    /// <remarks>
    /// Rules:
    /// - Digits 1–9: cycle through their mapped characters (e.g., '2' -> "ABC2").
    /// - '0': inserts a real space after flushing any pending sequence.
    /// - ' ': separator that flushes the pending sequence without inserting a space.
    /// - '*': backspace; if a sequence is pending it is canceled, otherwise removes the last emitted character.
    /// - '#': end; flushes any pending sequence and stops.
    /// Any unknown characters are ignored.
    /// </remarks>
	class Keypad
	{
        /// <summary>
        /// Decodes the specified keypad input string into text.
        /// </summary>
        /// <param name="input">The raw keypad input consisting of digits and control characters.</param>
        /// <returns>The decoded text.</returns>
		public static String OldPhonePad(string input)
		{
			string result = "";
			char lastChar = '\0';
			int count = 0;
			for (int i = 0; i < input.Length; i++)
       		{
				char c = input[i];
                // Ignore anything that is not handled (not space, *, #, and not a known key)
                if (!KeypadDictionary.Map.ContainsKey(c) && c != '*' && c != '#' && c != ' ')
                    continue;
                // End: stop processing after flushing pending sequence
                if (c == '#')
                    break;
                // Backspace: cancel pending sequence if any; otherwise remove last emitted char
                if (c == '*')
                {
                    if (count > 0)
                    {
                        lastChar = '\0';
                        count = 0;
                    }
                    else if (result.Length > 0)
                        result = result.Substring(0, result.Length - 1);
                    continue;
                }
                // ' ' space acts as a separator: confirms the current letter without adding a visible space.
                if (c == ' ')
                {
                    if (count > 0)
                    {
                        result += GetLetterFromDictionary(lastChar, count);
                        lastChar = '\0';
                        count = 0;    
                    }
                    continue;
                }
                // Handles multi-tap behavior: consecutive presses of the same key cycle through its letters.
                // When a different key is pressed, the previous letter is confirmed and a new sequence begins.
                if (c == lastChar)
                    count++;
                else
                {
                    if (lastChar != '\0')
                        result += GetLetterFromDictionary(lastChar, count);
                    lastChar = c;
                    count = 1;
                }
        	}
            // Final flush to ensure the last sequence of key presses is processed before returning the final decoded text.
            if (count > 0)
                result += GetLetterFromDictionary(lastChar, count);
			return result;
		}

        /// <summary>
        /// Returns the decoded character for a given key and press count.
        /// </summary>
        /// <param name="key">The keypad key.</param>
        /// <param name="count">The number of times the key was pressed consecutively.</param>
        /// <returns>The corresponding character.</returns>
        private static char GetLetterFromDictionary(char key, int count)
        {
            if (!KeypadDictionary.Map.TryGetValue(key, out var letters) || letters.Length == 0)
                return key;

            int index = (count - 1) % letters.Length;
                return letters[index];
        }
	}
}