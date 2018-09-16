using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace PayPal.Business.BLL
{
    public static class Security
    {
        public static X509Certificate2 GetClientCertificate(HttpContext httpContext)
        {
            X509Certificate2 clientCertificate = null;

            try
            {
                clientCertificate = httpContext.Connection.ClientCertificate;
                Security.IsValidClientCertificate(clientCertificate);
            }
            catch (Exception e)
            {
                throw e;
            }
            return clientCertificate;
        }

        public static bool IsValidClientCertificate(X509Certificate2 certificate)
        {
            // In this example we will only accept the certificate as a valid certificate if all the conditions below are met:
            // 1. The certificate is not expired and is active for the current time on server.
            // 2. The subject name of the certificate has the common name nildevecc
            // 3. The issuer name of the certificate has the common name nildevecc and organization name Microsoft Corp
            // 4. The thumbprint of the certificate is 30757A2E831977D8BD9C8496E4C99AB26CB9622B
            //
            // This example does NOT test that this certificate is chained to a Trusted Root Authority (or revoked) on the server 
            // and it allows for self signed certificates
            //
            string error = "";
            if (certificate == null)
            {
                error = "certicate is null";
                return false;
            }

            // 1. Check time validity of certificate
            if (DateTime.Compare(DateTime.Now, certificate.NotBefore) < 0 )
            {
                error = "certicate is not yet valid";
                return false;
            }

            if(DateTime.Compare(DateTime.Now, certificate.NotAfter) > 0)
            {
                error = "certicate is outdated";
                return false;
            }
            /*
            // 2. Check subject name of certificate
            bool foundSubject = false;
            string[] certSubjectData = certificate.Subject.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in certSubjectData)
            {
                if (String.CompareOrdinal(s.Trim(), "CN=nildevecc") == 0)
                {
                    foundSubject = true;
                    break;
                }
            }
            if (!foundSubject)
            {
                error = "certicate subject not found";
                return false;
            }
            */
            /*
            // 3. Check issuer name of certificate
            bool foundIssuerCn = false, foundIssuerO = false;
            string[] certIssuerData = certificate.Issuer.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string s in certIssuerData)
            {
                if (String.CompareOrdinal(s.Trim(), "CN=nildevecc") == 0)
                {
                    foundIssuerCn = true;
                    if (foundIssuerO) break;
                }

                if (String.CompareOrdinal(s.Trim(), "O=Microsoft Corp") == 0)
                {
                    foundIssuerO = true;
                    if (foundIssuerCn) break;
                }
            }

            if (!foundIssuerCn || !foundIssuerO)
            {
                error = "issuer not found";
                return false;
            }

            // 4. Check thumprint of certificate
            if (String.CompareOrdinal(certificate.Thumbprint.Trim().ToUpper(), "30757A2E831977D8BD9C8496E4C99AB26CB9622B") != 0)
            {
                error = "bad thumb";
                return false;
            }

            // If you also want to test if the certificate chains to a Trusted Root Authority you can uncomment the code below
            //
            //X509Chain certChain = new X509Chain();
            //certChain.Build(certificate);
            //bool isValidCertChain = true;
            //foreach (X509ChainElement chElement in certChain.ChainElements)
            //{
            //    if (!chElement.Certificate.Verify())
            //    {
            //        isValidCertChain = false;
            //        break;
            //    }
            //}
            //if (!isValidCertChain) return false;
            */
            if(error!="")
                throw new Exception(error);
            return true;
        }

        private static X509Certificate2 GetCert(string thumbprint)
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            
            thumbprint = Regex.Replace(thumbprint, @"[^\da-fA-F]", string.Empty).ToUpper();
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2 clientCertInRequest = null;

            clientCertInRequest = store.Certificates.Find(X509FindType.FindByThumbprint, thumbprint, false)
                .OfType<X509Certificate2>().FirstOrDefault();

            return (clientCertInRequest);
        }

        public static X509Certificate2 GetLocalCertificate(string thumbprint)
        {
            X509Certificate2 certificate = Security.GetCert(thumbprint);

            if (certificate == null)
            {
                throw new Exception("local certificate not found, thumb=" + thumbprint);
            }

            return (certificate);
        }

        //public static X509Certificate2 GetLocalCertificate()
        //{
            //ProjectLib projectLib = new ProjectLib();
            //string thumbprint = projectLib.Thumb;

            //X509Certificate2 certificate = GetLocalCertificate(thumbprint);

            //return (certificate);
       // }

       
    }
}
