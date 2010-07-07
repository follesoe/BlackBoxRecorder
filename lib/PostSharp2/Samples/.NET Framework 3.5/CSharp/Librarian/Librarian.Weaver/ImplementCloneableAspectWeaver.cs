#region Released to Public Domain by Gael Fraiteur

/*----------------------------------------------------------------------------*
 *   This file is part of samples of PostSharp.                                *
 *                                                                             *
 *   This sample is free software: you have an unlimited right to              *
 *   redistribute it and/or modify it.                                         *
 *                                                                             *
 *   This sample is distributed in the hope that it will be useful,            *
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of            *
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.                      *
 *                                                                             *
 *----------------------------------------------------------------------------*/

#endregion

using System;
using System.Reflection;
using Librarian.Framework;
using PostSharp;
using PostSharp.Aspects;
using PostSharp.Sdk;
using PostSharp.Sdk.AspectInfrastructure;
using PostSharp.Sdk.AspectWeaver;
using PostSharp.Sdk.AspectWeaver.AspectWeavers;
using PostSharp.Sdk.AspectWeaver.Transformations;
using PostSharp.Sdk.CodeModel;
using PostSharp.Sdk.CodeModel.Helpers;
using PostSharp.Sdk.Collections;
using PostSharp.Extensibility;

namespace Librarian.Weaver
{
    /// <summary>
    /// Generates code (specifically the <see cref="BaseEntity.CopyTo"/> method) for the 'Cloneable' aspect.
    /// </summary>
    internal class ImplementCloneableAspectWeaver : TypeLevelAspectWeaver
    {
        private ImplementCloneableAspectTransformation transformation;

        public ImplementCloneableAspectWeaver() : base( null, MulticastTargets.Class )
        {
        }

        protected override void Initialize()
        {
            base.Initialize();

            this.transformation = new ImplementCloneableAspectTransformation( this);
            this.ApplyWaivedEffects( this.transformation );
        }

        protected override AspectWeaverInstance CreateAspectWeaverInstance( AspectInstanceInfo aspectInstanceInfo )
        {
            return new ImplementCloneableAspectWeaverInstance( this, aspectInstanceInfo );
        }


        private class ImplementCloneableAspectWeaverInstance : TypeLevelAspectWeaverInstance
        {
            private readonly ImplementCloneableAspectWeaver parent;

            public ImplementCloneableAspectWeaverInstance(ImplementCloneableAspectWeaver parent, AspectInstanceInfo aspectInstanceInfo)
                : base( parent, aspectInstanceInfo )
            {
                this.parent = parent;
            }

            public override void ProvideAspectTransformations( AspectWeaverTransformationAdder adder )
            {
                adder.Add( this.TargetElement, this.parent.transformation.CreateInstance( this ) );
            }
        }
    }

    internal class ImplementCloneableAspectTransformation : StructuralTransformation
    {
        public ImplementCloneableAspectTransformation( AspectWeaver aspectWeaver)
            : base( aspectWeaver )
        {
        }


        public AspectWeaverTransformationInstance CreateInstance( AspectWeaverInstance aspectWeaverInstance )
        {
            return new ImplementCloneableAspectTransformationInstance( this, aspectWeaverInstance );
        }

        private class ImplementCloneableAspectTransformationInstance : StructuralTransformationInstance
        {
            public ImplementCloneableAspectTransformationInstance( StructuralTransformation parent, AspectWeaverInstance aspectWeaverInstance )
                : base( parent, aspectWeaverInstance )
            {
            }


            public override void Implement( TransformationContext context )
            {
                TypeDefDeclaration typeDef = (TypeDefDeclaration) context.TargetElement;

                ModuleDeclaration module = this.AspectWeaver.Module;
                ITypeSignature baseEntityType = module.Cache.GetType( typeof(BaseEntity) );

                // Find the base method.
                IMethod baseCopyToMethod = null;
                IType baseTypeCursor = typeDef;
                MethodSignature methodSignature =
                    new MethodSignature(module, CallingConvention.HasThis, module.Cache.GetIntrinsic(IntrinsicType.Void),
                                         new[] { baseEntityType }, 0);

                while (baseCopyToMethod == null && baseTypeCursor != null)
                {
                    TypeDefDeclaration baseTypeCursorTypeDef = baseTypeCursor.GetTypeDefinition();

                    baseCopyToMethod =
                        baseTypeCursorTypeDef.Methods.GetMethod("CopyTo",
                                                                 methodSignature.Translate(baseTypeCursorTypeDef.Module),
                                                                 BindingOptions.OnlyExisting |
                                                                 BindingOptions.DontThrowException);

                    baseTypeCursor = baseTypeCursorTypeDef.BaseType;
                }

                if (baseCopyToMethod == null)
                    throw new AssertionFailedException("Could not find a method CopyTo.");

                if ( baseCopyToMethod.DeclaringType == typeDef )
                    return;


                // Declare the method.
                MethodDefDeclaration methodDef = new MethodDefDeclaration
                                                     {
                                                         Name = "CopyTo",
                                                         Attributes =
                                                             (MethodAttributes.Family | MethodAttributes.ReuseSlot |
                                                              MethodAttributes.Virtual),
                                                         CallingConvention = CallingConvention.HasThis
                                                     };
                typeDef.Methods.Add( methodDef );
                methodDef.CustomAttributes.Add( this.AspectWeaver.AspectInfrastructureTask.WeavingHelper.GetDebuggerNonUserCodeAttribute() );

                // Define parameter.
                methodDef.ReturnParameter = new ParameterDeclaration
                                                {
                                                    ParameterType = module.Cache.GetIntrinsic( IntrinsicType.Void ),
                                                    Attributes = ParameterAttributes.Retval
                                                };

                ParameterDeclaration cloneParameter =
                    new ParameterDeclaration( 0, "clone", baseEntityType );
                methodDef.Parameters.Add( cloneParameter );

                // Define the body
                MethodBodyDeclaration methodBody = new MethodBodyDeclaration();
                methodDef.MethodBody = methodBody;
                InstructionBlock instructionBlock = methodBody.CreateInstructionBlock();
                methodBody.RootInstructionBlock = instructionBlock;
                InstructionSequence sequence = methodBody.CreateInstructionSequence();
                instructionBlock.AddInstructionSequence( sequence, NodePosition.After, null );
                using (InstructionWriter writer = new InstructionWriter())
                {
                    writer.AttachInstructionSequence( sequence );

                    // Cast the argument and store it in a local variable.
                    IType typeSpec = GenericHelper.GetTypeCanonicalGenericInstance( typeDef );
                    LocalVariableSymbol castedCloneLocal = instructionBlock.DefineLocalVariable( typeSpec, "typedClone" );
                    writer.EmitInstruction( OpCodeNumber.Ldarg_1 );
                    writer.EmitInstructionType( OpCodeNumber.Castclass, typeSpec );
                    writer.EmitInstructionLocalVariable( OpCodeNumber.Stloc, castedCloneLocal );

                  
                    // TODO: support generic base types.


                    // Call the base method.
                    writer.EmitInstruction( OpCodeNumber.Ldarg_0 );
                    writer.EmitInstruction( OpCodeNumber.Ldarg_1 );
                    writer.EmitInstructionMethod( OpCodeNumber.Call, (IMethod) baseCopyToMethod.Translate( typeDef.Module ) );

                    // Loop on all fields and clone cloneable ones.
                    TypeRefDeclaration cloneableTypeRef = (TypeRefDeclaration)
                                                          module.Cache.GetType( typeof(ICloneable) );
                    MethodRefDeclaration cloneMethodRef = (MethodRefDeclaration) cloneableTypeRef.MethodRefs.GetMethod(
                                                                                     "Clone",
                                                                                     new MethodSignature(
                                                                                         module,
                                                                                         CallingConvention.HasThis,
                                                                                         module.Cache.GetIntrinsic(
                                                                                             IntrinsicType.Object ),
                                                                                         new ITypeSignature[0], 0 ),
                                                                                     BindingOptions.Default );

                    foreach ( FieldDefDeclaration fieldDef in typeDef.Fields )
                    {
                        if ( (fieldDef.Attributes & FieldAttributes.Static) != 0 )
                            continue;

                        if ( fieldDef.FieldType == module.Cache.GetIntrinsic( IntrinsicType.String ) )
                            continue;

                        // Does not work?
                        //bool cloneable = fieldDef.FieldType.Inherits(cloneableTypeRef, GenericMap.Empty);
                        bool cloneable = typeof(ICloneable).IsAssignableFrom( fieldDef.FieldType.GetSystemType( null, null ) );


                        if ( cloneable )
                        {
                            IField fieldSpec = GenericHelper.GetFieldCanonicalGenericInstance( fieldDef );
                            bool isValueType =
                                fieldSpec.FieldType.BelongsToClassification( TypeClassifications.ValueType ).Equals(
                                    NullableBool.True );

                            InstructionSequence nextSequence = null;
                            if ( !isValueType )
                            {
                                nextSequence = methodBody.CreateInstructionSequence();
                                writer.EmitInstruction( OpCodeNumber.Ldarg_0 );
                                writer.EmitInstructionField( OpCodeNumber.Ldfld, fieldSpec );
                                writer.EmitBranchingInstruction( OpCodeNumber.Brfalse, nextSequence );
                            }
                            writer.EmitInstructionLocalVariable( OpCodeNumber.Ldloc, castedCloneLocal );
                            writer.EmitInstruction( OpCodeNumber.Ldarg_0 );
                            writer.EmitInstructionField( OpCodeNumber.Ldfld, fieldSpec );
                            if ( isValueType )
                            {
                                writer.EmitInstructionType( OpCodeNumber.Box, fieldSpec.FieldType );
                            }
                            //writer.EmitInstructionType(OpCodeNumber.Castclass, cloneableTypeRef);
                            writer.EmitInstructionMethod( OpCodeNumber.Callvirt, cloneMethodRef );
                            if ( isValueType )
                            {
                                writer.EmitInstructionType( OpCodeNumber.Unbox, fieldSpec.FieldType );
                                writer.EmitInstructionType( OpCodeNumber.Ldobj, fieldSpec.FieldType );
                            }
                            else
                            {
                                writer.EmitInstructionType( OpCodeNumber.Castclass, fieldSpec.FieldType );
                            }
                            writer.EmitInstructionField( OpCodeNumber.Stfld, fieldSpec );


                            if ( !isValueType )
                            {
                                writer.DetachInstructionSequence();
                                instructionBlock.AddInstructionSequence( nextSequence, NodePosition.After, sequence );
                                sequence = nextSequence;
                                writer.AttachInstructionSequence( sequence );
                            }
                        }
                    }

                    writer.EmitInstruction( OpCodeNumber.Ret );
                    writer.DetachInstructionSequence();
                }
            }
        }

        public override string GetDisplayName( MethodSemantics semantic )
        {
            return "Implement ICloneable";
        }
    }
}