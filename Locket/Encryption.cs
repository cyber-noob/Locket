using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace Locket
{
    sealed class Encryption
    {
        #region Property
        const string master_key = "codehunt";

        #endregion

        #region Constuctor

        #endregion

        #region Method

        #region New

        public void Encrypt1(string file, string path)
        {
            byte[] plainContent = File.ReadAllBytes(file);
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(master_key);
                DES.Key = Encoding.UTF8.GetBytes(master_key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (var ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, DES.CreateEncryptor(),
                    CryptoStreamMode.Write);
                    cs.Write(plainContent, 0, plainContent.Length);
                    cs.FlushFinalBlock();

                    FileInfo info = new FileInfo(file);
                    System.IO.File.WriteAllBytes(string.Format("{0}\\{1}{2}", path, SystemData.COUNT.ToString(), SystemData.EXTENSION), ms.ToArray());
                    SystemData.FILES.Add((SystemData.COUNT++).ToString(), info.Name);
                    cs.Close();

                    GC.Collect();

                    // Wait for all finalizers to complete before continuing.
                    GC.WaitForPendingFinalizers();

                }
            }
        }

        public void Decrypt1(string file, string path, string destination)
        {
            byte[] encrypted = File.ReadAllBytes(path + "\\" + file);
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(master_key);
                DES.Key = Encoding.UTF8.GetBytes(master_key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (var ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, DES.CreateDecryptor(),
                    CryptoStreamMode.Write);
                    cs.Write(encrypted, 0, encrypted.Length);
                    cs.FlushFinalBlock();
                    //File.WriteAllBytes(file, ms.ToArray());
                    System.IO.File.WriteAllBytes(string.Format("{0}\\{1}", destination, SystemData.FILES[file]), ms.ToArray());
                    cs.Close();

                    GC.Collect();

                    // Wait for all finalizers to complete before continuing.
                    GC.WaitForPendingFinalizers();
                }

            }
        }

        public byte[] DecryptTemp1(string file)
        {
            byte[] encrypted = File.ReadAllBytes(file);
            using (var DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(master_key);
                DES.Key = Encoding.UTF8.GetBytes(master_key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (var ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, DES.CreateDecryptor(),
                    CryptoStreamMode.Write);
                    cs.Write(encrypted, 0, encrypted.Length);
                    cs.FlushFinalBlock();

                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    return ms.ToArray();
                }

            }
        }

        #endregion

        #region Best

        /// Encrypts a file using Rijndael algorithm.
        public void Encrypt(string file, string path)
        {

            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(master_key);

                FileStream fsCrypt = new FileStream(string.Format("{0}\\{1}{2}", path, SystemData.COUNT.ToString(), SystemData.EXTENSION), FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(file, FileMode.Open);

                FileInfo info = new FileInfo(file);
                int data;
                while ((data = fsIn.ReadByte()) != -1)
                {
                    cs.WriteByte((byte)data);
                }

                SystemData.FILES.Add((SystemData.COUNT++).ToString(), info.Name);

                fsIn.Close();
                cs.Close();
                fsCrypt.Close();
            }
            catch
            {

            }
        }

        /// Decrypts a file using Rijndael algorithm.
        public void Decrypt(string file, string path, string destination)
        {
            try
            {
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(master_key);

                FileStream fsCrypt = new FileStream(path + "\\" + file, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(destination + "\\" + SystemData.FILES[file], FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);


                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
            catch
            {

            }
        }

        /// Decrypts a file using Rijndael algorithm.
        //public byte[] DecryptTemp(string file)
        //{
        //    try
        //    {
        //        UnicodeEncoding UE = new UnicodeEncoding();
        //        byte[] key = UE.GetBytes(master_key);

        //        byte[] encrypted = File.ReadAllBytes(file);
        //        FileStream fsCrypt = new FileStream(file, FileMode.Open);

        //        RijndaelManaged RMCrypto = new RijndaelManaged();



        //        using (var ms = new MemoryStream())
        //        {

        //            CryptoStream cs = new CryptoStream(fsCrypt,
        //                RMCrypto.CreateDecryptor(key, key),
        //                CryptoStreamMode.Read);

        //            cs.Write(encrypted, 0, encrypted.Length);
        //            cs.FlushFinalBlock();
        //            cs.Close();
        //            fsCrypt.Close();

        //            GC.Collect();
        //            GC.WaitForPendingFinalizers();

        //            return ms.ToArray();
        //        }
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        #endregion

        #region Encrypt String

        public string EncryptString(string textToEncrypt)
        {
            try
            {
                string ToReturn = "";
                //string _key = "ay$a5%&jwrtmnh;lasjdf98787";
                //string _iv = "abc@98797hjkas$&asd(*$%";
                string _key = "abcdefg123hijklmnopqrstuvw23";
                string _iv = "abc232hil343k1jjamakkfajfkja";
                byte[] _ivByte = { };
                _ivByte = System.Text.Encoding.UTF8.GetBytes(_iv.Substring(0, 8));
                byte[] _keybyte = { };
                _keybyte = System.Text.Encoding.UTF8.GetBytes(_key.Substring(0, 8));
                MemoryStream ms = null; CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(_keybyte, _ivByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }

        public string DecryptString(string textToDecrypt)
        {
            try
            {
                string ToReturn = "";
                string _key = "abcdefg123hijklmnopqrstuvw23";
                string _iv = "abc232hil343k1jjamakkfajfkja";
                byte[] _ivByte = { };
                _ivByte = System.Text.Encoding.UTF8.GetBytes(_iv.Substring(0, 8));
                byte[] _keybyte = { };
                _keybyte = System.Text.Encoding.UTF8.GetBytes(_key.Substring(0, 8));
                MemoryStream ms = null; CryptoStream cs = null;
                byte[] inputbyteArray = new byte[textToDecrypt.Replace(" ", "+").Length];
                inputbyteArray = Convert.FromBase64String(textToDecrypt.Replace(" ", "+"));
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateDecryptor(_keybyte, _ivByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    Encoding encoding = Encoding.UTF8;
                    ToReturn = encoding.GetString(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ae)
            {
                throw new Exception(ae.Message, ae.InnerException);
            }
        }
        #endregion


        #endregion

    }
}
