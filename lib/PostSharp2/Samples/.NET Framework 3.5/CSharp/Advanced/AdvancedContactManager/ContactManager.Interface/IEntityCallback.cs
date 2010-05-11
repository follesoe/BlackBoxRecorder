using System;
using System.ServiceModel;

namespace ContactManager.Interface
{
    [ServiceContract]
    public interface IEntityCallback
    {
        [OperationContract( IsOneWay = true )]
        void OnEntityDeleted( Guid id );

        [OperationContract( IsOneWay = true )]
        void OnEntityUpdated( EntityData entity );

        [OperationContract( IsOneWay = true )]
        void OnEntityCreated( EntityData entity );
    }
}