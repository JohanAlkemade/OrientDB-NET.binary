﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Orient.Client.API.Types;
using Orient.Client.Protocol.Serializers;

namespace Orient.Client.Protocol.Operations
{
    internal class RecordCreate : BaseOperation
    {
        private readonly ODocument _document;
        //private readonly ODatabase _database;
        internal OperationMode OperationMode { get; set; }

        public RecordCreate(ODocument document, ODatabase database)
            :base(database)
        {
            _document = document;
            _database = database;
        }

        public override Request Request(Request request)
        {
            if (_document.ORID != null)
                throw new InvalidOperationException();

            CorrectClassName();

            var clusterId = _database.GetClusterIdFor(_document.OClassName);
            _document.ORID = new ORID(clusterId, -1);

            // standard request fields
            request.AddDataItem((byte)OperationType.RECORD_CREATE);
            request.AddDataItem(request.SessionId);

            if (OClient.ProtocolVersion < 24)
            {
                request.AddDataItem((int)-1);  // data segment id
            }



            request.AddDataItem((short)clusterId);
            request.AddDataItem(Serializer.Serialize(_document));
            request.AddDataItem((byte)ORecordType.Document);
            request.AddDataItem((byte)((OperationMode == OperationMode.Synchronous) ? 0 : 1));


            return request;
        }

        private void CorrectClassName()
        {
            if (_document.OClassName == "OVertex")
                _document.OClassName = "V";
            if (_document.OClassName == "OEdge")
                _document.OClassName = "E";
        }

        public override ODocument Response(Response response)
        {
            ODocument responseDocument = _document;


            if (response == null)
            {
                return responseDocument;
            }

            var reader = response.Reader;

            if (OClient.ProtocolVersion > 25)
                _document.ORID.ClusterId = reader.ReadInt16EndianAware();

            _document.ORID.ClusterPosition = reader.ReadInt64EndianAware();

            if (OClient.ProtocolVersion >= 11)
            {
                _document.OVersion = reader.ReadInt32EndianAware();
            }

            if (_database.ProtocolVersion > 21)
            {
                int collectionChangesCount = reader.ReadInt32EndianAware();
                for (var i = 0; i < collectionChangesCount; i++)
                {
                    throw new NotImplementedException("Collection changes not yet handled - failing rather than ignoring potentially significant information");
                    //var mostSigBits = reader.ReadInt64EndianAware();
                    //var leastSigBits = reader.ReadInt64EndianAware();
                    //var updatedFileId = reader.ReadInt64EndianAware();
                    //var updatedPageIndex = reader.ReadInt64EndianAware();
                    //var updatedPageOffset = reader.ReadInt32EndianAware();
                }
            }
            return responseDocument;


        }

    }
}
