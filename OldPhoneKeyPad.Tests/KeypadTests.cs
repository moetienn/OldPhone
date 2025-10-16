using Xunit;
using System;
using System.Linq;

namespace OldPhoneKeyPad.Tests
{
	public class KeypadTests
	{
		// ---------------------------
		// Edge cases: empty / null 
		// ---------------------------

		[Fact]
		public void EmptyString_ReturnsEmpty()
		{
			var output = KeypadDecoder.OldPhonePad(string.Empty);
			Assert.Equal(string.Empty, output);
		}

		[Fact]
		public void NullInput_DoesNotThrowAndReturnsEmpty()
		{
			string? input = null;
			var ex = Record.Exception(() => KeypadDecoder.OldPhonePad(input!));
			Assert.Null(ex);
			var output = KeypadDecoder.OldPhonePad(input!);
			Assert.Equal(string.Empty, output);
		}

		// ---------------------------
		// Backspace '*' related tests (grouped)
		// ---------------------------

		[Fact]
		public void Backspace_AtStart_DoesNotThrowAndReturnsEmpty()
		{
			var input = "*#";
			var output = KeypadDecoder.OldPhonePad(input);
			Assert.Equal(string.Empty, output);
		}

		[Fact]
		public void Consecutive_Stars_DoNotThrow()
		{
			var input = "**#";
			var output = KeypadDecoder.OldPhonePad(input);
			Assert.Equal(string.Empty, output);
		}

		[Fact]
		public void Star_Cancels_Pending()
		{
			var output = KeypadDecoder.OldPhonePad("2*#");
			Assert.Equal(string.Empty, output);
		}

		[Fact]
		public void Double_Star_Cancels_Pending_Then_Removes_Emitted()
		{
			var output = KeypadDecoder.OldPhonePad("2 2**#");
			Assert.Equal(string.Empty, output);
		}

		[Fact]
		public void Star_Removes_Last_Emitted_When_No_Pending()
		{
			Assert.Equal(string.Empty, KeypadDecoder.OldPhonePad("2 *#"));
		}

		// ---------------------------
		// Hash '#' related tests (grouped)
		// ---------------------------

		[Fact]
		public void Single_Hash_TerminatesProcessing()
		{
			var output = KeypadDecoder.OldPhonePad("33#2222");
			Assert.Equal("E", output); // characters after first # are ignored
		}

		[Fact]
		public void Multiple_Hash_Only_First_IsProcessed()
		{
			var output = KeypadDecoder.OldPhonePad("33#222#2");
			Assert.Equal("E", output);
		}

		[Fact]
		public void Hash_Alone_ReturnsEmpty()
		{
			Assert.Equal(string.Empty, KeypadDecoder.OldPhonePad("#"));
		}

		// ---------------------------
		// Cycle behavior tests (one test per key to cover cycles)
		// ---------------------------

		[Fact]
		public void Key1_Cycling_Behaviour()
		{
			// '1' -> ".,?!:'\"()-1" (11 items)
			Assert.Equal(".", KeypadDecoder.OldPhonePad("1#"));
			Assert.Equal(",", KeypadDecoder.OldPhonePad("11#"));
			Assert.Equal("?", KeypadDecoder.OldPhonePad("111#"));
			Assert.Equal("!", KeypadDecoder.OldPhonePad("1111#"));
			Assert.Equal(":", KeypadDecoder.OldPhonePad("11111#"));
			Assert.Equal("'", KeypadDecoder.OldPhonePad("111111#"));
			Assert.Equal("\"", KeypadDecoder.OldPhonePad("1111111#"));
			Assert.Equal("(", KeypadDecoder.OldPhonePad("11111111#"));
			Assert.Equal(")", KeypadDecoder.OldPhonePad("111111111#"));
			Assert.Equal("-", KeypadDecoder.OldPhonePad("1111111111#"));
			Assert.Equal("1", KeypadDecoder.OldPhonePad("11111111111#"));
			Assert.Equal(".", KeypadDecoder.OldPhonePad("111111111111#"));
		}

		[Fact]
		public void Key2_Cycling_Behaviour()
		{
			// '2' -> "ABC2"
			Assert.Equal("A", KeypadDecoder.OldPhonePad("2#"));
			Assert.Equal("B", KeypadDecoder.OldPhonePad("22#"));
			Assert.Equal("C", KeypadDecoder.OldPhonePad("222#"));
			Assert.Equal("2", KeypadDecoder.OldPhonePad("2222#"));
			Assert.Equal("A", KeypadDecoder.OldPhonePad("22222#"));
		}

		[Fact]
		public void Key3_Cycling_Behaviour()
		{
			// '3' -> "DEF3"
			Assert.Equal("D", KeypadDecoder.OldPhonePad("3#"));
			Assert.Equal("E", KeypadDecoder.OldPhonePad("33#"));
			Assert.Equal("F", KeypadDecoder.OldPhonePad("333#"));
			Assert.Equal("3", KeypadDecoder.OldPhonePad("3333#"));
			Assert.Equal("D", KeypadDecoder.OldPhonePad("33333#"));
		}

		[Fact]
		public void Key4_Cycling_Behaviour()
		{
			// '4' -> "GHI4"
			Assert.Equal("G", KeypadDecoder.OldPhonePad("4#"));
			Assert.Equal("H", KeypadDecoder.OldPhonePad("44#"));
			Assert.Equal("I", KeypadDecoder.OldPhonePad("444#"));
			Assert.Equal("4", KeypadDecoder.OldPhonePad("4444#"));
			Assert.Equal("G", KeypadDecoder.OldPhonePad("44444#"));
		}

		[Fact]
		public void Key5_Cycling_Behaviour()
		{
			// '5' -> "JKL5"
			Assert.Equal("J", KeypadDecoder.OldPhonePad("5#"));
			Assert.Equal("K", KeypadDecoder.OldPhonePad("55#"));
			Assert.Equal("L", KeypadDecoder.OldPhonePad("555#"));
			Assert.Equal("5", KeypadDecoder.OldPhonePad("5555#"));
			Assert.Equal("J", KeypadDecoder.OldPhonePad("55555#"));
		}

		[Fact]
		public void Key6_Cycling_Behaviour()
		{
			// '6' -> "MNO6"
			Assert.Equal("M", KeypadDecoder.OldPhonePad("6#"));
			Assert.Equal("N", KeypadDecoder.OldPhonePad("66#"));
			Assert.Equal("O", KeypadDecoder.OldPhonePad("666#"));
			Assert.Equal("6", KeypadDecoder.OldPhonePad("6666#"));
			Assert.Equal("M", KeypadDecoder.OldPhonePad("66666#"));
		}

		[Fact]
		public void Key7_Cycling_Behaviour()
		{
			// '7' -> "PQRS7"
			Assert.Equal("P", KeypadDecoder.OldPhonePad("7#"));
			Assert.Equal("Q", KeypadDecoder.OldPhonePad("77#"));
			Assert.Equal("R", KeypadDecoder.OldPhonePad("777#"));
			Assert.Equal("S", KeypadDecoder.OldPhonePad("7777#"));
			Assert.Equal("7", KeypadDecoder.OldPhonePad("77777#"));
			Assert.Equal("P", KeypadDecoder.OldPhonePad("777777#"));
		}

		[Fact]
		public void Key8_Cycling_Behaviour()
		{
			// '8' -> "TUV8"
			Assert.Equal("T", KeypadDecoder.OldPhonePad("8#"));
			Assert.Equal("U", KeypadDecoder.OldPhonePad("88#"));
			Assert.Equal("V", KeypadDecoder.OldPhonePad("888#"));
			Assert.Equal("8", KeypadDecoder.OldPhonePad("8888#"));
			Assert.Equal("T", KeypadDecoder.OldPhonePad("88888#"));
		}

		[Fact]
		public void Key9_Cycling_Behaviour()
		{
			// '9' -> "WXYZ9"
			Assert.Equal("W", KeypadDecoder.OldPhonePad("9#"));
			Assert.Equal("X", KeypadDecoder.OldPhonePad("99#"));
			Assert.Equal("Y", KeypadDecoder.OldPhonePad("999#"));
			Assert.Equal("Z", KeypadDecoder.OldPhonePad("9999#"));
			Assert.Equal("9", KeypadDecoder.OldPhonePad("99999#"));
			Assert.Equal("W", KeypadDecoder.OldPhonePad("999999#"));
		}

		[Fact]
		public void Key0_Cycling_Behaviour()
		{
			// '0' -> " 0" (space then zero)
			Assert.Equal(" ", KeypadDecoder.OldPhonePad("0#"));
			Assert.Equal("0", KeypadDecoder.OldPhonePad("00#"));
			Assert.Equal(" ", KeypadDecoder.OldPhonePad("000#"));
		}


		// ---------------------------
		// Mixed / functional sequences
		// ---------------------------

		[Fact]
		public void WeirdPrefix_With_Digits_Produces_CurrentOutput()
		{
			var input = @"\0\uFFFF\u0007abc@2020020002#";
			var output = KeypadDecoder.OldPhonePad(input);
			Assert.Equal("0PA A0A A", output);
		}

		[Fact]
		public void Classic_Hello()
		{
			var input = "4433555 555666#";
			var output = KeypadDecoder.OldPhonePad(input);
			Assert.Equal("HELLO", output);
		}

		[Fact]
		public void Decodes_Turing_Sequence()
		{
			var input = "8 8877744466*664#";
			var output = KeypadDecoder.OldPhonePad(input);
			Assert.Equal("TURING", output);
		}

		[Fact]
		public void Space_Separator_DoesNotInsertRealSpace()
		{
			var input = "22 22#"; // B + B
			var output = KeypadDecoder.OldPhonePad(input);
			Assert.Equal("BB", output);
		}

		[Fact]
		public void Unknown_Chars_Are_Ignored()
		{
			var input = "2211c122222c#";
			var output = KeypadDecoder.OldPhonePad(input);
			Assert.Equal("B?A", output);
		}

		// ---------------------------
		// Large / performance tests
		// ---------------------------

		[Fact]
		public void Very_Long_Chain_Repeats_HelloWorld_Hundred_Times()
		{
			var unit = "4433555 555666096667775553"; // HELLO WORLD
			var repeat = 100;
			var input = string.Concat(Enumerable.Repeat(unit, repeat)) + "#";
			var expected = string.Concat(Enumerable.Repeat("HELLO WORLD", repeat));
			var output = KeypadDecoder.OldPhonePad(input);
			Assert.Equal(expected, output);
		}

		[Fact]
		public void Very_Long_Chain_Repeats_HelloWorld_million_Times()
		{
			var unit = "4433555 555666096667775553";
			var repeat = 1000000;
			var input = string.Concat(Enumerable.Repeat(unit, repeat)) + "#";
			var expected = string.Concat(Enumerable.Repeat("HELLO WORLD", repeat));
			var output = KeypadDecoder.OldPhonePad(input);
			Assert.Equal(expected, output);
		}

		[Fact]
		public void Key2_100_Presses_Yields_Expected()
		{
			var n = 100;
			var input = new string('2', n) + "#";
			var mapping = KeypadMapping.LettersByKey['2']; // "ABC2"
			var expected = mapping[(n - 1) % mapping.Length].ToString();
			var actual = KeypadDecoder.OldPhonePad(input);
			Assert.Equal(expected, actual); // here expected == "2"
		}

		[Fact]
		public void Key2_10000_Presses_Yields_Expected()
		{
			var n = 10000;
			var input = new string('2', n) + "#";
			var mapping = KeypadMapping.LettersByKey['2']; // "ABC2"
			var expected = mapping[(n - 1) % mapping.Length].ToString();
			var actual = KeypadDecoder.OldPhonePad(input);
			Assert.Equal(expected, actual); // here expected == "2"
		}
	}
}