using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using Dependencies.Entities;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;

namespace Dependencies.Aspects
{
    [Serializable]
    [ProvideAspectRole(StandardRoles.Security)]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.Caching)]
    class AuthorizationAttribute : OnMethodBoundaryAspect
    {
        private readonly string role;

        public AuthorizationAttribute( string role )
        {
            this.role = role;
        }

        public override bool CompileTimeValidate(System.Reflection.MethodBase method)
        {
            if ( !typeof(ISecurable).IsAssignableFrom(method.DeclaringType))
            {
                Message.Write(SeverityType.Warning, "CUSTOM01", "Cannot apply AuthorizationAttribute on a method of type {0} because the type does not implement ISecurable.",
                              method.DeclaringType);
                return false;
            }

            if ( method.IsStatic )
            {
                Message.Write(SeverityType.Warning, "CUSTOM01", "Cannot apply AuthorizationAttribute on a method {0}.{1} because the method is static.",
                              method.DeclaringType, method);
                return false;
            }

            return base.CompileTimeValidate(method);
        }

        public override void OnEntry(MethodExecutionArgs args)
        {
            if ( !((ISecurable) args.Instance).IsUserInRole( User.Current, this.role  ) )
                throw new SecurityException();
        }
    }
}