using CTrade.Client.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTrade.Client.DataAccess.Test
{
    [TestClass]
    public abstract class TestBase
    {
        private readonly string _connectionKey;
        private readonly IDbRepository _dbRepository;

        protected TestBase() : this(Constants.DefaultConnectionKey) { }

        protected TestBase(string connectionKey)
        {
            _connectionKey = connectionKey;
            _dbRepository = TestRuntime.CreateRepository(connectionKey);
        }

        protected IDbRepository Repository
        {
            get { return _dbRepository; }
        }

        protected string ConnectionKey
        {
            get { return _connectionKey; }
        }

        [TestCleanup]
        public virtual void Cleanup()
        {
            TestRuntime.EnsureCleanup(_connectionKey);
        }
    }
}
