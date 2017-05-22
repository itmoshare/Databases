using System;
using System.Runtime.Remoting;
using Neo4jClient;

namespace RestApi.Core
{
    public class GraphClientCluster
    {
        private readonly Uri[] _uris;
        private int _lastActive = 0;
        private GraphClient _currentConnected;

        private readonly string _username;
        private readonly string _password;

        public GraphClientCluster(Uri[] uris, string username = null, string password = null)
        {
            _uris = uris;
            _username = username;
            _password = password;
        }

        public GraphClient GetActive()
        {
            if (_currentConnected != null && _currentConnected.IsConnected)
                return _currentConnected;

            _currentConnected = FindAlive();
            if (_currentConnected == null)
                throw new RemotingTimeoutException("No active servers");
            return _currentConnected;
        }

        private GraphClient FindAlive()
        {
            // We need to find alive server from all _uris list
            int i = _lastActive;
            do
            {
                try
                {
                    var cur = new GraphClient(_uris[i], _username, _password);
                    cur.Connect();
                    return cur;
                }
                catch (Exception e)
                {
                    // Go to next server
                    i++;
                    i %= _uris.Length;
                }
            } while (i != _lastActive);
            return null;
        }
    }
}
