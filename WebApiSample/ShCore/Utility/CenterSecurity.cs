using System;
namespace ShCore.Utility
{
    /// <summary>
    /// Trung tâm bảo mật chăng :D
    /// </summary>
    public class CenterSecurity
    {
        /// <summary>
        /// Ngăn không cho khởi tạo CenterSecurity
        /// </summary>
        private CenterSecurity() { }

        private static CenterSecurity inst = new CenterSecurity();
        /// <summary>
        /// Khởi tạo một instance duy nhất để sử dụng
        /// </summary>
        public static CenterSecurity Inst
        {
            get { return inst; }
        }

        /// <summary>
        /// Private Key
        /// </summary>
        private string privateKey = "<RSAKeyValue><Modulus>uhhwLgXWDxlRuMAvpexfaa3tfu6LUpY+OAlA7OMpefbN6cyT/G2s09MpfU8UlJwcZKtXB2BhJ7EjNDYJ+Sw8Lu5zx4ZRNlwO07xupeMWaVezcv9zut79OEfjc0GAIU2rOxp/wjuM1JFJn0Qd63/pbqOpwZHkmd/I2w873/4M1FM=</Modulus><Exponent>AQAB</Exponent><P>3/jMMWK8QAc137Swb/nuMgemyfd+1MqJFFTxQKMugxrOmO0Wo+fOakAB10JAc3BHATC4B/DGgcouUpqkkBk58Q==</P><Q>1LUN6nPdGYCZkmm/6IZLh3ulzV0z3jQAGu3xarB2kZwknCouerd34yyBfzENIfVqByoCb3qzZJtAxO/IvokOgw==</Q><DP>QcUAyVvSJgc4Bco8qZU+Ikjm7JYWE4yqNmM/ORjyNqOvmW694EHY9pB3OewFmyCUaUASOpq04DYr5ivtOTd/MQ==</DP><DQ>neEVRyRgxAET+/zKGMk1XoaEdn3rdc6bFWHvgwUfvMxs0AzvGt76+X+bTtEVslL6M/8Wd7BXXyFtXb+s/N+2CQ==</DQ><InverseQ>GNziJpJnzfnQtOeG7DBnZg5x8qRCVvoTXt/waKhIcGX1FR3D90E6/wTUB8ra+YyNUvZvyKqzXh8YMPiqg9mv+w==</InverseQ><D>eAN0rSmUYA5rFqPS1sW2zredV2PNtAgyvf6xwVPKlt5k82e89GlisQUYV7jdQ+3dncqmCJrObUOeuXg0PF6bvGrqy28E/ib4Hfsy+SSUMMdqtAooISQGTrOqMTS4IbC1UXyQQ0eZqEr52J4Qj2EDbw8vCxy2irlRtfp6lMSuCOE=</D></RSAKeyValue>";

        /// <summary>
        /// Public Key
        /// </summary>
        private string publicKey = "<RSAKeyValue><Modulus>uhhwLgXWDxlRuMAvpexfaa3tfu6LUpY+OAlA7OMpefbN6cyT/G2s09MpfU8UlJwcZKtXB2BhJ7EjNDYJ+Sw8Lu5zx4ZRNlwO07xupeMWaVezcv9zut79OEfjc0GAIU2rOxp/wjuM1JFJn0Qd63/pbqOpwZHkmd/I2w873/4M1FM=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

        /// <summary>
        /// Độ dài của Key
        /// </summary>
        private int keySize = 1024;

        /// <summary>
        /// Các ký tự được phép của một key
        /// </summary>
        private string KEYLIB = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        Random ran = new Random();
        /// <summary>
        /// Tạo động chuỗi Key Random
        /// </summary>
        /// <returns></returns>
        private string RenderKeyRandom()
        {
            int len = KEYLIB.Length;
            string retKey = string.Empty;
            while (retKey.Length < 9) retKey += KEYLIB[ran.Next(0, len - 1)];
            return retKey;
        }

        /// <summary>
        /// Thực hiện mã hóa dữ liệu
        /// Trả về ShKeyValue gồm Key = Key đã được mã hóa, Value = data đã được mã hóa
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Pair<string, string> EnCrypt(string data)
        {
            // Tạo một chuỗi Key random
            var keyRandom = this.RenderKeyRandom();

            // 
            return EnCrypt(data, keyRandom);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public Pair<string, string> EnCrypt(string data, string key)
        {
            // Thực hiện mã hóa keyRandom
            var keyRandomEnCrypt = Cryptography.EnCryptByRSA(key, keySize, publicKey);

            // Thực hiện mã hóa dữ liệu
            var dataEnCrypt = Cryptography.EncryptDataByTripleDES(data, key);

            // return kết quả
            return new Pair<string, string> { T1 = keyRandomEnCrypt, T2 = dataEnCrypt };
        }

        /// <summary>
        /// Thực hiện giải mã với data chứa Key = key được mã hóa bởi public key, data = dữ liệu mã bởi key random
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Decrypt(Pair<string, string> data)
        {
            return this.Decrypt(data.T1, data.T2);
        }

        /// <summary>
        /// Thực hiện giải mã
        /// </summary>
        /// <param name="keyCrypt"></param>
        /// <param name="dataCrypt"></param>
        /// <returns></returns>
        public string Decrypt(string keyCrypt, string dataCrypt)
        {
            // giải mã key 
            var keyRandomDeCrypt = Cryptography.DeCryptByRSA(keyCrypt, keySize, privateKey);

            // Trả ra kết quả giải mã data
            return Cryptography.DecryptDataByTripleDES(dataCrypt, keyRandomDeCrypt);
        }
    }
}
