using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestMetaIt.Controllers;
using RestMetaIt.Model;
using System.Collections.Generic;

namespace UnitTestRestMetaIt
{
    [TestClass]
    public class UnitTestMetaIt
    {
        MessagesController _MessagesController;

        [TestInitialize]
        public void Initialize()
        {
            MemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions());
            memoryCache.Set("MetaItMessages", new List<MetaItMessage>() { new MetaItMessage() { Msg = "Test", Ts = 0} });
            _MessagesController = new MessagesController(memoryCache, null);           
        }

        [TestCleanup]
        public void Cleanup()
        {
            _MessagesController = null;
        }


        [TestMethod]
        public void Get()
        {
            // Nemám k dipozici VS s podporou Web Performance, pøikládám jinou ukázku testu
            var result = _MessagesController.Get();       
            Assert.AreEqual(((List<MetaItMessage>)result.Value)?[0].Msg, "Test");
        }
    }
}
