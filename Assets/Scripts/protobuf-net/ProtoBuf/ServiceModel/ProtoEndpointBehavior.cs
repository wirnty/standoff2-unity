using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ProtoBuf.ServiceModel
{
	public class ProtoEndpointBehavior : IEndpointBehavior
	{
		void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
		{
		}

		void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
		{
			ReplaceDataContractSerializerOperationBehavior(endpoint);
		}

		void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
		{
			ReplaceDataContractSerializerOperationBehavior(endpoint);
		}

		void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
		{
		}

		private static void ReplaceDataContractSerializerOperationBehavior(ServiceEndpoint serviceEndpoint)
		{
			foreach (OperationDescription operation in serviceEndpoint.Contract.Operations)
			{
				ReplaceDataContractSerializerOperationBehavior(operation);
			}
		}

		private static void ReplaceDataContractSerializerOperationBehavior(OperationDescription description)
		{
			DataContractSerializerOperationBehavior val = description.Behaviors.Find<DataContractSerializerOperationBehavior>();
			if (val != null)
			{
				description.Behaviors.Remove((IOperationBehavior)val);
				ProtoOperationBehavior protoOperationBehavior = new ProtoOperationBehavior(description);
				((DataContractSerializerOperationBehavior)protoOperationBehavior).MaxItemsInObjectGraph = val.MaxItemsInObjectGraph;
				description.Behaviors.Add((IOperationBehavior)(object)protoOperationBehavior);
			}
		}
	}
}
