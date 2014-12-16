/*
   CopyRight (C) 1998-2005 CyberPlat.Com. All Rights Reserved.
   e-mail: support@cyberplat.com
*/

using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace com.dhs.webapi.Model.Common.CyberPlate
{
    public class IPrivException : Exception
    {
        private readonly int m_code;
        public IPrivException(int c)
        {
            m_code = c;
        }
        public override string ToString()
        {
            switch (m_code)
            {
                case -1: return "Error in arguments";
                case -2: return "Memory allocation error";
                case -3: return "Invalid document format";
                case -4: return "The reading of the document has not been completed";
                case -5: return "Error in the document’s internal structure";
                case -6: return "Unknown encryption algorithm";
                case -7: return "The key length and the signature length do not match";
                case -8: return "Invalid code phrase to the secret key";
                case -9: return "Invalid document type";
                case -10: return "Error ASCII in encoding of the document";
                case -11: return "Error ASCII in decoding of the document";
                case -12: return "Unknown type of the encryption item";
                case -13: return "The encryption item is not ready";
                case -14: return "The call is not supported by the encryption item";
                case -15: return "Failed to find the file";
                case -16: return "File reading error";
                case -17: return "The key cannot be used";
                case -18: return "Error in signature creation";
                case -19: return "A public key with this serial number is not found";
                case -20: return "The signature and the document contents do not match";
                case -21: return "File creation error";
                case -22: return "Filing error";
                case -23: return "Invalid key card format";
                case -24: return "Keys generation error";
                case -25: return "Encryption error";
                case -26: return "Decryption error";
                case -27: return "The sender is not defined";
            }
            return "General error";
        }
        public int code
        {
            get { return m_code; }
        }
    };

    public class IPrivKey : IDisposable
    {
        private byte[] pkey;
        private unsafe byte* ppkey;
        private GCHandle handle;
        public unsafe IPrivKey()
        {
            pkey = new byte[64];
            handle = GCHandle.Alloc(pkey, GCHandleType.Pinned);
            ppkey = (byte*)handle.AddrOfPinnedObject();
        }
        ~IPrivKey()
        {
            closeKey();
        }
        public string signText(string src)
        {
            return IPriv.signText(src, this);
        }
        public string verifyText(string src)
        {
            return IPriv.verifyText(src, this);
        }
        public unsafe void closeKey()
        {
            if (ppkey != (byte*)IntPtr.Zero)
            {
                IPriv.closeKey(this);
                handle.Free();
                ppkey = (byte*)IntPtr.Zero;
            }
        }

        public byte[] getKey()
        {
            return pkey;
        }

        public unsafe byte* getPKey()
        {
            return ppkey;
        }

        public void Dispose()
        {
            closeKey();
        }
    };

    [SuppressUnmanagedCodeSecurity]
    public static class IPriv
    {
        internal static readonly Encoding cp1251 = Encoding.GetEncoding(1251);
        private const string libname = "libipriv";
        // for internal usage only

        [DllImport(libname)]
        private static extern int Crypt_Initialize();

        [DllImport(libname)]
        private static extern int Crypt_Done();

        [DllImport(libname)]
        private static extern unsafe int Crypt_OpenSecretKeyFromFile(int eng,
            byte* path,
            byte* passwd,
            byte* pkey);

        [DllImport(libname)]
        internal static extern unsafe int Crypt_OpenPublicKeyFromFile(int eng,
            byte* path,
            uint keyserial,
            byte* pkey,
            byte* ñakey);

        [DllImport(libname)]
        private static unsafe extern int Crypt_Sign(byte* src,
            int nsrc, sbyte* dst,
            int ndst,
            byte* pkey);

        [DllImport(libname)]
        private static extern unsafe int Crypt_Verify(byte* src,
            int nsrc, sbyte** pdst,
            ref int pndst, byte* pkey);

        [DllImport(libname)]
        private static extern unsafe int Crypt_CloseKey(byte* pkey);

        public static void Initialize()
        {
            Crypt_Initialize();
        }
        public static void Done()
        {
            Crypt_Done();
        }

        public static unsafe IPrivKey openSecretKey(string path, string passwd)
        {
            IPrivKey k = new IPrivKey();
            byte[] bpath = new byte[path.Length + 1];//zero-terminated string
            byte[] bpasswd = new byte[passwd.Length + 1];//zero-terminated string
            cp1251.GetBytes(path, 0, path.Length, bpath, 0);
            cp1251.GetBytes(passwd, 0, passwd.Length, bpasswd, 0);
            fixed (byte* ppath = bpath)
            fixed (byte* ppasswd = bpasswd)
            {
                int rc = Crypt_OpenSecretKeyFromFile(0, ppath, ppasswd, k.getPKey());
                if (rc != 0)
                    throw (new IPrivException(rc));
            }
            return k;
        }

        public static unsafe IPrivKey openPublicKey(string path, uint keyserial)
        {
            IPrivKey k = new IPrivKey();
            byte[] bpath = new byte[path.Length + 1];//zero-terminated string
            cp1251.GetBytes(path, 0, path.Length, bpath, 0);
            fixed (byte* ppath = bpath)
            {
                int rc = Crypt_OpenPublicKeyFromFile(0, ppath, keyserial, k.getPKey(), null);
                if (rc != 0)
                    throw (new IPrivException(rc));
            }
            return k;
        }

        public static unsafe string signText(string src, IPrivKey key)
        {
            const int max_length = 2048;
            sbyte* tmp = stackalloc sbyte[max_length];
            byte[] bsrc = cp1251.GetBytes(src);
            int rc;
            fixed (byte* psrc = bsrc)
                rc = Crypt_Sign(psrc, bsrc.Length, tmp, max_length, key.getPKey());
            if (rc < 0)
                throw (new IPrivException(rc));
            return new string(tmp, 0, rc, cp1251);
        }
        public static unsafe string verifyText(string src, IPrivKey key)
        {
            byte[] srcb = cp1251.GetBytes(src);
            fixed (byte* psrc = srcb)
            {
                sbyte* pdst = (sbyte*)IntPtr.Zero;
                int pndst = 0;
                int rc = Crypt_Verify(psrc, srcb.Length, &pdst, ref pndst, key.getPKey());
                if (rc != 0)
                    throw (new IPrivException(rc));
                return new string(pdst, 0, pndst, cp1251);
            }
        }
        public static unsafe void closeKey(IPrivKey key)
        {
            Crypt_CloseKey(key.getPKey());
        }
    };

}
