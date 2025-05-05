using System.Text;

namespace Infrastructure.Helpers
{
   

    public static class PasswordGenerator
    {
        private static readonly string UpperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private static readonly string LowerCase = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string Numbers = "0123456789";
        private static readonly string SpecialCharacters = "!@#$%^&*()-_=+<>?";

        public static string GeneratePassword(int length = 12)
        {
            if (length < 8) throw new ArgumentException("Password length must be at least 8 characters.");

            string allChars = UpperCase + LowerCase + Numbers + SpecialCharacters;
            Random random = new Random();

            // Ensure password contains at least one of each category
            StringBuilder password = new StringBuilder();
            password.Append(UpperCase[random.Next(UpperCase.Length)]);
            password.Append(LowerCase[random.Next(LowerCase.Length)]);
            password.Append(Numbers[random.Next(Numbers.Length)]);
            password.Append(SpecialCharacters[random.Next(SpecialCharacters.Length)]);

            // Fill remaining length randomly
            for (int i = 4; i < length; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the password for randomness
            return new string(password.ToString().OrderBy(_ => random.Next()).ToArray());
        }
    }
}
