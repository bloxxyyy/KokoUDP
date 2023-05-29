namespace Koko_ClientServer;
public class TestComponent : NetworkComponent {
    public int x = 1;
    public int y = 3;
    public int z = 5;
    public string fun = "haha";
}

public class TestComponent2 : NetworkComponent {
    public int r = 10;
    public int g = 50;
    public string haha = "haha2";
}

public abstract class NetworkComponent {
    public Type type;

    protected NetworkComponent() {
        type = GetType();
    }
}
