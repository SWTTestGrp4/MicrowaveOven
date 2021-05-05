using System;
using System.IO;
using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System.Threading;
using Timer = Microwave.Classes.Boundary.Timer;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT2
    {
        private ICookController _ctrl;
        private IPowerTube _powerTube;
        private ITimer _timer;
        private IDisplay _display;
        private IOutput _output;
        private IUserInterface _ui;
        private StringWriter str;


        [SetUp]
        public void Setup()
        {
            str = new StringWriter();
            Console.SetOut(str);
            _output = new Output();
            _timer = new Timer();
            _ui = Substitute.For<IUserInterface>();
            _display = new Display(_output);
            _powerTube = new PowerTube(_output);
            _ctrl = new CookController(_timer, _display, _powerTube, _ui);
        }

        [TestCase(51, 10)]
        [TestCase(50, 10)]
        [TestCase(700, 10)]
        public void CookCtrl_StartCooking_PowerTubeStartedWithCorrectPower(int power, int time)
        {
            //ACT
            _ctrl.StartCooking(power, time);

            //ASSERT
            Assert.That(str.ToString().Contains($"PowerTube works with {power}"));
        }

        [TestCase(0, 10)]
        [TestCase(1, 10)]
        [TestCase(701, 10)]
        public void CookCtrl_StartCooking_PowerTubeNotStartedIncorrectPower(int power, int time)
        {
            //ACT - ASSERT
            Assert.Throws<System.ArgumentOutOfRangeException>(() => _ctrl.StartCooking(power, time));
        }

        [TestCase(80, 10)]
        [TestCase(80, 90)]
        [TestCase(80, 60)]
        public void CookCtrl_StartCooking_TimerStartedWithCorrectTime(int power, int time)
        {
            //ACT
            _ctrl.StartCooking(power, time);

            //ASSERT
            Assert.That(_timer.TimeRemaining, Is.EqualTo(time * 1000)); //times 1000 to turn sec into millisec
        }

        [TestCase(100, 60)]
        public void CookCtrl_StartCookingTimerExpired_UIReceivedCookingIsDone(int power, int time)
        {
            //ACT
            _ctrl.StartCooking(power, time);
            //Når der er gået noget tid, ses om der blev received cooking is done.
            Thread.Sleep(61000);


            //ASSERT
            _ui.Received(1).CookingIsDone();
        }

        [TestCase(100, 60)]
        public void CookCtrl_StartCookingTimerExpired_PowerTubeTurnedOff(int power, int time)
        {
            //ACT
            _ctrl.StartCooking(power, time);
            Thread.Sleep(61000);

            //ASSERT
            Assert.That(str.ToString().Contains($"PowerTube turned off"));
        }

        [TestCase(100, 60)]
        public void CookCtrl_StartCooking_OnTimerTick_DisplayShowsCorrectTime(int power, int time)
        {
            //ACT
            _ctrl.StartCooking(power, time);
            Thread.Sleep(1500);

            //ASSERT
            Assert.That(str.ToString().Contains("Display shows: 00:59"));
        }

        [TestCase(100, 60)]
        public void CookCtrl_StartCookingAndStopPrematurely_UIdidNotReceiveCookingIsDone(int power, int time)
        {
            //ACT
            _ctrl.StartCooking(power, time);
            _ctrl.Stop(); //opened door or pressed cancel on microwave

            //ASSERT
            _ui.DidNotReceive().CookingIsDone();
        }
        [TestCase(100, 60)]
        public void CookCtrl_StartCookingAndStopPrematurely_PowerTubeTurnedOff(int power, int time)
        {
            //ACT
            _ctrl.StartCooking(power, time);
            _ctrl.Stop(); //opened door or pressed cancel on microwave

            //ASSERT
            Assert.That(str.ToString().Contains($"PowerTube turned off"));
        }

    }
}