﻿//
// TorrentDataExtensionsTests.cs
//
// Authors:
//   Alan McGovern alan.mcgovern@gmail.com
//
// Copyright (C) 2020 Alan McGovern
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using MonoTorrent.BEncoding;
using MonoTorrent.Client.Connections;
using MonoTorrent.Client.Messages;
using MonoTorrent.Client.Messages.Standard;

using NUnit.Framework;

namespace MonoTorrent.Client
{
    [TestFixture]
    public class TorrentDataExtensionsTests
    {
        class Data : ITorrentData
        {
            public IList<ITorrentFileInfo> Files { get; set; }
            public int PieceLength { get; set; }
            public long Size { get; set; }
        }

        [Test]
        public void BlocksPerPiece ()
        {
            Assert.AreEqual (2, new Data { Size = Piece.BlockSize * 2, PieceLength = Piece.BlockSize * 2 }.BlocksPerPiece (0));
            Assert.AreEqual (2, new Data { Size = Piece.BlockSize * 4, PieceLength = Piece.BlockSize * 2 }.BlocksPerPiece (1));
            Assert.AreEqual (1, new Data { Size = Piece.BlockSize * 4 + 1, PieceLength = Piece.BlockSize * 2 }.BlocksPerPiece (2));
            Assert.AreEqual (1, new Data { Size = Piece.BlockSize * 5 - 1, PieceLength = Piece.BlockSize * 2 }.BlocksPerPiece (2));
        }

        [Test]
        public void BytesPerPiece ()
        {
            Assert.AreEqual (Piece.BlockSize * 2, new Data { Size = Piece.BlockSize * 4, PieceLength = Piece.BlockSize * 2 }.BytesPerPiece (0));
            Assert.AreEqual (Piece.BlockSize * 2, new Data { Size = Piece.BlockSize * 4, PieceLength = Piece.BlockSize * 2 }.BytesPerPiece (1));

            Assert.AreEqual (1, new Data { Size = Piece.BlockSize * 4 + 1, PieceLength = Piece.BlockSize * 2 }.BytesPerPiece (2));
            Assert.AreEqual (Piece.BlockSize - 1, new Data { Size = Piece.BlockSize * 5 - 1, PieceLength = Piece.BlockSize * 2 }.BytesPerPiece (2));
        }

        [Test]
        public void PieceCount ()
        {
            Assert.AreEqual (2, new Data { Size = Piece.BlockSize * 2 + 1, PieceLength = Piece.BlockSize * 2 }.PieceCount ());
            Assert.AreEqual (2, new Data { Size = Piece.BlockSize * 4 - 1, PieceLength = Piece.BlockSize * 2 }.PieceCount ());
            Assert.AreEqual (2, new Data { Size = Piece.BlockSize * 4, PieceLength = Piece.BlockSize * 2 }.PieceCount ());
            Assert.AreEqual (3, new Data { Size = Piece.BlockSize * 4 + 1, PieceLength = Piece.BlockSize * 2 }.PieceCount ());
        }
    }
}