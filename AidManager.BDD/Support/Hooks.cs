using TechTalk.SpecFlow;

namespace AidManager.BDD.Support
{
    [Binding]
    public class Hooks
    {
        private readonly World _world;
        public Hooks(World world) => _world = world;

        [BeforeScenario]
        public void BeforeScenario()
        {
            _world.Factory = new AidManagerFactory();
            _world.Client = _world.Factory.CreateClient();
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _world.Client?.Dispose();
            _world.Factory?.Dispose();
        }
    }
}
