using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;

namespace FileToXML
{
    public class Common
    {

        public static bool WriteEncodedFile(string source, string destination, bool compressFirst, string description, out Exception exceptionArg)
        {
            exceptionArg = null;

            byte[] contents = File.ReadAllBytes(source);
            int contentsOriginalSize = contents.Length;
            //get all the hashes out of the way from the original 
            string contentsMD5 = Convert.ToBase64String(new MD5Cng().ComputeHash(contents));
            string contentsSHA1 = Convert.ToBase64String(new SHA1Managed().ComputeHash(contents));
            string contentsSHA256 = Convert.ToBase64String(new SHA256Managed().ComputeHash(contents));
            string contentsRipeMD = Convert.ToBase64String(new RIPEMD160Managed().ComputeHash(contents));

            if (compressFirst)
                contents = Compress(contents);
            string encodedContents = Convert.ToBase64String(contents, Base64FormattingOptions.InsertLineBreaks);

            XElement encodedFile = new XElement("EncodedFile");
            encodedFile.Add(new XComment("always virus-scan any file you get from the Internet, regardless of where you got it - TRUST NO ONE"));
            encodedFile.Add(new XElement("OriginalFilename", Path.GetFileName(source)));
            encodedFile.Add(new XElement("OriginalSize", contentsOriginalSize));
            encodedFile.Add(new XElement("IsCompressed", compressFirst));
            if (compressFirst)
                encodedFile.Add(new XElement("CompressionMethod", "GZip"));
            if (!string.IsNullOrWhiteSpace(description))
            {
                //encode description first before storing it
                encodedFile.Add(new XElement("Description", description));
            }
            encodedFile.Add(SetHashXElement("MD5", contentsMD5));
            encodedFile.Add(SetHashXElement("SHA1", contentsSHA1));
            encodedFile.Add(SetHashXElement("SHA256", contentsSHA256));
            encodedFile.Add(SetHashXElement("RIPEMD", contentsRipeMD));
            encodedFile.Add(new XElement("EncodedFileContents", encodedContents));
            XDocument encodedXMLFile = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"), encodedFile);
            encodedFile.Save(destination);
            return true;
        }

        public static bool WriteDecodedFile(string source, string destination, out Exception exceptionArg)
        {
            exceptionArg = null;
            //we're expecting the caller to have already found the destination file in the XML, or to have overridden it
            //  before calling this method

            XDocument xdoc = XDocument.Load(source);
            XElement root = xdoc.Root;

            byte[] contents = null;
            XElement contentsElement = root.Element("EncodedFileContents");
            if (contentsElement != null && !string.IsNullOrWhiteSpace(contentsElement.Value))
            {
                contents = Convert.FromBase64String(contentsElement.Value);
                XElement compElement = root.Element("IsCompressed");
                if (compElement != null && !string.IsNullOrWhiteSpace(compElement.Value))
                {
                    bool decompressMe = false;
                    if(bool.TryParse(compElement.Value, out decompressMe) && decompressMe)
                        contents = Decompress(contents);
                }
            }
            else
            {
                exceptionArg = new XmlException("XML missing EncodedFileContents node (or node is empty). source=" + source);
                return false;
            }

            XElement originalLength = root.Element("OriginalLength");
            if (!string.IsNullOrWhiteSpace(originalLength?.Value))
            {
                int length = 0;
                if (int.TryParse(originalLength.Value, out length) && length != contents.Length)
                {
                    exceptionArg = new Exception("Length of contents does not match specified original length. source=" + source);
                    return false;
                }
            }
            string contentsHash = null;
            if (!string.IsNullOrWhiteSpace(root.Element("OriginalHashMD5")?.Value))
            {
                contentsHash = Convert.ToBase64String(new MD5Cng().ComputeHash(contents));
                if (root.Element("OriginalHashMD5").Value.CompareTo(contentsHash) != 0)
                {
                    exceptionArg = new Exception("MD5 Hash failed. source=" + source);
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(root.Element("OriginalHashSHA1")?.Value))
            {
                contentsHash = Convert.ToBase64String(new SHA1Managed().ComputeHash(contents));
                if (root.Element("OriginalHashSHA1").Value.CompareTo(contentsHash) != 0)
                {
                    exceptionArg = new Exception("SHA1 Hash failed. source=" + source);
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(root.Element("OriginalHashSHA256")?.Value))
            {
                contentsHash = Convert.ToBase64String(new SHA256Managed().ComputeHash(contents));
                if (root.Element("OriginalHashSHA256").Value.CompareTo(contentsHash) != 0)
                {
                    exceptionArg = new Exception("SHA256 Hash failed. source=" + source);
                    return false;
                }
            }
            if (!string.IsNullOrWhiteSpace(root.Element("OriginalHashRIPEMD")?.Value))
            {
                contentsHash = Convert.ToBase64String(new RIPEMD160Managed().ComputeHash(contents));
                if (root.Element("OriginalHashRIPEMD").Value.CompareTo(contentsHash) != 0)
                {
                    exceptionArg = new Exception("RipeMD Hash failed. source=" + source);
                    return false;
                }
            }
            try
            {
                File.WriteAllBytes(destination, contents);
            }
            catch (Exception ex)
            {
                exceptionArg = new Exception("File write error [" +  destination + "]: " + ex.Message, ex);
                return false;
            }
            return true;
        }

        public static string ExtractOriginalFileName(string source, out string description)
        {
            description = null;
            XDocument xdoc = null;
            try
            {
                xdoc = XDocument.Load(source);
            }
            catch
            {
                return null;
            }
            if (xdoc == null)
                return null;
            if (xdoc.Root.Element("Description") != null)
                description = xdoc.Root.Element("Description").Value;
            if (!string.IsNullOrWhiteSpace(xdoc.Root.Element("OriginalFilename")?.Value))
            {
                return xdoc.Root.Element("OriginalFilename").Value;
            }
            else
                return null;
        }
        
        public static bool LogException(Exception exceptionArg, out Exception newExceptionArg)
        {
            newExceptionArg = null;
            try
            {
                string theAssembly = Assembly.GetAssembly(typeof(Common)).CodeBase;
                string thePath = Path.GetFullPath(
                    theAssembly.Replace("file:///", "")).Replace(
                    Path.GetFileName(theAssembly), "");
                DateTime thisInstant = DateTime.Now;
                string theFile = thePath +
                    "Exception" +
                    thisInstant.ToString("yyyy-MM-dd_HH-mm-ss") + ".txt";
                string exMessage = "EXCEPTION: " + exceptionArg.Message + "\r\n" + exceptionArg.StackTrace + "\r\n\r\n";
                if (exceptionArg.InnerException != null)
                    exMessage += "INNER EXCEPTION: " + exceptionArg.InnerException.Message + "\r\n" + exceptionArg.InnerException.StackTrace;
                File.WriteAllText(theFile, exMessage);
                return true;
            }
            catch (Exception ex)
            {
                newExceptionArg = ex;
                return false;
            }
        }


        private static byte[] Compress(byte[] source)
        {
            using (MemoryStream msIn = new MemoryStream(source))
            {
                using (MemoryStream msOut = new MemoryStream())
                {
                    using (GZipStream compress = new GZipStream(msOut, CompressionMode.Compress))
                    {
                        msIn.CopyTo(compress);
                    }
                    return (byte[])msOut.ToArray().Clone();
                }
            }
        }

        private static byte[] Decompress(byte[] source)
        {
            using (MemoryStream msIn = new MemoryStream(source))
            {
                using (MemoryStream msOut = new MemoryStream())
                {
                    using (GZipStream compress = new GZipStream(msIn, CompressionMode.Decompress))
                    {
                        compress.CopyTo(msOut);
                    }
                    return (byte[])msOut.ToArray().Clone();
                }
            }
        }

        private static XElement SetHashXElement(string typeAttribute, string value)
        {
            //XElement results = new XElement("OriginalHash", value);
            //results.SetAttributeValue("type", typeAttribute + "Base64Encoded");
            XElement results = new XElement("OriginalHash" + typeAttribute, value);
            return results;
        }

        public static string EscapeStringToXml (string unescaped)
        {
            return new System.Xml.Linq.XText(unescaped).ToString();
        }
        public static string UnescapeXmlString( string escaped)
        {
            return System.Xml.XmlReader.Create(new StringReader("<r>" + escaped + "</r>")).ReadElementString();
        }

    }
}
