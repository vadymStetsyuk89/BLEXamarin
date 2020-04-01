using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace StBox.Environment.Helpers.JWTUtils
{
    /// <summary>
    /// How to use:
    /// 
    /// try {
    ///     string token = GlobalSettings.Instance.UserProfile.AccesToken;
    ///     byte[] decodedPayload = JWTConvert.Base64UrlDecode(token.Split('.')[1]);
    ///     string payloadJson = Encoding.UTF8.GetString(decodedPayload, 0, decodedPayload.Length);
    /// }
    /// catch (Exception exc) {
    ///     string message = exc.Message;
    /// }
    /// </summary>
    public static class JWTConvert
    {
        private static Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms;

        static JWTConvert()
        {
            HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>>
            {
                {
                    JwtHashAlgorithm.HS256, (key, value) =>
                    {
                        using (HMACSHA256 sha = new HMACSHA256(key))
                        {
                            return sha.ComputeHash(value);
                        }
                    }
                }
                //{ JwtHashAlgorithm.HS384, (key, value) => { using (var sha = new HMACSHA384(key)) { return sha.ComputeHash(value); } } },
                //{ JwtHashAlgorithm.HS512, (key, value) => { using (var sha = new HMACSHA512(key)) { return sha.ComputeHash(value); } } }
            };
        }

        /// <summary>
        /// Creates a JWT given a payload, the signing key, and the algorithm to use.
        /// </summary>
        /// <param name="payload">An arbitrary payload (must be serializable to JSON via <see cref="System.Web.Script.Serialization.JavaScriptSerializer"/>).</param>
        /// <param name="key">The key bytes used to sign the token.</param>
        /// <param name="algorithm">The hash algorithm to use.</param>
        /// <returns>The generated JWT.</returns>
        public static string Encode(object payload, byte[] key, JwtHashAlgorithm algorithm)
        {
            List<string> segments = new List<string>();
            var header = new { typ = "JWT", alg = algorithm.ToString() };

            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header));
            byte[] payloadBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));

            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(payloadBytes));

            string stringToSign = string.Join(".", segments.ToArray());

            byte[] bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] signature = HashAlgorithms[algorithm](key, bytesToSign);
            segments.Add(Base64UrlEncode(signature));

            return string.Join(".", segments.ToArray());
        }

        /// <summary>
        /// Creates a JWT given a payload, the signing key, and the algorithm to use.
        /// </summary>
        /// <param name="payload">An arbitrary payload (must be serializable to JSON via <see cref="System.Web.Script.Serialization.JavaScriptSerializer"/>).</param>
        /// <param name="key">The key used to sign the token.</param>
        /// <param name="algorithm">The hash algorithm to use.</param>
        /// <returns>The generated JWT.</returns>
        public static string Encode(object payload, string key, JwtHashAlgorithm algorithm)
        {
            return Encode(payload, Encoding.UTF8.GetBytes(key), algorithm);
        }

        /// <summary>
        /// Given a JWT, decode it and return the JSON payload.
        /// </summary>
        /// <param name="token">The JWT.</param>
        /// <param name="key">The key bytes that were used to sign the JWT.</param>
        /// <param name="verify">Whether to verify the signature (default is true).</param>
        /// <returns>A string containing the JSON payload.</returns>
        /// <exception cref="SignatureVerificationException">Thrown if the verify parameter was true and the signature was NOT valid or if the JWT was signed with an unsupported algorithm.</exception>
        public static string Decode(string token, byte[] key, bool verify = true)
        {
            string[] parts = token.Split('.');
            string header = parts[0];
            string payload = parts[1];
            byte[] crypto = Base64UrlDecode(parts[2]);
            byte[] decodedHeader = Base64UrlDecode(header);
            byte[] decodedPayload = Base64UrlDecode(payload);

            string headerJson = Encoding.UTF8.GetString(decodedHeader, 0, decodedHeader.Length);
            Dictionary<string, object> headerData = JsonConvert.DeserializeObject<Dictionary<string, object>>(headerJson);
            string payloadJson = Encoding.UTF8.GetString(decodedPayload, 0, decodedPayload.Length);

            if (!verify) return payloadJson;

            byte[] bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", payload));
            string algorithm = (string)headerData["alg"];

            var signature = HashAlgorithms[GetHashAlgorithm(algorithm)](key, bytesToSign);
            string decodedCrypto = Convert.ToBase64String(crypto);
            string decodedSignature = Convert.ToBase64String(signature);

            if (decodedCrypto != decodedSignature)
            {
                throw new SignatureVerificationException(string.Format("Invalid signature. Expected {0} got {1}", decodedCrypto, decodedSignature));
            }

            return payloadJson;
        }

        /// <summary>
        /// Given a JWT, decode it and return the JSON payload.
        /// </summary>
        /// <param name="token">The JWT.</param>
        /// <param name="key">The key that was used to sign the JWT.</param>
        /// <param name="verify">Whether to verify the signature (default is true).</param>
        /// <returns>A string containing the JSON payload.</returns>
        /// <exception cref="SignatureVerificationException">Thrown if the verify parameter was true and the signature was NOT valid or if the JWT was signed with an unsupported algorithm.</exception>
        public static string Decode(string token, string key, bool verify = true)
        {
            return Decode(token, Encoding.UTF8.GetBytes(key), verify);
        }

        /// <summary>
        /// Given a JWT, decode it and return the payload as an object (by deserializing it with <see cref="System.Web.Script.Serialization.JavaScriptSerializer"/>).
        /// </summary>
        /// <param name="token">The JWT.</param>
        /// <param name="key">The key that was used to sign the JWT.</param>
        /// <param name="verify">Whether to verify the signature (default is true).</param>
        /// <returns>An object representing the payload.</returns>
        /// <exception cref="SignatureVerificationException">Thrown if the verify parameter was true and the signature was NOT valid or if the JWT was signed with an unsupported algorithm.</exception>
        public static object DecodeToObject(string token, string key, bool verify = true)
        {
            string payloadJson = JWTConvert.Decode(token, key, verify);
            Dictionary<string, object> payloadData = JsonConvert.DeserializeObject<Dictionary<string, object>>(payloadJson);
            return payloadData;
        }

        public static string Base64UrlEncode(byte[] input)
        {
            string output = Convert.ToBase64String(input);
            output = output.Split('=')[0]; // Remove any trailing '='s
            output = output.Replace('+', '-'); // 62nd char of encoding
            output = output.Replace('/', '_'); // 63rd char of encoding
            return output;
        }

        public static byte[] Base64UrlDecode(string input)
        {
            string output = input;
            output = output.Replace('-', '+'); // 62nd char of encoding
            output = output.Replace('_', '/'); // 63rd char of encoding
            switch (output.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: output += "=="; break; // Two pad chars
                case 3: output += "="; break; // One pad char
                default: throw new Exception("Illegal base64url string!");
            }
            byte[] converted = Convert.FromBase64String(output); // Standard base64 decoder
            return converted;
        }

        private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        {
            switch (algorithm)
            {
                case "HS256": return JwtHashAlgorithm.HS256;
                case "HS384": return JwtHashAlgorithm.HS384;
                case "HS512": return JwtHashAlgorithm.HS512;
                default: throw new SignatureVerificationException("Algorithm not supported.");
            }
        }
    }
}
