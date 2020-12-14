using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using TrackLib.Constants;

namespace TrackLib.Utils {
    public static class TextUtil {
        public static bool IsBase64String(string base64) {
            if (string.IsNullOrEmpty(base64))
                return false;

            Span<byte> buffer = new Span<byte>(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }

        public static string HexToBinaryString(string hexString) {

            if (string.IsNullOrEmpty(hexString) || !Regex.IsMatch(hexString, Patterns.HEX_NUMBER))
                return "";

            var hexCharacterToBinary = new Dictionary<char, string> {
                { '0', "0000" }, { '1', "0001" }, { '2', "0010" }, { '3', "0011" },
                { '4', "0100" }, { '5', "0101" }, { '6', "0110" }, { '7', "0111" },
                { '8', "1000" }, { '9', "1001" }, { 'a', "1010" }, { 'b', "1011" },
                { 'c', "1100" }, { 'd', "1101" }, { 'e', "1110" }, { 'f', "1111" }
            };
            var result = new StringBuilder();
            foreach (char c in hexString) {
                result.Append(hexCharacterToBinary[char.ToLower(c)]);
            }
            return result.ToString();
        }

        public static string CreateMD5(string input) {
            // Use input string to calculate MD5 hash
            using (var md5 = MD5.Create()) {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++) {
                    sb.Append(hashBytes[i].ToString("x2"));
                }
                return sb.ToString();
            }
        }
    }
}
