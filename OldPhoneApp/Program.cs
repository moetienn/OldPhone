using System;

namespace OldPhoneKeyPad
{
    /// <summary>
    /// Entry point of the OldPhoneKeyPad application.
    /// </summary>
    /// <remarks>
    /// Expects a single command-line argument containing the keypad input string.
    /// The decoded text is printed to the console.
    /// Example:
    /// <code>
    /// dotnet run --project OldPhoneApp "8 8877744466*664#"
    /// </code>
    /// </remarks>
	class Program
	{
        /// <summary>
        /// Application entry point.
        /// </summary>
        /// <param name="args">
        /// Command-line arguments. The first argument should be the keypad input sequence to decode.
        /// </param>
		static void Main(string[] args)
		{
            // validdate command-line arguments
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: dotnet run --project OldPhoneApp \"<input>\"");
                return;
            }
            // Decode and display the result
			Console.WriteLine(Keypad.OldPhonePad(args[0]));
		}
	}
}