﻿using CoCSharp.Networking.Cryptography;
using Ionic.Zlib;
using System;
using System.IO;

namespace CoCSharp.Networking.Messages
{
    /// <summary>
    /// Defines login failure reasons.
    /// </summary>
    public enum LoginFailureReason : int
    {
        /// <summary>
        /// Content version is outdated.
        /// </summary>
        OutdatedContent = 7,

        /// <summary>
        /// Client verision is outdated.
        /// </summary>
        OutdatedVersion = 8,

        /// <summary>
        /// Unknown reasons 1.
        /// </summary>
        Unknown1 = 9,

        /// <summary>
        /// Server is in maintenance.
        /// </summary>
        Maintenance = 10,

        /// <summary>
        /// Temporarily banned.
        /// </summary>
        TemporarilyBanned = 11,

        /// <summary>
        /// Take a rest.
        /// </summary>
        TakeRest = 12,

        /// <summary>
        /// Account has been locked. Can only be unlocked with a specific code.
        /// </summary>
        Locked = 13
    };

    /// <summary>
    /// Message that is sent by the server to the client when a <see cref="LoginRequestMessage"/>
    /// failed.
    /// </summary>
    public class LoginFailedMessage : Message
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginFailedMessage"/>.
        /// </summary>
        public LoginFailedMessage()
        {
            // Space
        }

        /// <summary>
        /// Nonce generated by the server which will be used for encryption. Also
        /// known as 'rnonce'.
        /// </summary>
        public byte[] Nonce;
        /// <summary>
        /// Public key generated by the server which will be used for encryption. Also
        /// known as 'k'.
        /// </summary>
        public byte[] PublicKey;

        /// <summary>
        /// Reason why login failed.
        /// </summary>
        public LoginFailureReason Reason;

        /// <summary>
        /// Unknown string 1.
        /// </summary>
        public string Unknown1;

        /// <summary>
        /// Host name.
        /// </summary>
        public string Hostname;
        /// <summary>
        /// Download root url from where all the assets will be downloaded.
        /// </summary>
        public string DownloadRootUrl;

        /// <summary>
        /// Unknown string 2.
        /// </summary>
        public string Unknown2;
        /// <summary>
        /// Unknown int 3.
        /// </summary>
        public int Unknown3;
        /// <summary>
        /// Unknown byte 4.
        /// </summary>
        public byte Unknown4;
        /// <summary>
        /// Unknown string 5.
        /// </summary>
        public string Unknown5;

        /// <summary>
        /// Fingerprint json string.
        /// </summary>
        public string FingerprintJsonString; //TODO: Implement fingerprint.
        /// <summary>
        /// Unknown int 6.
        /// </summary>
        public int Unknown6;
        /// <summary>
        /// Unknown int 7.
        /// </summary>
        public int Unknown7;

        /// <summary>
        ///  Gets the ID of the <see cref="LoginFailedMessage"/>.
        /// </summary>
        public override ushort ID { get { return 20103; } }

        /// <summary>
        /// Reads the <see cref="LoginFailedMessage"/> from the specified <see cref="MessageReader"/>.
        /// </summary>
        /// <param name="reader">
        /// <see cref="MessageReader"/> that will be used to read the <see cref="LoginFailedMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="reader"/> is null.</exception>
        public override void ReadMessage(MessageReader reader)
        {
            ThrowIfReaderNull(reader);

            Nonce = reader.ReadBytes(CoCKeyPair.NonceLength);
            PublicKey = reader.ReadBytes(CoCKeyPair.KeyLength);

            Reason = (LoginFailureReason)reader.ReadInt32();

            Unknown1 = reader.ReadString(); // null, outdatedcontent

            Hostname = reader.ReadString(); // stage.clashofclans.com
            DownloadRootUrl = reader.ReadString(); // http://b46f744d64acd2191eda-3720c0374d47e9a0dd52be4d281c260f.r11.cf2.rackcdn.com/

            Unknown2 = reader.ReadString(); // market://details?id=com.supercell.clashofclans
            Unknown3 = reader.ReadInt32(); // -1
            Unknown4 = reader.ReadByte(); // 0
            Unknown5 = reader.ReadString(); // ""

            var fingerprintData = reader.ReadBytes();
            using (var br = new BinaryReader(new MemoryStream(fingerprintData)))
            {
                var decompressedLength = br.ReadInt32();
                var compressedFingerprint = br.ReadBytes(fingerprintData.Length - 4);
                var fingerprintJson = ZlibStream.UncompressString(compressedFingerprint);
                FingerprintJsonString = fingerprintJson;
            }

            Unknown6 = reader.ReadInt32(); // -1
            Unknown7 = reader.ReadInt32(); // 2
        }

        /// <summary>
        /// Writes the <see cref="LoginFailedMessage"/> to the specified <see cref="MessageWriter"/>.
        /// </summary>
        /// <param name="writer">
        /// <see cref="MessageWriter"/> that will be used to write the <see cref="LoginFailedMessage"/>.
        /// </param>
        /// <exception cref="ArgumentNullException"><paramref name="writer"/> is null.</exception>
        public override void WriteMessage(MessageWriter writer)
        {
            ThrowIfWriterNull(writer);

            writer.Write((int)Reason);

            writer.Write(Unknown1);

            writer.Write(Hostname);
            writer.Write(DownloadRootUrl);

            writer.Write(Unknown2);
            writer.Write(Unknown3);
            writer.Write(Unknown4);

            writer.Write(Unknown5);

            writer.Write(FingerprintJsonString); //TODO: Implmenent compression

            writer.Write(Unknown5);
            writer.Write(Unknown6);
            writer.Write(Unknown7);
        }
    }
}
