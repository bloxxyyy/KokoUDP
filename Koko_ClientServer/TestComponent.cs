namespace Koko_ClientServer;

public abstract class NetworkComponent {
    public Type type;

    protected NetworkComponent() {
        type = GetType();
    }
}
