namespace OldPhoneApp
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
		static int Main(string[] args)
        {
            try
            {
                // Validate CLI args
                if (args.Length != 1)
                {
                    Console.WriteLine("Usage: dotnet run --project OldPhoneApp \"<input>\"");
                    return 1;
                }
                // Decode and print
                string output = KeypadDecoder.OldPhonePad(args[0]);
                Console.WriteLine(output);
                return 0;
            }
            catch (IOException)
            {
                // Broken pipe / stdout closed (e.g., piping to 'head' truncated the stream).
                // Exit gracefully without noisy stack traces.
                return 0;
            }
            catch (Exception ex)
            {
                // Last-resort guardrail for unexpected failures
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return 1;
            }
        }
    }
}