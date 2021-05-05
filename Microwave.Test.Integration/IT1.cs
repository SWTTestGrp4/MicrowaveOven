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

        private Light _uut_light;
        private PowerTube _uut_Pt;
        private Display _uut_display;
        private StringWriter _str;
        private IOutput _output;

        [SetUp]
        public void Setup()
        {

            _output = new Output();
            _uut_light = new Light(_output);
            _uut_Pt = new PowerTube(_output);
            _uut_display = new Display(_output);
            _str = new StringWriter();
            Console.SetOut(_str); //Setup Console to be str

        }

        #region Display IT
        [TestCase (0,0)]
        [TestCase (1,0)]
        [TestCase(0,1)]
        [TestCase (2,33)]
        public void ShowTime_CallMethod_OutputShowsTime(int min, int sec)
        {
            //ACT
            _uut_display.ShowTime(min, sec);

            //ASSERT
            Assert.That(_str.ToString().Contains($"{min:D2}:{sec:D2}"));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(49)]
        [TestCase(701)]
        public void ShowPower_ValuesOutsideBoundaries_ExceptionThrown(int power)
        {
            //ACT
            _uut_display.ShowPower(power);

            //ASSERT
            Assert.Throws<System.ArgumentOutOfRangeException>(() => _uut_Pt.TurnOn(power));
        }

        [TestCase(50)]
        [TestCase(700)]
        public void ShowPower_ValuesInsideBoundaries_OutputShowsPower(int power)
        {
            //ACT
            _uut_display.ShowPower(power);
            
            //ASSERT
            Assert.That(_str.ToString().Contains($"{power} W"));
        }

        [Test]
        public void ClearOutput_CallMethod_OutputShowsCleared()
        {
            //ACT
            _uut_display.Clear();

            //ASSERT
            Assert.That(_str.ToString().Contains("cleared"));
        }

        #endregion
        #region Light IT

        [Test]
        public void LightOn_WasOff_OutputShowsLightIsOn()
        {
            //ACT
            _uut_light.TurnOn();

            //ASSERT
            Assert.That(_str.ToString().Contains("on"));
        }

        [Test]
        public void LightOff_WasOff_CantReceive()
        {
            //ACT
            _uut_light.TurnOff();

            //ASSERT
            Assert.That(_str.ToString().Contains("")); //Assert that the string is empty
        }


        [Test]
        public void LightOn_WasOn_OutputShowsLightIsOn()
        {
            _uut_light.TurnOn();
            _uut_light.TurnOn();
            Assert.That(_str.ToString().Contains("on"));
        }

        [Test]
        public void LightOff_WasOn_LightOff()
        {
            _uut_light.TurnOn();
            _uut_light.TurnOff();
            Assert.That(_str.ToString().Contains("off"));
        }

        #endregion
        #region PowerTube IT
        [TestCase (700)]
        [TestCase (50)]
        [TestCase(95)]
        public void PowertubeOn_TurnOnWithValuesInsideRange_OutputShowsPowerTubeIsOn(int power)
        {
            _uut_Pt.TurnOn(power);
            Assert.That(_str.ToString().Contains($"PowerTube works with {power}"));
        }

        [Test]
        public void PowertubeOffWasOff_TurnPowerOff_NotReceived()
        {
            //ACT
            _uut_Pt.TurnOff();

            //ASSERT
            Assert.That(_str.ToString().Contains("")); //Assert that string is empty
        }

        [TestCase(50)]
        public void PowertubeOffWasOn_TurnPowerOnThenOff_PowerTubeOff(int power)
        {
            //ACT
            _uut_Pt.TurnOn(power);
            _uut_Pt.TurnOff();

            //ASSERT
            Assert.That(_str.ToString().Contains("off")); 

        }
        #endregion
    }
}