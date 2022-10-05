using System.Security.Cryptography;

namespace DotCart;

public static class PasswordUtils
{
    // Define default min and max password lengths.
    /// <summary>
    ///     The defaul t_ mi n_ passwor d_ length
    /// </summary>
    private static readonly int DefaultMinPasswordLength = 8;

    /// <summary>
    ///     The defaul t_ ma x_ passwor d_ length
    /// </summary>
    private static readonly int DefaultMaxPasswordLength = 10;
    private static readonly string PasswordCharsLcase = "abcdefgijkmnopqrstwxyz";
    private static readonly string PasswordCharsUcase = "ABCDEFGHJKLMNPQRSTWXYZ";
    private static readonly string PasswordCharsNumeric = "23456789";
    private static readonly string PasswordCharsSpecial = "*$-+?_&=!%{}/";
    public static bool IsEncodedSameAsPassword(string encoded, string plainText)
    {
        if (string.IsNullOrEmpty(encoded) && string.IsNullOrEmpty(plainText)) return true;
        if (encoded == null) return false;
        if (encoded.Length < 32) return false;
        var md5 = encoded[^32..]; // MD5 is 32 char length.
        var saltHex = encoded[..^md5.Length];
        var salt = HexUtils.GetStringFromHex(saltHex);
        return encoded.Equals(saltHex + (salt + plainText).MD5Encode(), StringComparison.InvariantCultureIgnoreCase);
    }
    public static string Generate() {
        return Generate(DefaultMinPasswordLength, DefaultMaxPasswordLength, true);
    }
    public static string Generate(int length) {
        return Generate(length, length, true);
    }
    public static string Generate(bool useSpecialsChars) {
        return Generate(DefaultMinPasswordLength, DefaultMaxPasswordLength, useSpecialsChars);
    }
    public static string Generate(int length, bool useSpecialsChars) {
        return Generate(length, length, useSpecialsChars);
    }
    public static string Generate(int minLength, int maxLength, bool useSpecialsChars) {
        char[] password = null; // This array will hold password characters.
        char[][] charGroups; // Create a local array containing supported password characters grouped by types.

        // Make sure that input parameters are valid.
        if (minLength <= 0 || maxLength <= 0 || minLength > maxLength) return null;

        // Create a local array containing supported password characters
        // grouped by types. You can remove character groups from this
        // array, but doing so will weaken the password strength.
        if (useSpecialsChars)
            charGroups = new[]
            {
                PasswordCharsLcase.ToCharArray(),
                PasswordCharsUcase.ToCharArray(),
                PasswordCharsNumeric.ToCharArray(),
                PasswordCharsSpecial.ToCharArray()
            };
        else
            charGroups = new[]
            {
                PasswordCharsLcase.ToCharArray(),
                PasswordCharsUcase.ToCharArray(),
                PasswordCharsNumeric.ToCharArray()
            };

        // Use this array to track the number of unused characters in each
        // character group.
        var charsLeftInGroup = new int[charGroups.Length];

        // Initially, all characters in each group are not used.
        for (var i = 0; i < charsLeftInGroup.Length; i++) charsLeftInGroup[i] = charGroups[i].Length;
        // Use this array to track (iterate through) unused character groups.
        var leftGroupsOrder = new int[charGroups.Length];
        // Initially, all character groups are not used.
        for (var i = 0; i < leftGroupsOrder.Length; i++) leftGroupsOrder[i] = i;
        var randomBytes = new byte[4];
        // Generate 4 random bytes.
        var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        // Convert 4 bytes into a 32-bit integer value.
        var seed = ((randomBytes[0] & 0x7f) << 24) |
                   (randomBytes[1] << 16) |
                   (randomBytes[2] << 8) |
                   randomBytes[3];

        // Now, this is real randomization.
        var random = new Random(seed);
        // Allocate appropriate memory for the password.
        password = minLength < maxLength ? new char[random.Next(minLength, maxLength + 1)] : new char[minLength];

        // Index of the last non-processed group.
        var lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;

        // Generate password characters one at a time.
        for (var i = 0; i < password.Length; i++)
        {
            // If only one character group remained unprocessed, process it;
            // otherwise, pick a random character group from the unprocessed
            // group list. To allow a special character to appear in the
            // first position, increment the second parameter of the Next
            // function call by one, i.e. lastLeftGroupsOrderIdx + 1.
            var nextLeftGroupsOrderIdx = // Index which will be used to track not processed character groups.
                lastLeftGroupsOrderIdx == 0 ? 0 : random.Next(0, lastLeftGroupsOrderIdx);
            // Get the actual index of the character group, from which we will
            // pick the next character.
            var nextGroupIdx =
                leftGroupsOrder[nextLeftGroupsOrderIdx]; // Index of the next character group to be processed.

            // Get the index of the last unprocessed characters in this group.
            var lastCharIdx =
                charsLeftInGroup[nextGroupIdx] - 1; // Index of the last non-processed character in a group.

            // If only one unprocessed character is left, pick it; otherwise,
            // get a random character from the unused character list.
            int nextCharIdx; // Index of the next character to be added to password.
            nextCharIdx = lastCharIdx == 0 ? 0 : random.Next(0, lastCharIdx + 1);
            // Add this character to the password.
            password[i] = charGroups[nextGroupIdx][nextCharIdx];
            // If we processed the last character in this group, start over.
            if (lastCharIdx == 0) {
                charsLeftInGroup[nextGroupIdx] = charGroups[nextGroupIdx].Length;
            }
            else
            {
                if (lastCharIdx != nextCharIdx) {
                    (charGroups[nextGroupIdx][lastCharIdx], charGroups[nextGroupIdx][nextCharIdx]) = (charGroups[nextGroupIdx][nextCharIdx], charGroups[nextGroupIdx][lastCharIdx]);
                }
                charsLeftInGroup[nextGroupIdx]--;
            }

            // If we processed the last group, start all over.
            if (lastLeftGroupsOrderIdx == 0)
            {
                lastLeftGroupsOrderIdx = leftGroupsOrder.Length - 1;
            }
            else
            {
                // There are more unprocessed groups left.
                // Swap processed group with the last unprocessed group
                // so that we don't pick it until we process all groups.
                if (lastLeftGroupsOrderIdx != nextLeftGroupsOrderIdx)
                {
                    var temp = leftGroupsOrder[lastLeftGroupsOrderIdx];
                    leftGroupsOrder[lastLeftGroupsOrderIdx] = leftGroupsOrder[nextLeftGroupsOrderIdx];
                    leftGroupsOrder[nextLeftGroupsOrderIdx] = temp;
                }

                // Decrement the number of unprocessed groups.
                lastLeftGroupsOrderIdx--;
            }
        }

        // Convert password characters into a string and return the result.
        return new string(password);
    }
}