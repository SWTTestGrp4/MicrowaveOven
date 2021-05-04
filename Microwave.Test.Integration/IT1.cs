using Microwave.Classes.Boundary;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;
using System;
using System.IO;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT1
    {

        private Light uut_light;
        private PowerTube uut_Pt;
        private Display uut_display;
        private StringWriter str;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {

            _output = new Output();
            uut_light = new Light(_output);
            uut_Pt = new PowerTube(_output);
            uut_display = new Display(_output);
            str = new StringWriter();
            Console.SetOut(str); //Setup Console to be str

        }

        #region Display IT
        [TestCase (2,33)]
        public void ShowTime_CallMethod_OutputShowsTime(int min, int sec)
        {
            uut_display.ShowTime(min, sec);
            Assert.That(str.ToString().Contains("02:33"));
        }

        [TestCase(3)]
        public void ShowPower_CallMethod_OutputShowsPower(int power)
        {
            uut_display.ShowPower(power);
            Assert.That(str.ToString().Contains("3 W"));
        }

        [Test]
        public void ClearOutput_CallMethod_OutputShowsCleared()
        {
            uut_display.Clear();
            Assert.That(str.ToString().Contains("cleared"));
        }

        #endregion
        #region Light IT

        [Test]
        public void LightOn_WasOff_OutputShowsLightIsOn()
        {
            uut_light.TurnOn();
            Assert.That(str.ToString().Contains("on"));
           // _output.Received(1).OutputLine(Arg.Is<string>(str => str.Contains("on")));
        }

        [Test]
        public void LightOff_WasOff_CantReceive()
        {
            _output = Substitute.For<IOutput>();
            uut_light.TurnOff();
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }


        [Test]
        public void LightOn_WasOn_OutputShowsLightIsOn()
        {
            uut_light.TurnOn();
            uut_light.TurnOn();
            Assert.That(str.ToString().Contains("on"));
        }

        [Test]
        public void LightOff_WasOn_LightOff()
        {
            uut_light.TurnOn();
            uut_light.TurnOff();
            Assert.That(str.ToString().Contains("off"));
        }

        #endregion
        #region PowerTube IT
        [TestCase (5)]
        [TestCase (50)]
        [TestCase(95)]
        public void PowertubeOn_TurnOn_OutputShowsPowerTubeIsOn(int power)
        {
            uut_Pt.TurnOn(power);
            Assert.That(str.ToString().Contains($"PowerTube works with {power}"));
            //TODO I guess I dont need to test on the exceptions since they have nothing to do with integration btween PowerTube and Output?
        }

        [TestCase (5)]
        public void PowertubeOff_TurnPowerOnThenOff_OutputShowsPowerTubeIsOff(int power)
        {
            uut_Pt.TurnOn(power);
            uut_Pt.TurnOff();
            Assert.That(str.ToString().Contains("off"));
            //TODO I guess I dont need to test on the exceptions since they have nothing to do with integration btween PowerTube and Output?
        }

        [Test]
        public void PowertubeOff_TurnPowerOff_NotReceived()
        {
            _output = Substitute.For<IOutput>();
            uut_Pt.TurnOff();
            _output.DidNotReceive().OutputLine(Arg.Is<string>(str => str.Contains("off")));
        }

        #endregion
    }
}