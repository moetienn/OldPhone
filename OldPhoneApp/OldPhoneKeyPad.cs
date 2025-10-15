using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Immutable;



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
    public class KeypadMapping
    {
        /// <summary>
        /// Provides the numeric-to-character mapping data used by <see cref="Keypad"/>.
        /// </summary>
        public static readonly ImmutableDictionary<char, string> LettersByKey =
            ImmutableDictionary.CreateRange(new[]
            {
                new KeyValuePair<char,string>('1', ".,?!:'\"()-1"),
                new KeyValuePair<char,string>('2', "ABC2"),
                new KeyValuePair<char,string>('3', "DEF3"),
                new KeyValuePair<char,string>('4', "GHI4"),
                new KeyValuePair<char,string>('5', "JKL5"),
                new KeyValuePair<char,string>('6', "MNO6"),
                new KeyValuePair<char,string>('7', "PQRS7"),
                new KeyValuePair<char,string>('8', "TUV8"),
                new KeyValuePair<char,string>('9', "WXYZ9"),
                new KeyValuePair<char,string>('0', " ")
            });
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
	public static class KeypadDecoder
	{
        /// <summary>
        /// Decodes the specified keypad input string into text.
        /// </summary>
        /// <param name="input">The raw keypad input consisting of digits and control characters.</param>
        /// <returns>The decoded text.</returns>
        /// /// <remarks>
        /// Assumes that every input sequence ends with a '#' character, as specified in the requirements.
        /// The '#' character signals the end of input and stops processing.
        /// </remarks>
		public static String OldPhonePad(string input)
		{
			var  result = new StringBuilder();
			char lastKey = '\0';
			int pressCount = 0;
			for (int i = 0; i < input.Length; i++)
       		{
				char c = input[i];
                // Ignore anything that is not handled (not space, *, #, and not a known key)
                if (!KeypadMapping.LettersByKey.ContainsKey(c) && c != '*' && c != '#' && c != ' ')
                    continue;
                // End: stop processing after flushing pending sequence
                if (c == '#')
                    break;
                // Backspace: cancel pending sequence if any; otherwise remove last emitted char
                if (c == '*')
                {
                    if (pressCount > 0)
                    {
                        lastKey = '\0';
                        pressCount = 0;
                    }
                    else if ( result.Length > 0)
                         result.Remove( result.Length - 1, 1);
                    continue;
                }
                // ' ' space acts as a separator: confirms the current letter without adding a visible space.
                if (c == ' ')
                {
                    if (pressCount > 0)
                    {
                        result.Append(GetMappedChar(lastKey, pressCount));
                        lastKey = '\0';
                        pressCount = 0;    
                    }
                    continue;
                }
                // Handles multi-tap behavior: consecutive presses of the same key cycle through its letters.
                // When a different key is pressed, the previous letter is confirmed and a new sequence begins.
                if (c == lastKey)
                    pressCount++;
                else
                {
                    if (lastKey != '\0')
                         result.Append(GetMappedChar(lastKey, pressCount));
                    lastKey = c;
                    pressCount = 1;
                }
        	}
            // Final flush to ensure the last sequence of key presses is processed before returning the final decoded text.
            if (pressCount > 0)
                 result.Append(GetMappedChar(lastKey, pressCount));
			return  result.ToString();
		}
        /// <summary>
        /// Returns the decoded character for a given key and press count.
        /// </summary>
        /// <param name="key">The keypad key.</param>
        /// <param name="pressCount">The number of times the key was pressed consecutively.</param>
        /// <returns>The corresponding character.</returns>
        private static char GetMappedChar(char key, int pressCount)
        {
            if (!KeypadMapping.LettersByKey.TryGetValue(key, out var letters) || letters.Length == 0)
                return key;

            int index = (pressCount - 1) % letters.Length;
                return letters[index];
        }
	}
}