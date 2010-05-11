using System;
using System.ServiceModel;

namespace ContactManager.Interface
{
    [ServiceContract( CallbackContract = typeof(IEntityCallback), SessionMode = SessionMode.Required )]
    public interface IEntityService
    {
        [OperationContract]
        void UpdateEntity( EntityData entity );

        [OperationContract]
        void DeleteEntity( Guid entityId );

        [OperationContract]
        EntityData[] GetEntities( EntityType entityType );
    }
}