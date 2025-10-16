using System;
using System.Text;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks.Dataflow;

namespace OldPhoneKeyPad
{
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

                switch (c)
                {
                    case '#':
                        // flush once and return immediately
                        FlushPending(result, ref lastKey, ref pressCount);
                        return result.ToString();
                    case '*':
                        HandleBackspace(result, ref lastKey, ref pressCount);
                        break;
                    case ' ':
                        HandleSpace(result, ref lastKey, ref pressCount);
                        break;
                    default:
                        if (KeypadMapping.LettersByKey.ContainsKey(c))
                            HandleMappedKey(result, ref lastKey, ref pressCount, c);
                        break;
                }
            }
            // Final flush in case the input ended without '#'
            FlushPending(result, ref lastKey, ref pressCount);
            return result.ToString();
        }

        /// <summary>
        /// Handler for the backspace '*' control.
        /// Cancels a pending sequence if present; otherwise removes the last emitted character.
        /// </summary>
        private static void HandleBackspace(StringBuilder result, ref char lastKey, ref int pressCount)
        {
            if (pressCount > 0)
            {
                lastKey = '\0';
                pressCount = 0;
            }
            else if (result.Length > 0)
            {
                result.Remove(result.Length - 1, 1);
            }
        }

        /// <summary>
        /// Handler for the separator space ' '.
        /// Confirms the pending sequence without inserting a visible space.
        /// </summary>
        private static void HandleSpace(StringBuilder result, ref char lastKey, ref int pressCount)
        {
            FlushPending(result, ref lastKey, ref pressCount);
        }

        /// <summary>
        /// Handler for mapped numeric keys (2-9, 0, 1).
        /// Implements multi-tap behavior: consecutive presses of the same key increment the counter;
        /// pressing a different key flushes the previous pending character and starts a new sequence.
        /// </summary>
        private static void HandleMappedKey(StringBuilder result, ref char lastKey, ref int pressCount, char c)
        {
            if (lastKey != '\0' && lastKey == c)
            {
                pressCount++;
            }
            else
            {
                FlushPending(result, ref lastKey, ref pressCount);
                lastKey = c;
                pressCount = 1;
            }
        }

        /// <summary>
        /// Flushes the pending key sequence into the result, resetting pending state.
        /// </summary>
        private static void FlushPending(StringBuilder result, ref char lastKey, ref int pressCount)
        {
            if (lastKey != '\0' && pressCount > 0)
            {
                result.Append(GetMappedChar(lastKey, pressCount));
                lastKey = '\0';
                pressCount = 0;
            }
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