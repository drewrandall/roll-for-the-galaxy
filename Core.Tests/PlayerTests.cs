﻿using Rftg.Tiles;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Rftg.Phases;

namespace Rftg
{
    [TestClass]
    public class PlayerTests
    {
        Game game;
        Player player;

        [TestInitialize]
        public void BeforeEach()
        {
            game = new Game();
            player = new Player(game);
        }

        [TestMethod]
        public void Starts_With_Two_Home_Dice_In_Citizenry()
        {
            Assert.AreEqual(2, player.Citizenry.Count());
            Assert.IsTrue(player.Citizenry.All(d => d is Dice.Home));
        }

        [TestMethod]
        public void Starts_With_Three_Home_Dice_In_Cup()
        {
            Assert.AreEqual(3, player.Cup.Count());
            Assert.IsTrue(player.Cup.All(d => d is Dice.Home));
        }

        [TestMethod]
        public void Starts_With_1_Credit()
        {
            Assert.AreEqual(1, player.Credits);
        }

        [TestMethod]
        public void Stock_Takes_Two_Credits()
        {
            var startingCredits = player.Credits;

            player.Stock(new object());

            Assert.AreEqual(2, player.Credits - startingCredits);
        }

        [TestMethod]
        public void Gain_Executes_GainPower()
        {
            var mockTile = new Mock<Ownable>();
            var mockGainPower = mockTile.As<GainPower>();

            player.Gain(mockTile.Object);

            mockGainPower.Verify(x => x.GainFor(player));
        }

        [TestMethod]
        public void Stock_Executes_StockPowers()
        {
            var mockTile = new Mock<Ownable>();
            var mockStockPower = mockTile.As<StockPower>();
            player.Tableau.Add(mockTile.Object);
            var die = new AlienArchaeology();

            player.Stock(die);

            mockStockPower.Verify(x => x.Execute(die, player));
        }
    }
}