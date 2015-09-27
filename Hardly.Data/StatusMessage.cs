namespace Hardly
{
    public class StatusMessage
    {
        public readonly string message;
        public readonly bool success;
        
        public StatusMessage(string message, bool success = true)
        {
            this.message = message;
            this.success = success;
        }
    }
}
