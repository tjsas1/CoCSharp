﻿using CoCSharp.Networking;
using NUnit.Framework;
using System;
using System.Net.Sockets;

namespace CoCSharp.Test.Networking
{
    [TestFixture]
    public class SocketAsyncEventArgsPoolTests
    {
        [Test]
        public void TestSocketAsyncEventArgsPoolConstructors()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SocketAsyncEventArgsPool(-1));
        }

        [Test]
        public void TestSocketAsyncEventArgsPoolPushPopDisposed()
        {
            var pool = new SocketAsyncEventArgsPool(64);

            // Populate the pool.
            for (int i = 0; i < 64; i++)
                pool.Push(new SocketAsyncEventArgs());

            // Remove a couple of the args.
            for (int i = 0; i < 32; i++)
                pool.Pop();

            pool.Dispose();

            Assert.Throws<ObjectDisposedException>(() => pool.Push(new SocketAsyncEventArgs()));
            Assert.Throws<ObjectDisposedException>(() => pool.Pop());
        }

        [Test]
        public void TestSocketAsyncEventArgsPoolRange()
        {
            var pool = new SocketAsyncEventArgsPool(1);

            // Populate the pool with one extra.
            pool.Push(new SocketAsyncEventArgs());
            Assert.Throws<InvalidOperationException>(() => pool.Push(new SocketAsyncEventArgs()));

            // Remove all th args with one extra.
            pool.Pop();
            Assert.Throws<InvalidOperationException>(() => pool.Pop());
        }
    }
}
