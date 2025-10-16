using System.Collections.Generic;
using System.Collections.Immutable;

namespace OldPhoneApp
{
    /// <summary>
    /// Defines the mapping between numeric keys and their corresponding
    /// characters on a traditional mobile phone keypad.
    /// </summary>
    /// <remarks>
    /// Each numeric key (2–9) maps to a sequence of uppercase letters followed by the digit itself.
    /// The '1' key maps to punctuation marks, and the '0' key maps to a space character.
    /// </remarks>
    public static class KeypadMapping
    {
        /// <summary>
        /// Provides the numeric-to-character mapping data used by <see cref="KeypadDecoder"/>.
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
                new KeyValuePair<char,string>('0', " 0")
            });
	}
}