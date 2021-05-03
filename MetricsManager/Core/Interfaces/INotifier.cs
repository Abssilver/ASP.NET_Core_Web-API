namespace Core.Interfaces
{
    public interface INotifier
    {
        void Notify();

        bool CanRun();
    }
}