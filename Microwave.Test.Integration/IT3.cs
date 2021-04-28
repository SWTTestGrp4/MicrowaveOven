using Microwave.Classes.Boundary;
using Microwave.Classes.Controllers;
using Microwave.Classes.Interfaces;
using NSubstitute;
using NUnit.Framework;

namespace Microwave.Test.Integration
{
    [TestFixture]
    public class IT3
    {
        private UserInterface _uut;
        private IButton fakeCancelButton;
        private IButton fakePowerButton;
        private IButton fakeTimeButton;
        private IDoor fakeDoor;
        private IOutput _output;
        private IDisplay _display;
        private ILight _light;
        private ITimer _timer;
        private IPowerTube _powerTube;
        private ICookController _cookController;

        [SetUp]
        public void Setup()
        {
            fakeCancelButton = Substitute.For<IButton>();
            fakePowerButton = Substitute.For<IButton>();
            fakeTimeButton = Substitute.For<IButton>();
            fakeDoor = Substitute.For<IDoor>();
            _output = new Output();
            _display = new Display(_output);
            _light = new Light(_output);
            _timer = new Timer();
            _powerTube = new PowerTube(_output);
            _cookController = new CookController(_timer, _display, _powerTube);
            _uut = new UserInterface(fakePowerButton, fakeTimeButton, fakeCancelButton, fakeDoor, _display, _light,
                _cookController);
        }


        [TestCase()]
        public void UserInterface_OnDoorOpenedCalled_LightTurnsOn()
        {
            //Arrange
            fakeDoor.Opened += Raise.EventWith(new object() { });

            //Act

            //Assert
        }

        [TestCase()]
        public void UserInterface_OnDoorOpenedCalled_LightTurnsO()
        {
            //Arrange

            //Act

            //Assert
        }
    }
}