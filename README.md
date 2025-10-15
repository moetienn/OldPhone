# OldPhone

A small command-line app that decodes input from a traditional multi-tap mobile phone keypad into text.

It implements the typical mapping (2 => ABC2, 3 => DEF3, etc.), handles separators and backspace, and stops when it sees `#`. The included tests show expected behaviour and edge cases.

## Features

- Decodes multi-tap keypad sequences into uppercase text.
- Handles:
  - `0` — inserts a real space.
  - `' '` (space) — acts as a separator to confirm the current letter (no visible space).
  - `*` — backspace: cancels the current pending sequence or removes the last emitted character.
  - `#` — end of input: flushes pending sequence and stops processing.
  - Unknown characters are ignored.
- Mapping includes punctuation on `1` and the digit itself at the end of each key's cycle (e.g., `2` -> `ABC2`).

## Requirements

- .NET SDK 6.0 or later (the project targets .NET; any recent SDK should work).

## Build

Restore and build the solution from the repository root:

```bash
dotnet build
```

You can also restore first (usually not necessary with `dotnet build`):

```bash
dotnet restore
dotnet build
```

## Usage

The program expects a single command-line argument containing the keypad input sequence. The input should end with `#` to mark the end of input (characters after `#` are ignored).

Run directly from source:

```bash
dotnet run --project OldPhoneApp "<input>"
```

Examples:

```bash
dotnet run --project OldPhoneApp "8 8877744466*664#"
# Output: TURING

dotnet run --project OldPhoneApp "4433555 555666#"
# Output: HELLO

dotnet run --project OldPhoneApp "44 4440 33#"
# Output: HI E
```

Exit codes:
- 0 — success (or graceful exit when stdout is closed).
- 1 — incorrect usage or unexpected error.

## Tests

Run the unit tests from the repository root:

```bash
dotnet test
```

Tests cover the main behaviours: decoding examples, separators, backspace rules, ignoring unknown chars, cycling and wrapping, and large inputs.

## Key mapping (summary)

- `1` -> ".,?!:'\"()-1"
- `2` -> "ABC2"
- `3` -> "DEF3"
- `4` -> "GHI4"
- `5` -> "JKL5"
- `6` -> "MNO6"
- `7` -> "PQRS7"
- `8` -> "TUV8"
- `9` -> "WXYZ9"
- `0` -> " 0" (space and zero)

Pressing a key cycles through the characters for that key; pressing more times than the number of characters wraps around.
