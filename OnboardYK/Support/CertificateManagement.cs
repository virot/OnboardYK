using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using Yubico.YubiKey.Piv;
using Yubico.YubiKey.Sample.PivSampleCode;
using Yubico.YubiKey;
using CERTCLILib;

namespace OnboardYK.Support
{
    internal static class CertificateManagement
    {
        internal static void RequestCertificate(byte slot, string template, string certificateAuthority, YubiKeyDevice yubiKeyDevice, YKKeyCollector yKKeyCollector)
        {
        }

        internal static void InstallCertificate(byte slot, X509Certificate2 certificate, YubiKeyDevice yubiKeyDevice, YKKeyCollector yKKeyCollector)
        {
            using (var pivSession = new PivSession((YubiKeyDevice)yubiKeyDevice!))
            {
                pivSession.KeyCollector = yKKeyCollector.YKKeyCollectorDelegate;
                pivSession.ImportCertificate(slot, certificate);
            }
        }
        internal static string GenerateCertificateSigningRequest(byte slot, YubiKeyDevice yubiKeyDevice, YKKeyCollector yKKeyCollector)
        {
            CertificateRequest request;
            X509SignatureGenerator signer;
            X500DistinguishedName Subjectname = new X500DistinguishedName("CN=OnboardYK");
            HashAlgorithmName hashAlgorithmName = HashAlgorithmName.SHA256;
            string pemData;

            using (var pivSession = new PivSession((YubiKeyDevice)yubiKeyDevice!))
            {
                pivSession.KeyCollector = yKKeyCollector.YKKeyCollectorDelegate;
                PivPublicKey? publicKey;
                try
                {
                    publicKey = pivSession.GetMetadata(slot).PublicKey;
                }
                catch
                {
                    throw new Exception("Failed to get public key");
                }

                using AsymmetricAlgorithm dotNetPublicKey = KeyConverter.GetDotNetFromPivPublicKey(publicKey);

                if (publicKey is PivRsaPublicKey)
                {
                    request = new CertificateRequest(Subjectname, (RSA)dotNetPublicKey, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
                else
                {
                    hashAlgorithmName = publicKey.Algorithm switch
                    {
                        PivAlgorithm.EccP256 => HashAlgorithmName.SHA256,
                        PivAlgorithm.EccP384 => HashAlgorithmName.SHA384,
                        _ => throw new Exception("Unknown PublicKey algorithm")
                    };
                    request = new CertificateRequest(Subjectname, (ECDsa)dotNetPublicKey, hashAlgorithmName);
                }
                X509Certificate2 slotAttestationCertificate = pivSession.CreateAttestationStatement(slot);
                byte[] slotAttestationCertificateBytes = slotAttestationCertificate.Export(X509ContentType.Cert);
                X509Certificate2 yubikeyIntermediateAttestationCertificate = pivSession.GetAttestationCertificate();
                byte[] yubikeyIntermediateAttestationCertificateBytes = yubikeyIntermediateAttestationCertificate.Export(X509ContentType.Cert);
                Oid oidIntermediate = new Oid("1.3.6.1.4.1.41482.3.2");
                Oid oidSlotAttestation = new Oid("1.3.6.1.4.1.41482.3.11");
                request.CertificateExtensions.Add(new X509Extension(oidSlotAttestation, slotAttestationCertificateBytes, false));
                request.CertificateExtensions.Add(new X509Extension(oidIntermediate, yubikeyIntermediateAttestationCertificateBytes, false));

                if (publicKey is PivRsaPublicKey)
                {
                    signer = new YubiKeySignatureGenerator(pivSession, slot, publicKey, RSASignaturePaddingMode.Pss);
                }
                else
                {
                    signer = new YubiKeySignatureGenerator(pivSession, slot, publicKey);
                }

                byte[] requestSigned = request.CreateSigningRequest(signer);
                pemData = PemEncoding.WriteString("CERTIFICATE REQUEST", requestSigned);
            }
            return pemData;
        }

        public static string SubmitRequest(string caServer, string csr, string template)
        {
            // Create a new request object
            //MessageBox.Show(csr, "CSR");
            CCertRequest certRequest = new CCertRequest();
            int disposition = certRequest.Submit(
                CR_IN_BASE64HEADER | CR_IN_FORMATANY,
                csr,
                $"CertificateTemplate:{template}",
                caServer);

            if (disposition != CR_DISP_ISSUED)
            {
                throw new Exception($"Certificate request failed with disposition: {disposition}");
            }

            // Retrieve the issued certificate
            return certRequest.GetCertificate(CR_OUT_BASE64HEADER);
        }

        private const int CR_IN_BASE64HEADER = 0x0;
        private const int CR_IN_BASE64 = 0x1;
        private const int CR_IN_FORMATANY = 0xff;
        private const int CR_DISP_ISSUED = 0x3;
        private const int CR_OUT_BASE64HEADER = 0x0;
        private const int CR_OUT_BASE64 = 0x1;
        private const int CR_OUT_CHAIN = 0x100;
    }
}
