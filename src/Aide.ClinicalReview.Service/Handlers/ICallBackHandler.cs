using Aide.ClinicalReview.Contracts.Messages;
using Monai.Deploy.Messaging.Messages;

namespace Aide.ClinicalReview.Service.Handler
{
    public interface ICallBackHandler<T>
    {
        Task HandleMessage(JsonMessage<T> message);
    }
}