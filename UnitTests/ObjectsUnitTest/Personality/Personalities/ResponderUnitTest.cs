using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Objects.Global;
using Objects.Mob.Interface;
using Objects.Personality.Personalities;
using Objects.Personality.Personalities.ResponderMisc.Interface;
using System.Collections.Generic;

namespace ObjectsUnitTest.Personality.Personalities
{
    [TestClass]
    public class ResponderUnitTest
    {
        Responder responder;
        Mock<INonPlayerCharacter> npc;
        Mock<IResponse> response;
        List<IOptionalWords> optionalWords;

        [TestInitialize]
        public void Setup()
        {
            GlobalReference.GlobalValues = new GlobalValues();

            responder = new Responder();
            npc = new Mock<INonPlayerCharacter>();
            response = new Mock<IResponse>();
            optionalWords = new List<IOptionalWords>();

            responder.NonPlayerCharacter = npc.Object;
            npc.SetupSequence(e => e.DequeueMessage()).Returns("<Communication>mobName tells you hello there</Communication>")
                                                    .Returns("<Communication>mobName says hello there</Communication>")
                                                    .Returns("<Communication>mobName shouts hello there</Communication>");
            response.Setup(e => e.Message).Returns("returnMessge");
            response.Setup(e => e.RequiredWordSets).Returns(optionalWords);
            response.Setup(e => e.Match(new List<string>() { "hello", "there" })).Returns(true);
            response.Setup(e => e.Message).Returns("matched message");
            responder.Responses.Add(response.Object);
        }

        [TestMethod]
        public void Responder_Process_ThreeMessage()
        {
            string command = responder.Process(npc.Object, null);
            Assert.AreEqual("tell mobName matched message", command);
            command = responder.Process(npc.Object, null);
            Assert.AreEqual("say matched message", command);
            command = responder.Process(npc.Object, null);
            Assert.AreEqual(null, command);
        }
    }
}
