using Microsoft.AspNetCore.Authentication;
using Microsoft.Azure.Cosmos;

public interface ISessionStore
{
    void StoreSession(string sessionId, UserSession session);
    UserSession RetrieveSession(string sessionId);
    void RemoveSession(string sessionId);
}

public class UserSession
{
    public string SessionId { get; set; }

    public string AccessToken { get; set; }

    public string IdToken { get; set; }

}

public class InMemorySessionStore : ISessionStore
{
    private readonly Dictionary<string, UserSession> _sessions = new Dictionary<string, UserSession>();

    public void StoreSession(string sessionId, UserSession session)
    {
        _sessions[sessionId] = session;
    }

    public UserSession RetrieveSession(string sessionId)
    {
        _sessions.TryGetValue(sessionId, out var session);
        return session;
    }

    public void RemoveSession(string sessionId)
    {
        _sessions.Remove(sessionId);
    }
}