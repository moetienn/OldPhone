using Xunit;


namespace OldPhoneKeyPad.Tests
{
    public class KeypadTests
    {
        [Fact]
        public void Decodes_Turing_Sequence()
        {
            var input = "8 8877744466*664#";
            var output = Keypad.OldPhonePad(input);
            Assert.Equal("TURING", output);
        }

        [Fact]
        public void Space_Separator_DoesNotInsertRealSpace()
        {
            var input = "22 22#"; // B + B
            var output = Keypad.OldPhonePad(input);
            Assert.Equal("BB", output);
        }

        [Fact]
        public void Zero_Inserts_Real_Space()
        {
            var input = "44 4440 33#"; // HI + space + E
            var output = Keypad.OldPhonePad(input);
            Assert.Equal("HI E", output);
        }

        [Fact]
        public void Classic_Hello()
        {
            var input = "4433555 555666#"; // HELLO
            var output = Keypad.OldPhonePad(input);
            Assert.Equal("HELLO", output);
        }

        [Fact]
        public void Backspace_Cancels_Pending_Or_Removes_Emitted()
        {
            var input = "66*664#";
            var output = Keypad.OldPhonePad(input);
            // 66 (pending) -> * cancels -> 66 -> N, then 4 -> G => "NG"
            Assert.Equal("NG", output);
        }

        [Fact]
        public void Unknown_Chars_Are_Ignored()
        {
            var input = "2211c122222c#";
            var output = Keypad.OldPhonePad(input);
            // "22" -> B, "111" -> ?, "22222" -> A  => "B?A"
            Assert.Equal("B?A", output);
        }

        [Fact]
        public void Key1_Punctuation_Wraps()
        {
            var input = "111#"; // 1 -> ".,?!:'\"()-1" => third press is '?'
            var output = Keypad.OldPhonePad(input);
            Assert.Equal("?", output);
        }

        [Fact]
        public void Cycling_Past_End_Emits_Digit_Then_Wraps()
        {
            var inputA = "2222#";   // "ABC2" -> 4th press => '2'
            var outA = Keypad.OldPhonePad(inputA);
            Assert.Equal("2", outA);

            var inputB = "22222#";  // 5th press wraps to 'A'
            var outB = Keypad.OldPhonePad(inputB);
            Assert.Equal("A", outB);
        }

        [Fact]
        public void Very_Long_Chain_Repeats_HelloWorld_Hundred_Times()
        {
            var unit = "4433555 555666096667775553"; // HELLO WORLD
            var repeat = 100;
            var input = string.Concat(Enumerable.Repeat(unit, repeat)) + "#";
            var expected = string.Concat(Enumerable.Repeat("HELLO WORLD", repeat));

            var output = Keypad.OldPhonePad(input);
            Assert.Equal(expected, output);
        }

        [Fact]
        public void Backspace_AtStart_DoesNotThrowAndReturnsEmpty()
        {
            var input = "*#"; // backspace before any char -> still empty
            var output = Keypad.OldPhonePad(input);
            Assert.Equal(string.Empty, output);
        }

        [Fact]
        public void Hash_TerminatesInput_IgnoresFollowingPresses()
        {
            var input = "33#2222"; // after # decoding should stop
            var output = Keypad.OldPhonePad(input);
            Assert.Equal("E", output);
        }

        [Fact]
        public void NullInput_ThrowsNullReferenceException()
        {
            var ex = Assert.Throws<NullReferenceException>(() => Keypad.OldPhonePad(null!));
            Assert.Equal("Object reference not set to an instance of an object.", ex.Message);
        }
    }
}
