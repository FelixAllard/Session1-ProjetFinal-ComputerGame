
namespace Communication.ControllerPc
{
    public interface IRemoteInputController
    {
        void OnButtonA();
        void OnButtonB();
        void OnButtonC();
        void OnButtonD();
        void ReceivedMessageFromRobot(string message);
        
        
    }
}