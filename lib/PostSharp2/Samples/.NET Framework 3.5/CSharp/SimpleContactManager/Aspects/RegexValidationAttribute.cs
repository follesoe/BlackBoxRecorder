using System;
using System.Text.RegularExpressions;
using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Extensibility;
using PostSharp.Reflection;

namespace ContactManager.Aspects
{
    [Serializable]
    [ProvideAspectRole(StandardRoles.Validation)]
    [AspectRoleDependency(AspectDependencyAction.Order, AspectDependencyPosition.Before, StandardRoles.DataBinding)]
    public sealed class RegexValidationAttribute : LocationInterceptionAspect
    {
        private readonly string regexStr;
        [NonSerialized] private Regex regex;

        public RegexValidationAttribute( string regexStr )
        {
            this.regexStr = regexStr;
        }

        public override bool CompileTimeValidate(LocationInfo locationInfo)
        {
            // Verify that we have a valid regular expression.
            try
            {
                Regex.IsMatch( "", regexStr );
            }
            catch ( Exception e )
            {
                Message.Write(SeverityType.Error, "CM00003", "Error with [RegexValidation] applied on '{0}': invalid regular expression: {1}",
                    locationInfo, e.Message);
                return false;
                
            }
            
            return base.CompileTimeValidate(locationInfo);
        }

        public override void RuntimeInitialize( LocationInfo locationInfo )
        {
            regex = new Regex( regexStr );
            base.RuntimeInitialize( locationInfo );
        }

        public override void OnSetValue( LocationInterceptionArgs args )
        {
            string s = (string) args.Value;
            if ( s == null )
                throw new ArgumentNullException( args.Location.Name );
            if ( !regex.IsMatch( s ) )
                throw new ArgumentOutOfRangeException( args.Location.Name );

            base.OnSetValue( args );
        }
    }
}